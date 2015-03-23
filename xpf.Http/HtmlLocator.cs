using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xpf.Http
{
    public class HtmlLocator<T>
    {
        HttpResponse<T> Response { get; set; }
        ExpressionGroupCollection Results { get; set; }
        
        public HtmlLocator(HttpResponse<T> response)
        {
            this.Response = response;
        }

        /// <summary>
        /// Provides the attributes and value of a tag with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>
        /// This method does not support returning nested tags. If a tag has another tag has nested tags
        /// then value will return as an empty string.
        /// </remarks>
        public HttpElement Name(string name)
        {
            // <(?<type>[^\s]*)\sname="description"(?<attributes>[^>]*)>(?<text>[^<]*)
            //<(?<type>[^\s]*)(?<attributes2>[^"]*)?(?:name|id)="(?<id>[^"]*)"(?<attributes>[^/>]*)/?>(?<text>[^<]*)
            // <(?<type>[^\s/]*)(?<attributesLeft>[^>]*?)(?:(?:name|id)="(?<id>[^"]*?)")(?<attributesRight>[^>]*?)>(?:(?<text>[^<]*)</)?
            var regEx = "<(?<type>[^\\s/]*)(?<attributesLeft>[^>]*?)(?:(?:name|id)=\"{0}\")(?<attributesRight>[^>]*?)>(?:(?<text>[^<]*)</)?";


            var value = this.Response.Scrape(string.Format(regEx, name)).ResultByMatch();

            // extract any matches
            foreach (var match in value)
            {
                // There are a number of groups needed to build the element
                var type = match["type"].SafeValue;
                var attributesLeft = match["attributesLeft"].SafeValue;
                var attributesRight = match["attributesRight"].SafeValue;
                var text = match["text"].SafeValue;

                var element = new HttpElement(name, text, type, attributesLeft + attributesRight);
                return element;
            }

            return null;
        }
    }

    public class HttpElement
    {
        public HttpElement(string id, string text, string type, string attributes)
        {
            this.Attributes = new HttpAttributeCollection();
            this.Id = id;
            this.Text = text;
            this.Type = type;

            // Process the attributes
            var attributeParts = new Scrape("(?<name>[^=]*)[=\\s\"]*(?<value>[^\"]*)\"", attributes).ResultByMatch();
            foreach(var m in attributeParts)
                this.Attributes.Add(new HttpAttribute{Name = m["name"].SafeValue, Value = m["value"].SafeValue});
        }

        public HttpElement()
        {
            this.Attributes = new HttpAttributeCollection();
        }

        public string Id { get; set; }
        public string Text { get; set; }

        public string Type { get; set; }

        public HttpAttributeCollection Attributes { get; set; } 
    }

    public class HttpAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class HttpAttributeCollection : KeyedCollection<string, HttpAttribute>
    {
        protected override string GetKeyForItem(HttpAttribute item)
        {
            return item.Name;
        }
    }
}
