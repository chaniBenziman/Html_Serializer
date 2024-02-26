using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public Selector()
        {
            Classes = new List<string>();
        }
        private static Selector CastSelector(String query)
        {
            string[] dividers = query.Split(' ');
            Selector current = new Selector();
            string[] validHtmlTags = HtmlHelper.Instance.TagsWithoutClosing.Concat(HtmlHelper.Instance.Tags).ToArray();
            foreach (string divider in dividers)
            {
                string[] partsQuery = divider.Split(new[] { '#', '.' }, StringSplitOptions.RemoveEmptyEntries);

                // Update current selector properties based on partsQuery
                foreach (string part in partsQuery)
                {
                    if (part.StartsWith("#"))
                    {
                        current.Id = part.Substring(1); // Remove '#' from id
                    }
                    else if (part.StartsWith("."))
                    {
                        current.Classes.Add(part.Substring(1)); // Remove '.' from class and add to classes list
                    }
                    else
                    {
                        // Check if it's a valid HTML tag name
                        if (IsValidTagName(part, validHtmlTags))
                        {
                            current.TagName = part;
                        }
                        // If not valid, treat it as a part of classes
                        else
                        {
                            current.Classes.Add(part);
                        }
                    }
                    
                }
                Selector newSelector = new Selector();
                current.Child = newSelector;
                newSelector.Parent = current;
                current = newSelector;
            }
            return current;

        }
        private static bool IsValidTagName(string tagName, string[] validHtmlTags)
        {
            return validHtmlTags.Contains(tagName, StringComparer.OrdinalIgnoreCase);
        }
    }
}
