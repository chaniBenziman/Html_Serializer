using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue=new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count>0)
            {
                var current=queue.Dequeue();
                yield return current;
                foreach(var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }  
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this;
            while(current.Parent != null) 
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }
        public void FindElements(List<HtmlElement> list, HtmlElement htmlElement, Selector selector)
        {
            if (MatchesSelector(htmlElement, selector))
            {
                list.Add(htmlElement);
            }
            // Recursively search through children elements
            foreach (var child in htmlElement.Descendants())
            {
                // Apply selector on each descendant
                if (MatchesSelector(child, selector))
                {
                    list.Add(child);
                }
            }

        }
        private bool MatchesSelector(HtmlElement htmlElement, Selector selector)
        {
            // Check if the tag name matches, if specified
            if (!string.IsNullOrEmpty(selector.TagName) && htmlElement.Name != selector.TagName)
            {
                return false;
            }

            // Check if the id matches, if specified
            if (!string.IsNullOrEmpty(selector.Id) && htmlElement.Id != selector.Id)
            {
                return false;
            }
            // If all conditions are met, return true
            return true;
        }
    }
}
