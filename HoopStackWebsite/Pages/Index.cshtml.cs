using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoopStackWebsite.Models;
using HoopStackWebsite.Solver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HoopStackWebsite.Pages.Solver.Level;
using HoopStackWebsite.Controllers;
using HoopStackWebsite.Services;

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

        public IndexModel(ILogger<IndexModel> logger, JsonLevelService levelService)
        {
            _logger = logger;
            this.levelService = levelService;
            this.levelsController = new LevelsController(this.levelService);
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostInput()
        {
            //added validation
            if (ModelState.IsValid == false) //if validation fails, return page
            {
                // save model to db
                Level level = new Level(LevelModel);
                levelsController.Patch(level);
                //show level solution somehow



                return Page();
            }
            // save model to db
            //Level level = new Level(LevelModel);
            //levelsController.Patch(level);

            // temp else redirect to index
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostSearch()
        {
            if (searchLevel.HasValue) //if validation fails, return page
            {
                // look for level with level num
                var levels = levelsController.levelService.GetLevels(); //get all levels
                List<Level> matching = new List<Level>(); //any levels that match searched level
                foreach (Level level in levels) //check to see if any levels match
                {
                    if (level.LevelNum == searchLevel)
                    {
                        matching.Add(level);
                    }
                }
                if (matching.Count == 0) //if no matching levels found
                {
                    // display not found somehow


                }
                else
                {
                    // display found levels somehow 



                }
            }

            // temp else redirect to index
            return RedirectToPage("/Index");
        }
    }
}
