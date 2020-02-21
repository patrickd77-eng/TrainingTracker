using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingTracker.Models
{
    public class Training
    {
        [Key]
        public int TrainingId { get; set; }
        [Required]
        [DisplayName("Category")]
        public string CategoryName { get; set; }
        [Required]
        [DisplayName("Module")]
        public string ModuleName { get; set; }
        public ICollection<Progress> Progresses { get; set; }

    }
}
