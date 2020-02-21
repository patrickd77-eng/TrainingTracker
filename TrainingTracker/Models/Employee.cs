using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingTracker.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Status")]
        public string Status { get; set; }

        public List<Progress> Progress { get; set; }
           
            
           
    }

    public enum Status
    {   
        [Display(Name ="New Starter")]
        Starter,

        [Display(Name = "Ongoing Training")]
        Ongoing_Training,

        [Display(Name = "Training Finished")]
        Trained_Refresher_Only
    }
}
