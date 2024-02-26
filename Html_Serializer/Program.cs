
using Html_Serializer;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

HtmlElement BuildTree(List<string> htmlLines)
{
    HtmlElement root = new();
    var current = root;
    foreach (var line in htmlLines)
    {
        var first=line.Split(' ')[0];
        if (first == "/html")
        {
            break;
        }
        else if(first.StartsWith("/"))
        {
           if(current.Parent != null)
           {
                current = current.Parent;
           }
        }
        else if(HtmlHelper.Instance.Tags.Contains(first))
        {
            HtmlElement htmlElemet = new HtmlElement();
            htmlElemet.Name = first;

            // Handle attributes
            var restOfString = line.Remove(0, first.Length);
            var attributes = Regex.Matches(restOfString, "([a-zA-Z]+)=\\\"([^\\\"]*)\\\"")
                .Cast<Match>()
                .Select(m => $"{m.Groups[1].Value}=\"{m.Groups[2].Value}\"")
                .ToList();

            //case of class attribute
            if (attributes.Any(attribute=>attribute.StartsWith("class")))
            {
                var classAttr = attributes.First(attr => attr.StartsWith("class"));
                var classes = classAttr.Split('=')[1].Trim('"').Split(' ');
                htmlElemet.Classes.AddRange(classes);
            }

            //id
            var idAttribute = attributes.FirstOrDefault(attribute => attribute.StartsWith("id="));
            if (!string.IsNullOrEmpty(idAttribute))
            {
                htmlElemet.Id = idAttribute.Split('=')[1].Trim('"');
            }

            htmlElemet.Attributes.AddRange(attributes);
            current.Children.Add(htmlElemet);
            htmlElemet.Parent = current;

            if (line.EndsWith("/")||HtmlHelper.Instance.TagsWithoutClosing.Contains(first))
            {
                current = htmlElemet.Parent;
            }
            else
            {
                current = htmlElemet;
            }
        }
        else
        {
            current.InnerHtml = line;
        }
    }
    return root;
}