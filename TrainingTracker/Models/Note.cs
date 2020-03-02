using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }
        [Required]
        [DisplayName("Employee")]
        public int EmployeeId { get; set; }
        [Required]
        [DisplayName("Note Content")]
        [MaxLength(250)]
        public string NoteContent { get; set; }

        public Employee Employee { get; set; }
    }
}
