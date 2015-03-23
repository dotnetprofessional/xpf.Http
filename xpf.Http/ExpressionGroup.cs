using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace xpf.Http
{
    public class ExpressionGroup
    {
        public ExpressionGroup()
        {
            this.Values = new List<string>();
        }

        public string Name { get; set; }

        public List<string> Values { get; set; }

        public string SafeValue
        {
            get
            {
                if (this.Values.Count > 0)
                    return this.Values[0];
                else
                    return "";
            }
        }
    }

    public class ExpressionGroupCollection : KeyedCollection<string, ExpressionGroup>
    {
        protected override string GetKeyForItem(ExpressionGroup item)
        {
            return item.Name;
        }
    }
}
