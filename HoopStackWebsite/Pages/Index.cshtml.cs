using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoopStackWebsite.Models;
using HoopStackWebsite.Solver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HoopStackWebsite.Models.Level;
using HoopStackWebsite.Controllers;
using HoopStackWebsite.Services;
using DataAccessLibrary;
using DataAccessLibrary.Models;

namespace HoopStackWebsite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        // put a input form here
        [BindProperty] //binds input from level entry to the LevelEntryModel Level
        public LevelEntryModel LevelModel { get; set; }
        public JsonLevelService levelService { get; set; }
        public LevelsController levelsController { get; set; } //idk if this is how to use the levels controller api
        [BindProperty(SupportsGet = true)] //bind input from search box 
        public int? searchLevel { get; set; }
        public IEnumerable<Level> levels { get; set; }

        public List<Level> displayLevels { get; set; }
        public bool errorSearch { get; set; } //if not found 
        public bool errorSolve { get; set; } //if not solved

        public LevelData Database { get; set; }

        public IndexModel(ILogger<IndexModel> logger, JsonLevelService levelService, LevelData levelData)
        {
            _logger = logger;
            this.levelService = levelService;
            this.levelsController = new LevelsController(this.levelService);
            this.errorSearch = false;
            this.errorSolve = false;
            this.displayLevels = new List<Level>();
            this.Database = levelData;
        }

        public void OnGet()
        {
            //levels = levelService.GetLevels();
        }

        public IActionResult OnPostInput()
        {
            //added validation
            if (ModelState.IsValid == true) //if validation fails, return page
            {
                var task = Task.Run(() => new Level(LevelModel)); //give solver 15 seconds to try to solve level, if not solved, show error
                if (task.Wait(TimeSpan.FromSeconds(15)))
                {
                    if (!LevelExists(task.Result))
                    {
                        levelsController.Patch(task.Result);
                        InsertDb(task.Result);
                    }
                    displayLevels.Add(task.Result);
                }
                else
                    errorSolve = true;

                return Page();
            }

            // temp else redirect to index
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostSearch()
        {
            if (searchLevel.HasValue) //if validation fails, return page
            {
                // look for level using jsonfile
                var levels = levelsController.levelService.GetLevels(); //get all levels
                List<Level> matching = new List<Level>(); //any levels that match searched level
                // look for level using db

                foreach (var level in levels) //check to see if any levels match
                {
                    if (level.LevelNum == searchLevel)
                    {
                        matching.Add(level);
                    }
                }
                if (matching.Count == 0) //if no matching levels found
                {
                    // display not found somehow
                    // gross way to display a not found level
                    errorSearch = true;

                    return Page();
                }
                else
                {
                    // display found levels 
                    displayLevels = matching;
                    return Page();
                }
            }
            // else redirect to home
            return RedirectToPage("/Index");
        }

        public bool LevelExists(Level newLevel) //checks json file for levels
        {
            var levels = levelService.GetLevels(); 
            foreach (Level level in levels)
            {
                if (newLevel.ToString() == level.ToString())
                    return true;
            }
            return false;
        }

        /*public bool LevelExistsDB(Level newLevel) //checks database for levels
        {
            LevelModel newLevelModel = new LevelModel
            {
                LevelNum = newLevel.LevelNum,
                NumStacks = newLevel.Stacks.Count,
                Stack1 = newLevel.Stacks[0].ToString(),
                Stack2 = newLevel.Stacks[1].ToString(),
                Stack3 = newLevel.Stacks[2].ToString(),
                Stack4 = newLevel.Stacks[3].ToString(),
                Stack5 = newLevel.Stacks[4].ToString(),
                Stack6 = newLevel.Stacks[5].ToString(),
                Stack7 = newLevel.Stacks[6].ToString(),
                Stack8 = newLevel.Stacks[7].ToString(),
                Stack9 = newLevel.Stacks[8].ToString(),
                Stack10 = newLevel.Stacks[9].ToString()
            }; //make LevelModel from newLevel for db entry

            var levels = Database.GetLevels();
            foreach (var level in levels)
            {
                if (newLevel.ToString() == level.ToString())
                    return true;
            }
            return false;
        }*/

        public void InsertDb(Level newLevel) //method for inserting a level to db
        {
            LevelModel newLevelModel = new LevelModel();
            newLevelModel.LevelNum = newLevel.LevelNum;
            newLevelModel.NumStacks = newLevel.Stacks.Count;

            if (newLevel.Stacks.Count >= 1)
                newLevelModel.Stack1 = string.Join(",", newLevel.Stacks[0]);
            if (newLevel.Stacks.Count >= 2)
                newLevelModel.Stack2 = string.Join(",", newLevel.Stacks[1]);
            if (newLevel.Stacks.Count >= 3)
                newLevelModel.Stack3 = string.Join(",", newLevel.Stacks[2]);
            if (newLevel.Stacks.Count >= 4)
                newLevelModel.Stack4 = string.Join(",", newLevel.Stacks[3]);
            if (newLevel.Stacks.Count >= 5)
                newLevelModel.Stack5 = string.Join(",", newLevel.Stacks[4]);
            if (newLevel.Stacks.Count >= 6)
                newLevelModel.Stack6 = string.Join(",", newLevel.Stacks[5]);
            if (newLevel.Stacks.Count >= 7)
                newLevelModel.Stack7 = string.Join(",", newLevel.Stacks[6]);
            if (newLevel.Stacks.Count >= 8)
                newLevelModel.Stack8 = string.Join(",", newLevel.Stacks[7]);
            if (newLevel.Stacks.Count >= 9)
                newLevelModel.Stack9 = string.Join(",", newLevel.Stacks[8]);
            if (newLevel.Stacks.Count >= 10)
                newLevelModel.Stack10 = string.Join(",", newLevel.Stacks[9]);
            //make LevelModel from newLevel for db entry in a horrible way

            Database.InsertLevel(newLevelModel);
        }
    }
}
