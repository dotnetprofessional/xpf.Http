using System.Collections.Generic;
using System.Collections.ObjectModel;
using xpf.Http.Original;

namespace xpf.Http
{
    public class HttpHeaderCollection : KeyedCollection<string, HttpHeader>
    {
        protected override string GetKeyForItem(HttpHeader item)
        {
            return item.Key;
        }

        public void AddRange(IEnumerable<HttpHeader> items)
        {
            foreach (var i in items)
            {
                if (this.Contains(i.Key))
                    this.Remove(i.Key);
                this.Add(i);
            }

        }
    }
}