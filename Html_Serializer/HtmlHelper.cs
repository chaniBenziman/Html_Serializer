using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] Tags { get; set; }
        public string[] TagsWithoutClosing { get; set; }
        private HtmlHelper()
        {
            Tags = LoadTagsFromFile("Json/HtmlTags.json");
            TagsWithoutClosing = LoadTagsFromFile("Json/HtmlVoidTags.json");
        }
        private string[] LoadTagsFromFile(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<string[]>(jsonString);
        }
    }
}
