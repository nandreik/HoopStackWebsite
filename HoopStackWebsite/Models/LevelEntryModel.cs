using HoopStackWebsite.Pages.Solver.Level;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HoopStackWebsite.Models
{
    public class LevelEntryModel //model class to hold information about a level from the input form
    {
        [Required(ErrorMessage = "Level Number is required.")]
        public int LevelNum { get; set; }
        [Range(4, 10, ErrorMessage = "Must have between 4 and 10 stacks.")]
        public int NumStacks{ get; set; }
        [Required(ErrorMessage = "Colors are required.")]
        public string Stack1{ get; set; }
        [Required(ErrorMessage = "Colors are required.")]
        public string Stack2{ get; set; }
        [Required(ErrorMessage = "Colors are required.")]
        public string Stack3{ get; set; }
        [Required(ErrorMessage = "Colors are required.")]
        public string Stack4{ get; set; } 
        public string Stack5{ get; set; }
        public string Stack6{ get; set; }
        public string Stack7{ get; set; }
        public string Stack8{ get; set; }
        public string Stack9{ get; set; }
        public string Stack10{ get; set; }
    }
}
