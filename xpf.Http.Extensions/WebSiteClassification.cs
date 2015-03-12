using System.Collections.Generic;

namespace xpf.Http.Extensions
{
    public class WebSiteClassification
    {
        public WebSiteClassification()
        {
            this.Categories = new List<string>();
        }

        public string SecurityRisk { get; set; }

        public List<string> Categories { get; set; }
    }
}