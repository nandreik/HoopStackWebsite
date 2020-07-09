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
           /* var searchLevels = levelService.GetLevels();
            if (searchLevel.HasValue) //if searchlevel has a value search for levels in db with same level num
            {
                searchLevels = searchLevels.Where(s => s.LevelNum == searchLevel);
            }
            //IList<Level> iListLevels = searchLevels.ToList()
            levels = searchLevels;
*/
        }

        public IActionResult OnPost()
        {
            //need to add validation b/c rn it fails if not all fields are filled
            if (ModelState.IsValid == false) //if validation fails, return page
            {
                return Page();
            }
            // save model to db
            Level level = new Level(LevelModel);
            levelsController.Patch(level);

            // temp else redirect to index
            return RedirectToPage("/Index");
        }
    }
}
