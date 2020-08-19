using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class LevelModel
    {
        public int LevelNum { get; set; }
        public int NumStacks { get; set; }

        public string Stack1 { get; set; }
        public string Stack2 { get; set; }
        public string Stack3 { get; set; }
        public string Stack4 { get; set; }
        public string Stack5 { get; set; }
        public string Stack6 { get; set; }
        public string Stack7 { get; set; }
        public string Stack8 { get; set; }
        public string Stack9 { get; set; }
        public string Stack10 { get; set; }

        public override string ToString() //tostring method made to make comparison between dif levelmodels easier
        {
            string str = this.LevelNum.ToString() + this.NumStacks.ToString() + Stack1 + Stack2 + Stack3 + Stack4 + Stack5 + Stack6 + Stack7 + Stack8 + Stack9 + Stack10;
            return str;
        }
    }
}
