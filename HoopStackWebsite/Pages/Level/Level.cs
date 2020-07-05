using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HoopStackWebsite.Pages.Level
{
    public class Level
    {
        [JsonPropertyName("level")]
        public int level { get; set; } //level number
        [JsonPropertyName("stacks")]
        public IEnumerable<IEnumerable<string>> stacks { get; set; } //stacks of the level
        [JsonPropertyName("instructions")]
        public IEnumerable<string> instructions { get; set; } //instructions for the solution
        [JsonPropertyName("error")]
        public string error { get; set; } //possible variable for reporting bugs in the solver?

        public override string ToString()
        {
            return JsonSerializer.Serialize<Level>(this);
        }
    }
}
