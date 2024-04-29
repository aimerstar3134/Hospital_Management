using Hospital_Management.Areas.Admin.Models;
using Hospital_Management.Areas.Doctor.Data;
using Hospital_Management.Areas.Patient.Data;
using Hospital_Management.Areas.Staff.Data;
using Hospital_Management.Areas.User.Data;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly user_layer _userService;
        private readonly layer _doctorService;
        private readonly Patient_Layer _patientService;
        private readonly Staff_layer _staffService;

        //public AdminController(user_layer userService, layer doctorService, Patient_Layer patientService, Staff_layer staffService)
        //{
        //    _userService = userService;
        //    _doctorService = doctorService;
        //    _patientService = patientService;
        //    _staffService = staffService;
        //}

        public IActionResult AdminDashboard()
        {
            //var model = new AdminDashboardViewModel
            //{
            //    //TotalUsers = _userService.GetTotalUsers(),
            //    //TotalDoctors = _doctorService.GetTotalDoctors(),
            //    //TotalPatients = _patientService.GetTotalPatients(),
            //    //TotalStaff = _staffService.GetTotalStaff()
            //    // Add more properties as needed
            //};
            return View();
        }
    }

}
