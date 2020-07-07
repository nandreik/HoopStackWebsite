using HoopStackWebsite.Pages.Solver.Level;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HoopStackWebsite.Services
{
    public class JsonLevelService //service to be able to fetch and add levels to db 
    {
        public JsonLevelService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "levels.json"); }
        }

        public IEnumerable<Level> GetLevels()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<Level[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public void AddLevel(Level newLevel)
        {
            IEnumerable<Level> levels = GetLevels();

            bool sameLevel = false;
            foreach (var level in levels)
            {
                if (level.LevelNum == newLevel.LevelNum) //if there is a level with the same levelNumber, check if their stacks are the same
                {
                    //sameLevel = (level.Stacks).SequenceEqual(newLevel.Stacks); //check if both levels stacks are equal (doesnt check if colors are spelled differently though)
                    /*
                     * SequenceEqual doesnt check stacks correctly, for now just add every level to the json file regardless
                     */

                    if (sameLevel) //stop looking if same level is found
                        break;
                }
            }
            //if not, add new level with same level num but dif stacks 
            if (!sameLevel)
            {
                levels = levels.Concat(new[] { newLevel }); //"add" the new level by concating it with the old levels 

                //update json file with new level
                writeJsonData(levels);
            }
        }

        public void writeJsonData(IEnumerable<Level> levels) //write level data to the json file of all levels 
        {
            using (var outputStream = File.OpenWrite(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<Level>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    levels
                );
            }
        }
    }
}
