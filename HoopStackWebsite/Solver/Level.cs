using HoopStackWebsite.Models;
using HoopStackWebsite.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HoopStackWebsite.Pages.Solver.Level
{
    public class Level
    {
        //public LevelEntryModel LevelModel { get; set; } //get the level info from levelentrymodel

        [JsonPropertyName("levelNum")]
        public int LevelNum { get; set; } //level number
        [JsonPropertyName("stacks")]
        public List<List<string>> Stacks { get; set; } //stacks of the level
        [JsonPropertyName("instructions")]
        public List<string> Instructions { get; set; } //instructions for the solution
        [JsonPropertyName("error")]
        public string Error { get; set; } //possible variable for reporting bugs in the solver?

        public Moves Solutions { get; set; }
        public List<Move> WrongMoves { get; set; }

        public Level(LevelEntryModel LevelModel)
        {
            //create level from levelentrymodel
            this.LevelNum = LevelModel.LevelNum; //get level number
            this.Stacks = new List<List<string>>();
            this.Instructions = new List<string>();
            this.Solutions = new Moves();
            this.WrongMoves = new List<Move>();
            int numStacks = LevelModel.NumStacks; //get the colors for numStacks stacks
            for (int i = 0; i < numStacks; i++)
            {
                string stack = i switch
                {
                    0 => LevelModel.Stack1,
                    1 => LevelModel.Stack2,
                    2 => LevelModel.Stack3,
                    3 => LevelModel.Stack4,
                    4 => LevelModel.Stack5,
                    5 => LevelModel.Stack6,
                    6 => LevelModel.Stack7,
                    7 => LevelModel.Stack8,
                    8 => LevelModel.Stack9,
                    9 => LevelModel.Stack10,
                    _ => "Could not retrieve stack.",
                };
                string[] words = stack.Split(",");
                List<string> temp = new List<string>();
                foreach (var word in words)
                    temp.Add(word);
                this.Stacks.Add(temp);
            }
            //solve the stacks in this constructor or do it another way?
            HoopStackSolver.solveLevel(this);
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize<Level>(this);
        }
    }
}
