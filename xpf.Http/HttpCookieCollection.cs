using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace xpf.Http
{
    public class HttpCookieCollection : KeyedCollection<string, HttpCookie>
    {
        protected override string GetKeyForItem(HttpCookie item)
        {
            return item.Name;
        }

        public void AddRange(IEnumerable<HttpCookie> items)
        {
            foreach (var i in items)
            {
                if (this.Contains(i.Name))
                    this.Remove(i.Name);
                this.Add(i);
            }

        }
    }
}