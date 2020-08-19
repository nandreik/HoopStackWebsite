using HoopStackWebsite.Models.Level;
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
            //check that level is not in the json file
            bool exists = false; 
            foreach (Level l in levels)
            {
                if (l.ToString() == newLevel.ToString())
                {
                    exists = true;
                }
            }
            if (!exists)
            {
                levels = levels.Concat(new[] { newLevel }); //"add" the new level by concating it with the old levels 
                writeJsonData(levels); //update json file with new level
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

        public bool LevelExistsJson(Level newLevel) //checks json file for levels
        {
            var levels = GetLevels();
            foreach (Level level in levels)
            {
                if (newLevel.ToString() == level.ToString())
                    return true;
            }
            return false;
        }
    }
}
