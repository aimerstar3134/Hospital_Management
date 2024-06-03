using Hospital_Management.Areas.User.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Hospital_Management.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly user_layer _user_Layer;

        public AdminController(user_layer user_Layer)
        {
            _user_Layer = user_Layer;
        }

        public IActionResult AdminDashboard()
        {
            int totalUsers = _user_Layer.GetUserCount();
            int totalDoctors = _user_Layer.GetDoctorCount();
            int totalPatients = _user_Layer.GetPatientCount();
            int totalStaff = _user_Layer.GetStaffCount();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalDoctors = totalDoctors;
            ViewBag.TotalPatients = totalPatients;
            ViewBag.TotalStaff = totalStaff;

            return View();
        }

        public IActionResult FacilityList()
        {
            var facilities = new List<FacilityViewModel>
            {
                new FacilityViewModel { Id = 1, Name = "Emergency Room", Location = "Building A", Status = "Operational" },
                new FacilityViewModel { Id = 2, Name = "ICU", Location = "Building B", Status = "Under Maintenance" },
                new FacilityViewModel { Id = 3, Name = "Radiology Department", Location = "Building C", Status = "Operational" },
                new FacilityViewModel { Id = 4, Name = "Pharmacy", Location = "Building D", Status = "Operational" },
                new FacilityViewModel { Id = 5, Name = "Laboratory", Location = "Building E", Status = "Operational" }
            };

            return View(facilities);
        }

        public IActionResult BillingAndFinance()
        {
            var billingRecords = new List<BillingViewModel>
            {
                new BillingViewModel { Id = 1, Description = "Consultation Fee", Amount = 100, Date = "2024-06-01" },
                new BillingViewModel { Id = 2, Description = "X-Ray", Amount = 200, Date = "2024-06-02" },
                // Add more static data here...
            };

            return View(billingRecords);
        }

        public IActionResult ReportingAndAnalytics()
        {
            var reports = new List<ReportViewModel>
            {
                new ReportViewModel { Id = 1, Title = "Monthly Revenue Report", CreatedDate = "2024-05-31", Author = "Admin" },
                new ReportViewModel { Id = 2, Title = "Patient Admission Statistics", CreatedDate = "2024-05-30", Author = "Admin" },
                // Add more static data here...
            };

            return View(reports);
        }

        public IActionResult SecurityAndAccessControl()
        {
            var securityLogs = new List<SecurityLogViewModel>
            {
                new SecurityLogViewModel { Id = 1, Event = "Login Attempt", User = "Admin", Date = "2024-06-01", Status = "Success" },
                new SecurityLogViewModel { Id = 2, Event = "Password Change", User = "Doctor", Date = "2024-06-02", Status = "Success" },
                // Add more static data here...
            };

            return View(securityLogs);
        }

        public IActionResult SystemConfiguration()
        {
            var configurations = new List<SystemConfigViewModel>
            {
                new SystemConfigViewModel { Id = 1, Key = "MaxLoginAttempts", Value = "5", Description = "Maximum number of login attempts before locking the account." },
                new SystemConfigViewModel { Id = 2, Key = "SessionTimeout", Value = "30", Description = "Session timeout duration in minutes." },
                // Add more static data here...
            };

            return View(configurations);
        }
    }

    public class FacilityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
    }

    public class BillingViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string Date { get; set; }
    }

    public class ReportViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreatedDate { get; set; }
        public string Author { get; set; }
    }

    public class SecurityLogViewModel
    {
        public int Id { get; set; }
        public string Event { get; set; }
        public string User { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }

    public class SystemConfigViewModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
