using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace part2
{
    internal class Selector
    {
        public string TagName { get; set; }

        public string Id { get; set; }

        public List<string> Classes { get; set; } = new List<string>();

        public Selector Parent { get; set; }

        public Selector Child { get; set; }

        public static Selector creatSelectRoot(string select)
        {
            var selects = select.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Selector root = null;
            Selector current = null;

            foreach (var sel in selects)
            {
                var selector = new Selector();
                var parts = Regex.Split(sel, "(?=[#.])");

                foreach (var part in parts)
                {
                    if (part.StartsWith("#"))
                        selector.Id = part.Substring(1);            
                    else if (part.StartsWith("."))
                        selector.Classes.Add(part.Substring(1));   
                    else if (HtmlHelper.Instance.HtmlTags.Contains(part)|| HtmlHelper.Instance.HtmlVoidTags.Contains(part))
                        selector.TagName = part;         
                }
                if (root!= null)
                { current.Child = selector;
                    selector.Parent = current;
                    current = selector;
                }
                else
                {
                    root = selector;
                    current = selector;
                }

            }
            return root;
        }
    }
}
