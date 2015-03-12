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
    }

    public class ExpressionGroupCollection : KeyedCollection<string, ExpressionGroup>
    {
        protected override string GetKeyForItem(ExpressionGroup item)
        {
            return item.Name;
        }
    }
}
