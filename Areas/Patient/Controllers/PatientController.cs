using Hospital_Management.Areas.Doctor.Models;
using Hospital_Management.Areas.Patient.Data;
using Hospital_Management.Areas.Patient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;

namespace Hospital_Management.Areas.Patient.Controllers
{
    public class PatientController : Controller
    {
        string connection = "Data Source=_aimerstar_\\HARSHILMSSQL;Initial Catalog=HospitalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        private readonly PatientDbContext patientDb;
        public PatientController(PatientDbContext patientDb)
        {
            this.patientDb = patientDb;
        }
        Patient_Layer patient_Layer = new Patient_Layer();
        public IActionResult AppoinmnetList()
        {
            string id = HttpContext.Session.GetString("userid");
            IEnumerable<AppoinmnetModel> appoinmnets = new List<AppoinmnetModel>();
            appoinmnets = patient_Layer.GetAppointments(id,HttpContext).ToList();
            
            return View(appoinmnets);
        }
        public IActionResult GetAvailableTimes(DateTime date, string doctorName, string timePeriod)
        {
            var availableTimes = GetAvailableTimesForDate(date, doctorName, timePeriod);
            
            return Json(new { availableTimes });
        }
        public IActionResult GetAvailableTime(DateTime date, string doctorName, string timePeriod)
        {
            var availableTimes = GetAvailableTimesForDate(date, doctorName, timePeriod);

            return Json(new { availableTimes });
        }
        private List<string> GetAvailableTimesForDate(DateTime date, string doctorName, string timePeriod)
        {
            var pname = HttpContext.Session.GetString("name");
            var availableTimes = new List<string>();
            int t = Convert.ToInt32(timePeriod);
            var availableSlots = GetAvailableSlotsForDate(date, doctorName);
            var a = "select time slot";
            availableTimes.Add(a.ToString());
            foreach (var slot in availableSlots)
            {
                var currentTime = slot.STime;
                var endTime = slot.ETime;
                while (currentTime < endTime)
                {
                    if (!patient_Layer.GetAppointmentsbypatientname(doctorName).Any(u => u.STime <= currentTime && u.ETime > currentTime))
                    {
                        availableTimes.Add(currentTime.ToString(@"hh\:mm"));
                    }
                    currentTime = currentTime.Add(TimeSpan.FromMinutes(t));
                }
            }
            return availableTimes;
        }
        private List<available> GetAvailableSlotsForDate(DateTime date, string doctorName)
        {
            var availableSlots = new List<available>();

            using (var conn = new SqlConnection(connection))
            {
                conn.Open();
                using (var command = new SqlCommand("SELECT Start_Time, End_Time FROM Available_tbl WHERE Date = @date AND Name = @name", conn))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@name", doctorName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var stime = (TimeSpan)reader["Start_Time"];
                            var etime = (TimeSpan)reader["End_Time"];

                            availableSlots.Add(new available { Date = date, STime = stime, ETime = etime });
                        }
                    }
                }
            }
            return availableSlots;
        }
        [HttpGet]
        public IActionResult AddAppoinment(string id)
        {
            string userId = HttpContext.Session.GetString("userid");

            var patient = patient_Layer.GetPatientProfileById(userId);
            var doctor = patient_Layer.GetDoctorAvailableProfileById(id);

            var model = new AppoinmnetModel
            {
                User_Id = userId,
                Patient_Name =patient.UserName,
                Doctor_Name = doctor.Name, 
                Date = doctor.Date.ToString() 
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult AddAppoinment(AppoinmnetModel appoinmnet)
        {
            patient_Layer.AddAppoinment(appoinmnet);
            return RedirectToAction("AppoinmnetList");
        }
        public IActionResult DoctorProfileList()
        {
            string id = "1";
            List<DoctorModel> users = new List<DoctorModel>();
            users = patient_Layer.GetDoctorProfile(id).ToList();
            return View(users);
        }
        public IActionResult details(string id,string name)
        {
            IEnumerable<AppoinmnetModel> appointments = patient_Layer.GetAppointmentsbypatient(id,name);
            return View(appointments);
        }
        [HttpPost]
        public IActionResult details(string Appoinment_Id, bool isDone)
        {
            patient_Layer.UpdateAppointmentStatus(Appoinment_Id, isDone);
            return RedirectToAction("AppoinmentList", "Doctor", new { area = "Doctor" });
        }
        [HttpGet]
        public IActionResult GetDoneStatus(string appointmentId)
        {
            bool isDone = patient_Layer.GetIsDoneStatus(appointmentId);
            return Json(isDone);
        }
        [HttpPost]
        public IActionResult UpdateDoneStatus(string appointmentId)
        {
            patient_Layer.UpdateAppointmentDoneStatus(appointmentId);
            return Ok();
        }
    }
}
