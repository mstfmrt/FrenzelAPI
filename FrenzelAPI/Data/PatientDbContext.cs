using FrenzelAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FrenzelAPI.Data
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options)
            : base(options)
        {

        }

        public DbSet<Patient> Patients { get; set; }
    }
}
