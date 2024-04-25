using System;

namespace Hospital_Management.Areas.Patient.Models
{
    public class TestReports
    {
        public int TestReportId { get; set; }
        public int UserId { get; set; }
        public string DoctorName { get; set; }
        public string TestCategory { get; set; }
        public string TestDate { get; set; }
        public string TestResult { get; set; }
    }
}
