/*=============================================================================
 |   Author and Copyright: Patrick Davis, s4901703
 |
 |   Designed in: 2019-2020 for Screwfix Poole Parkstone
 |
 |   As part of: Bournemouth University, Business Information Technology Final Year Project 
 |
 |   This code: Creates a progress model that acts as a link table between Employee and Training.
 |              
 *===========================================================================*/

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
