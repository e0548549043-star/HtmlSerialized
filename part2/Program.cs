

using part2;
using System.Text.RegularExpressions;

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}


var url = "https://magic-kass.co.il/";
var htmlContent = await Load(url);


var cleanedHtml = new Regex("\\s+").Replace(htmlContent, " ");
var htmlLines = new Regex("<(.*?)>").Split(cleanedHtml).Where(l => l.Length > 0);



HtmlElement BuildTree(IEnumerable<string> htmlLines)
{
    HtmlElement root = new HtmlElement { Name = "root" };
    HtmlElement current = root;
    var attributeRegex = new Regex("([^\\s]*?)=\"(.*?)\"");

    foreach (var line in htmlLines)
    {
        var firstWord = Regex.Match(line, "^\\S+").Value;

       
        if (firstWord == "/html")
            break;

  
        if (firstWord.StartsWith("/"))
        {
            if (current.Parent != null)
                current = current.Parent;
            continue;
        }

        var tagName = firstWord.TrimEnd('/');  

        if (HtmlHelper.Instance.HtmlTags.Contains(tagName) ||
            HtmlHelper.Instance.HtmlVoidTags.Contains(tagName))
        {
            var newElement = new HtmlElement { Name = tagName, Parent = current };

            foreach (Match attribute in attributeRegex.Matches(line))
            {
                var name = attribute.Groups[1].Value;
                var value = attribute.Groups[2].Value;

                if (name == "id")
                    newElement.Id = value;
                else if (name == "class")
                    newElement.Classes.AddRange(value.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                else
                    newElement.Attributes.Add(attribute.Value);
            }

            current.Children.Add(newElement);

        
            bool isSelfClosing = line.EndsWith("/")
                || HtmlHelper.Instance.HtmlVoidTags.Contains(tagName);

            if (!isSelfClosing)
                current = newElement;
        }
        
        else
        {
            current.InnerHtml += line;
        }
    }

    return root;
}

var root = BuildTree(htmlLines);

Console.WriteLine("enter selector:");

string selectorInput = Console.ReadLine();
var selector = Selector.creatSelectRoot(selectorInput);
var elements = root.FindElements(selector);

Console.WriteLine($"found: {elements.Count()}");
foreach (var e in elements)
    Console.WriteLine($"{e.Name} id={e.Id} class={string.Join(" ", e.Classes)}");