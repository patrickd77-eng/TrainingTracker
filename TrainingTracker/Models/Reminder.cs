using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Models
{
    public class Reminder
    {
        [Key]
        public int ReminderId { get; set; }
        [Required]
        [DisplayName("Reminder")]
        [MaxLength(300)]
        public string ReminderContent { get; set; }

        [Display(Name = "Date Due")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime? DateDue { get; set; }
    }
}
