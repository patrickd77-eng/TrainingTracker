/*=============================================================================
 |   Author and Copyright: Patrick Davis, s4901703
 |
 |   Designed in: 2019-2020 for Screwfix Poole Parkstone
 |
 |   As part of: Bournemouth University, Business Information Technology Final Year Project 
 |
 |   This code: Creates a training item model with a 1:M progress record relationship and necessary attributes/display names.
 |              
 *===========================================================================*/

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [DisplayName("Module/Description")]
        public string ModuleName { get; set; }
        public ICollection<Progress> Progresses { get; set; }

    }
}
