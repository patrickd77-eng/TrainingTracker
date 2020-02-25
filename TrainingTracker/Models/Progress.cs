using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Models
{
    public class Progress
    {
        [Key]
        public int ProgressId { get; set; }
        public int EmployeeId { get; set; }
        public int TrainingId { get; set; }
        public bool Completed { get; set; }

        public Employee Employee { get; set; }
        public Training Training { get; set; }

    }
}
