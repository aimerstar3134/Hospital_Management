using Hospital_Management.Areas.Doctor.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Areas.Doctor.Data
{
    public class DoctorDbContext :DbContext
    {
        public DoctorDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<available> availables { get; set; }
    }
}
