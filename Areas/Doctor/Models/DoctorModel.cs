namespace Hospital_Management.Areas.Doctor.Models
{
    public class DoctorModel
    {
        public int User_Id { get; set; }
        public string Name { get; set; }
        
        public string Designation { get; set; }
        public TimeSpan AvailableStartTime { get; set; }
        public TimeSpan AvailableEndTime { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }

    }
}
