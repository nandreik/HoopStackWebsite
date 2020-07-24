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
//using DataAccessLibrary;
//using DataAccessLibrary.Models;
//noDB branch works on website. need to make a new publish profile to avoid auth error

namespace HoopStackWebsite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        // put a input form here
        [BindProperty] //binds input from level entry to the LevelEntryModel Level
        public LevelEntryModel LevelModel { get; set; } //model for getting user input 
        public JsonLevelService levelService { get; set; } //service for writing/getting data from json file
        public LevelsController levelsController { get; set; } //idk if this is how to use the levels controller api
        [BindProperty(SupportsGet = true)] //bind input from search box 
        public int? searchLevel { get; set; } //input for search level
        public List<Level> displayLevels { get; set; } //list of levels to display on page
        public bool errorSearch { get; set; } //if not found 
        public bool errorSolve { get; set; } //if not solved

        //public LevelData Database { get; set; } //db

        /*public IndexModel(ILogger<IndexModel> logger, JsonLevelService levelService, LevelData levelData)
        {
            _logger = logger;
            this.levelService = levelService;
            this.levelsController = new LevelsController(this.levelService);
            this.errorSearch = false;
            this.errorSolve = false;
            this.displayLevels = new List<Level>();
            this.Database = levelData;
        }*/

        public IndexModel(ILogger<IndexModel> logger, JsonLevelService levelService)
        {
            _logger = logger;
            this.levelService = levelService;
            this.levelsController = new LevelsController(this.levelService);
            this.errorSearch = false;
            this.errorSolve = false;
            this.displayLevels = new List<Level>();
        }

        public void OnGet()
        {
            //levels = levelService.GetLevels();
        }

        public IActionResult OnPostInput()
        {
            //added validation
            if (ModelState.IsValid) //if validation fails, return page
            {
                var task = Task.Run(() => new Level(LevelModel)); //give solver 15 seconds to try to solve level, if not solved, show error
                if (task.Wait(TimeSpan.FromSeconds(10)))
                {
                    if (!LevelExistsJson(task.Result)) //check json file 
                    {
                        levelsController.Patch(task.Result);
                    }
                    /*if (LevelExistsDb(task.Result).Result == null) //check db
                    {
                        InsertDb(task.Result);
                    }*/
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
                List<Level> matching = new List<Level>(); //any levels that match searched level

                // look for level using jsonfile
                var levels = levelsController.levelService.GetLevels(); //get all levels
                foreach (var level in levels) //check to see if any levels match 
                {
                    if (level.LevelNum == searchLevel)
                    {
                        matching.Add(level);
                    }
                }

                // look for level using db
                /*var levelsDb = await Database.GetLevels();
                foreach (var levelModel in levelsDb) //check to see if any levels match 
                {
                    if (levelModel.LevelNum == searchLevel)
                    {
                        Level searchedLevel = new Level(levelModel);
                        matching.Add(searchedLevel);
                    }
                }*/
                if (matching.Count == 0) //if no matching levels found
                {
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

        public bool LevelExistsJson(Level newLevel) //checks json file for levels
        {
            var levels = levelService.GetLevels(); 
            foreach (Level level in levels)
            {
                if (newLevel.ToString() == level.ToString())
                    return true;
            }
            return false;
        }

        /*public async Task<LevelModel> LevelExistsDb(Level newLevel) //checks database if newLevel is already in it (if not null; true, if null; false)
        {
            LevelModel newLevelModel = toLevelModel(newLevel); //make LevelModel from newLevel in a horrible way
            var levels = await Database.GetLevels();
            foreach (var level in levels) //check each level in db
            {
                if (level.ToString() == newLevelModel.ToString())
                    return level;
            }
            return null;
        }*/

        /*public void InsertDb(Level newLevel) //method for inserting a level to db
        {
            LevelModel newLevelModel = toLevelModel(newLevel); //make LevelModel from newLevel for db entry in a horrible way
            Database.InsertLevel(newLevelModel);
        }*/

        /*public LevelModel toLevelModel(Level newLevel) //method to convert LevelModel to Level
        {
            LevelModel newLevelModel = new LevelModel(); //make LevelModel from newLevel
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

            return newLevelModel;
        }*/
    }
}
