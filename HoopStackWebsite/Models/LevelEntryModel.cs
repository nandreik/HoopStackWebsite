using HoopStackWebsite.Pages.Solver.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoopStackWebsite.Models
{
    public class LevelEntryModel //model class to hold information about a level from the input form
    {
        public int LevelNum { get; set; }
        public int NumStacks{ get; set; }
        public string Stack1{ get; set; }
        public string Stack2{ get; set; }
        public string Stack3{ get; set; }
        public string Stack4{ get; set; } 
        public string Stack5{ get; set; }
        public string Stack6{ get; set; }
        public string Stack7{ get; set; }
        public string Stack8{ get; set; }
        public string Stack9{ get; set; }
        public string Stack10{ get; set; }
    }
}
