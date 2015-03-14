using System.Linq;
using System.Net;
using System.Threading.Tasks;
using xpf.Http.Original;

namespace xpf.Http.Extensions
{
    public static class Classification
    {
        public static async Task<WebSiteClassification> GetClassification(this NavigationContext navigationContext)
        {
            var http = new Http();
            var fisrtRequest = await http.Navigate("http://global.sitesafety.trendmicro.com/index.php")
                .UserAgent.IE11
                .WithoutRedirect
                .GetAsync<string>().Result.Navigate("http://global.sitesafety.trendmicro.com/result.php")
                .WithFormValue("urlname", navigationContext.Model.Url)
                .WithFormValue("getinfo", "Check Now")
                .PostAsync<string>();

            if (fisrtRequest.StatusCode == HttpStatusCode.OK | fisrtRequest.StatusCode == HttpStatusCode.Redirect)
            {
                var classificationsResult = fisrtRequest.Scrape("labeltitlesmallresult\">(?<classification>\\w*)")
                    .And("labeltitleresult\">(?<security>\\w*)").Result();

                var classification = new WebSiteClassification();
                var security = classificationsResult["security"].Values[0];
                classification.SecurityRisk = security;
                foreach (var c in classificationsResult["classification"].Values)
                    classification.Categories.Add(c);

                return classification;
            }            

            // return an empty classification
            return new WebSiteClassification();
        }
    }
}
