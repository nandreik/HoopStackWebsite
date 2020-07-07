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

        public IndexModel(ILogger<IndexModel> logger, JsonLevelService levelService)
        {
            _logger = logger;
            this.levelService = levelService;
            this.levelsController = new LevelsController(this.levelService);
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            //need to add validation b/c rn it fails if not all fields are filled
            /*            if (ModelState.IsValid == false) //if validation fails, return page
                        {
                            return Page();
                        }*/
            // save model to db
            Level level = new Level(LevelModel);
            levelsController.Patch(level);

            // temp else redirect to index
            return RedirectToPage("/Index");
        }
    }
}
