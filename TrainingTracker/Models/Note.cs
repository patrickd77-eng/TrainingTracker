/*=============================================================================
 |   Author and Copyright: Patrick Davis, s4901703
 |
 |   Designed in: 2019-2020 for Screwfix Poole Parkstone
 |
 |   As part of: Bournemouth University, Business Information Technology Final Year Project 
 |
 |   This code: Creates a note model with required attributes and custom display names. 
 |   Also establishes M:1 relationship with employes.
 |              
 *===========================================================================*/

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
