using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoopStackWebsite.Models.Level;
using HoopStackWebsite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoopStackWebsite.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LevelsController : ControllerBase //controller to level service to add levels to the database
    {
        public JsonLevelService levelService { get; }
        public LevelsController(JsonLevelService levelService)
        {
            this.levelService = levelService;
        }

        [HttpGet]
        public IEnumerable<Level> Get()
        {
            return levelService.GetLevels();
        }

        // ?????????
        [HttpPatch]
        public ActionResult Patch(Level level)
        {
            levelService.AddLevel(level);
            return Ok();
        }
    }
}
