using FrenzelAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FrenzelAPI.Data
{
    public class DiagnosisDbContext : DbContext
    {
        public DiagnosisDbContext(DbContextOptions<DiagnosisDbContext> options)
            :base(options)
        {

        }

        public DbSet<Diagnosis> Diagnosis { get; set; }
    }
}
