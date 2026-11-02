using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace part2
{
    internal class HtmlHelper
    {

        private readonly static HtmlHelper _instance = new HtmlHelper();

        public static HtmlHelper Instance => _instance;
        public string[] HtmlTags { get; set; }

        public string[] HtmlVoidTags { get; set; }

        private HtmlHelper()
        {
            var tagsJson = File.ReadAllText("HtmlTags.json");
            var voidTagsJson = File.ReadAllText("HtmlVoidTags.json");

            HtmlTags = JsonSerializer.Deserialize<string[]>(tagsJson);
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(voidTagsJson);
        }
    }
}
