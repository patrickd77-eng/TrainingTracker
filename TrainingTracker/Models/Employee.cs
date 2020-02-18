using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingTracker.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string fullName { get; set; }

        public int overallProgress { get; set; }
    }
}
