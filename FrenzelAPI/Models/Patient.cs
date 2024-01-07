using System.ComponentModel.DataAnnotations;

namespace FrenzelAPI.Models
{
    public class Patient
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public long TCKN { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderType Gender { get; set; } 

        public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
    }

    public enum GenderType
    {
        Male,
        Female
    }
}
