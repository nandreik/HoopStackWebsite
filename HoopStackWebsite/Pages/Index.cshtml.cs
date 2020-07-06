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

namespace HoopStackWebsite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public HoopStackSolver _solver;
        // put a input form here?
        [BindProperty] //binds input from level entry to the LevelEntryModel Level
        public LevelEntryModel Level { get; set; }

        public IndexModel(ILogger<IndexModel> logger, HoopStackSolver solver)
        {
            _logger = logger;
            _solver = solver;
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
            Level level = new Level(Level);

            // temp else redirect to index
            return RedirectToPage("/Index");
        }
    }
}
