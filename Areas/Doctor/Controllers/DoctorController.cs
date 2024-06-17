using Hospital_Management.Areas.Doctor.Data;
using Hospital_Management.Areas.Doctor.Models;
using Hospital_Management.Areas.Patient.Data;
using Hospital_Management.Areas.Patient.Models;
using Hospital_Management.Areas.User.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;


namespace Hospital_Management.Areas.Doctor.Controllers
{
    public class DoctorController : Controller
    {
        layer layer = new layer();
        string connection = "Data Source=_aimerstar_\\HARSHILMSSQL;Initial Catalog=HospitalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public IActionResult DoctorList()
        {
            int i = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            List<DoctorModel> users = new List<DoctorModel>();
            users = layer.GetDoctorProfile(i).ToList();
            return View(users);
        }
        [HttpGet]
        public IActionResult DoctorProfile()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DoctorProfile([Bind] DoctorModel doctor)
        {
            layer.DoctorProfile(doctor, HttpContext);
            return RedirectToAction("DoctorProfileList");
        }
        [HttpGet]
        public IActionResult EditDoctorProfile(int id)
        {
            int i = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.User_Id = i;
            DoctorModel doctor = layer.GetDoctorProfileById(i);
            return View(doctor);
        }
        [HttpPost]
        public IActionResult EditDoctorProfile(int id, [Bind] DoctorModel doctor)
        {
            layer.DoctorProfile(doctor, HttpContext);
            return RedirectToAction("DoctorList", "Doctor");
        }
        [HttpGet]
        public IActionResult DoctorAvailableProfile()
        {
            var id = HttpContext.Session.GetString("userid");
            available doctor = layer.GetDoctorAvailableProfileById(id);
            return View(doctor);

        }
        [HttpPost]
        public IActionResult DoctorAvailableProfile(available model)
        {
            if (ModelState.IsValid)
            {
                if (layer.IsTimeSlotAvailable(model.Name, model.Date, model.STime, model.ETime))
                {
                    layer.DoctorAvailableProfile(model,HttpContext);

                    return RedirectToAction("DoctorAvailableProfileList");
                }
                else
                {
                   ModelState.AddModelError("", "The selected time slot overlaps with existing availability.");
                }
            }

            return View(model);
        }
        public IActionResult DoctorAvailableProfileList()
        {
            string id = HttpContext.Session.GetString("userid");
            List<available> users = new List<available>();
            users = layer.GetDoctorAvailableProfile(id).ToList();
            return View(users);
        }
        public IActionResult AppoinmentList(bool isDone)
        {
            string id = HttpContext.Session.GetString("name");
            List<AppoinmnetModel> appoinmnet = new List<AppoinmnetModel>();
            appoinmnet = layer.GetAppointments(id).ToList();
            ViewData["IsDone"] = isDone;
            return View(appoinmnet);
        }
        [HttpGet]
        public IActionResult resetpassword()
        {
            string id = HttpContext.Session.GetString("userid");
            var model = new resetpasswordviewmodel()
            {
                UserId = id
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult resetpassword([Bind] resetpasswordviewmodel user)
        {
            layer.resetPassword(user);
            return RedirectToAction("Login", "User", new { area = "User" });
        }
        [HttpGet]
        public IActionResult GetAvailableTimes(DateTime date, string doctorName, string startTime, string endTime)
        {
            TimeSpan newStartTime = TimeSpan.Parse(startTime);
            TimeSpan newEndTime = TimeSpan.Parse(endTime);

            bool isAvailable = layer.IsTimeSlotAvailable(doctorName, date, newStartTime, newEndTime);

            if (!isAvailable)
            {
                return Conflict("The provided time slot overlaps with existing availability.");
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost]
        public IActionResult AddAvailableTime(DateTime date, string doctorName, string startTime, string endTime)
        {
            // Convert start and end times to TimeSpan objects
            TimeSpan newStartTime = TimeSpan.Parse(startTime);
            TimeSpan newEndTime = TimeSpan.Parse(endTime);

            // Check if the time slot is available
            bool isAvailable = layer.IsTimeSlotAvailable(doctorName, date, newStartTime, newEndTime);

            if (!isAvailable)
            {
                // Return a conflict status to indicate overlapping time slots
                return Conflict("The provided time slot overlaps with existing availability.");
            }
            else
            {
                // Proceed with adding the new entry
                // Add your code here to insert the new availability entry into the database
                return Ok("The time slot is available and has been successfully added.");
            }
        }
        [HttpPost]
        public IActionResult DeleteAvailability(TimeSpan STime)
        {
            layer.DeleteAvailability(STime);
            return RedirectToAction("DoctorAvailableProfileList");
        }
        [HttpGet]
        public IActionResult GetAvailableTime(DateTime date)
        {
            List<DoctorModel> availableDoctors = new List<DoctorModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = @"
                    SELECT U.[User_ID], 
                           U.[Name], 
                           U.[Designation], 
                           A.[Start_Time], 
                           A.[End_Time], 
                           U.[Phone], 
                           U.[Gender]
                    FROM [dbo].[User_tbl] U
                    JOIN [dbo].[Available_tbl] A ON U.[User_ID] = A.[User_Id]
                    WHERE CONVERT(DATE, @Date) BETWEEN CONVERT(DATE, A.[Date]) AND CONVERT(DATE, A.[Date])";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Date", date);

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DoctorModel user = new DoctorModel();
                            user.User_Id = Convert.ToInt32(reader["User_ID"]);
                            user.Name = reader["Name"].ToString();
                            user.Designation = reader["Designation"].ToString();
                            user.AvailableStartTime = (TimeSpan)reader["Start_Time"];
                            user.AvailableEndTime = (TimeSpan)reader["End_Time"];
                            user.Phone = reader["Phone"].ToString();
                            user.Gender = reader["Gender"].ToString();
                            availableDoctors.Add(user);
                        }
                    }
                }
            }
            return Json(availableDoctors);
        }

        [HttpPost]
        public IActionResult DeleteAppointment(string patientName)
        {
            layer.DeleteAppointment(patientName);
            return RedirectToAction("AppoinmentList");
        }
    }
}

