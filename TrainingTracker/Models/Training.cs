using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingTracker.Models
{
    public class Training
    {
        [Key]
        public int TrainingID { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string ModuleName { get; set; }

        
    }
}
