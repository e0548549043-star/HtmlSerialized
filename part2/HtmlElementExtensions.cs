using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace part2
{
    internal static class HtmlElementExtensions
    {

        public static IEnumerable<HtmlElement> FindElements(this HtmlElement element, Selector selector)
        {
            var result = new HashSet<HtmlElement>();
            FindElementsBySelector(element, selector, result);
            return result;
        }
        private static  void FindElementsBySelector(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
        {
            var descendants = element.Descendants().Skip(1);

            var filtered = descendants.Where(e => e.Matches(selector));

            if (selector.Child == null)
            {
                foreach (var e in filtered)
                    result.Add(e);
                return;
            }
            foreach (var e in filtered)
                FindElementsBySelector(e, selector.Child, result);
        }

        private static  bool Matches(this HtmlElement element, Selector selector)
        {
            if (selector.TagName != null && element.Name != selector.TagName)
                return false;
            if (selector.Id != null && element.Id != selector.Id)
                return false;
            if (selector.Classes != null && !selector.Classes.All(c => element.Classes.Contains(c)))
                return false;
            return true;
        }

    }
}
