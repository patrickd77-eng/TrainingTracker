using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Models
{
    public class Progress
    {
        [Key]
        
        public int ProgressID { get; set; }     
        public int Completed { get; set; }

        public Training Training { get; set; }

        public Employee Employee { get; set; }
       


    }


}