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
        public string Name(string name)
        {
            // <(?<type>[^\s]*)\sname="description"(?<attributes>[^>]*)>(?<text>[^<]*)
            //<(?<type>[^\s]*)(?<attributes2>[^"]*)?(?:name|id)="(?<id>[^"]*)"(?<attributes>[^/>]*)/?>(?<text>[^<]*)
            // <(?<type>[^\s/]*)(?<attributesLeft>[^>]*?)(?:(?:name|id)="(?<id>[^"]*?)")(?<attributesRight>[^>]*?)>(?:(?<text>[^<]*)</)?
            var regEx = "<(?<type>[^\\s/]*)(?<attributesLeft>[^>]*?)(?:(?:name|id)=\"{0}\")(?<attributesRight>[^>]*?)>(?:(?<text>[^<]*)</)?";


            var value = this.Response.Scrape(string.Format(regEx, name)).ResultByMatch();

            // extract any matches
            for (int i = 0; i < value.Count; i++)
            {
                //var element = new HttpElement(value["id"].Values[0]);
            }

            return "";
        }
    }

    public class HttpElement
    {
        public HttpElement(string id, string text, string type, string attributres)
        {
            this.Id = id;
            this.Text = text;
            this.Type = type;
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
            throw new NotImplementedException();
        }
    }
}
