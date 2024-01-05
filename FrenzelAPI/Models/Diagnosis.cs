using System.ComponentModel.DataAnnotations;

namespace FrenzelAPI.Models
{
    public class Diagnosis
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public NystagmusType? NystagmusType { get; set; }
        public string NystagmusSeverity { get; set; }
        public string RecordPath { get; set; }
    }

    public enum NystagmusType
    {
        Horizontal,
        Vertical,
        Rotary
    }
}
