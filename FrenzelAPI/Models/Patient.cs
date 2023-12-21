namespace FrenzelAPI.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
