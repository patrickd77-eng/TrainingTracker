/*=============================================================================
 |   Author and Copyright: Patrick Davis, s4901703
 |
 |   Designed in: 2019-2020 for Screwfix Poole Parkstone
 |
 |   As part of: Bournemouth University, Business Information Technology Final Year Project 
 |
 |   This code: Creates an employee model with required attributes and custom display names. 
 |   Also establishes 1:M relationships with progress records and notes.
 |              
 *===========================================================================*/

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public ICollection<Progress> Progresses { get; set; }
        public ICollection<Note> Notes { get; set; }

    }
}
