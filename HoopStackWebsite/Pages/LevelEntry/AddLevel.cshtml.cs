using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoopStackWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoopStackWebsite.Pages.LevelEntry
{
    public class AddLevelModel : PageModel
    {
        [BindProperty] //binds input from level entry to the LevelEntryModel Level
        public LevelEntryModel Level { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            //need to add validation b/c rn it fails if not all fields are filled
            if (ModelState.IsValid == false) //if validation fails, return page
            {
                return Page();
            }
            // save model to db


            // temp else redirect to index
            return RedirectToPage("/Index");
        }
    }
}