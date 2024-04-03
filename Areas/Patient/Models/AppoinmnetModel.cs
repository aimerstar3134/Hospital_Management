namespace Hospital_Management.Areas.Patient.Models
{
    public class AppoinmnetModel
    {
        public int Appoinment_Id { get; set; }

        public string User_Id { get; set; }

        public string Patient_Name { get; set; }
        public string Doctor_Name { get; set; }

        public string Date { get; set; }  

        public string Timeperiod { get; set; }
        
        public TimeSpan STime { get; set; }

        public TimeSpan ETime { get; set; }
        public bool IsDone { get; set; }
    }
}
