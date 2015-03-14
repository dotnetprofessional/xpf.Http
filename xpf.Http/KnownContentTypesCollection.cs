using System.Collections.ObjectModel;

namespace xpf.Http
{
    public class KnownContentTypesCollection : KeyedCollection<string, IContentType>
    {
        protected override string GetKeyForItem(IContentType item)
        {
            return item.ContentType;
        }
    }
}