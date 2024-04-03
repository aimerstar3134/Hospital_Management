using Hospital_Management.Areas.Doctor.Models;
using Hospital_Management.Areas.Patient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Areas.Patient.Data
{
    public class PatientDbContext :DbContext
    {
        public PatientDbContext(DbContextOptions options) : base(options) 
        {
            
        }
        public DbSet<available> availables { get; set; }
        public DbSet<AppoinmnetModel> appoinmnets { get; set;}
    }
}
