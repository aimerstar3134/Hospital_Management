using Hospital_Management.Areas.Patient.Models;
using Hospital_Management.Areas.Staff.Data;
using Hospital_Management.Areas.User.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital_Management.Areas.Staff.Controllers
{
    public class StaffController : Controller
    {
        string cn = "Data Source=_aimerstar_\\HARSHILMSSQL;Initial Catalog=HospitalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        Staff_layer staff_Layer = new Staff_layer();
        public IActionResult StaffDashboard()
        {
            return View(); 
        }

        public IActionResult AppointmentList()
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("Select * from Appoinment_tbl", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AppoinmnetModel appointment = new AppoinmnetModel();
                    appointment.Appoinment_Id = Convert.ToInt32(reader["Appoinment_Id"]);
                    appointment.User_Id = reader["User_ID"].ToString();
                    appointment.Patient_Name = reader["Patient_Name"].ToString();
                    appointment.Doctor_Name = reader["Doctor_Name"].ToString();
                    appointment.Date = reader["Date"].ToString();
                    appointment.Timeperiod = reader["Timeperiod"].ToString();
                    appointment.STime = (TimeSpan)reader["STime"];
                    appointment.ETime = (TimeSpan)reader["ETime"];

                    list.Add(appointment);
                }
            }

            return View(list);
        } 
        [HttpGet]
        public IActionResult EditAppointment(int id)
        {
            var appointment = staff_Layer.GetAppointmentById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        [HttpPost]
        public IActionResult EditAppointment(AppoinmnetModel appointment)
        {
            staff_Layer.EditAppointment(appointment);
            return RedirectToAction("AppointmentList");
        }

        public IActionResult AppointmentDetails(string id)
        {
            var  appointment = staff_Layer.GetAppointmentById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        public IActionResult DeleteAppointment(int id)
        {
            staff_Layer.DeleteAppointment(id);
            return RedirectToAction("AppointmentList"); // Placeholder for demonstration
        }

        public IActionResult ManagePatientRecords()
        {
            var patients = staff_Layer.GetAllPatients();
            return View(patients);
        }
        [HttpGet]
        public IActionResult AddTestReport()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTestReport(TestReports test)
        {
            staff_Layer.AddTestReports(test);
            return RedirectToAction();
        }
        [HttpGet]
        public IActionResult ViewTestReports()
        {
            return View(staff_Layer.ViewTestReports());
        }
    }
}
