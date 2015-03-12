using System.Linq;
using System.Net;
using System.Threading.Tasks;
using xpf.Http.Original;

namespace xpf.Http.Extensions
{
    public static class Classification
    {
        public static async Task<WebSiteClassification> GetClassification(this Url url)
        {
            var http = new Http();
            var fisrtRequest = await http.Url("http://global.sitesafety.trendmicro.com/index.php")
                .WithoutRedirect
                .GetAsync<string>();

            if (fisrtRequest.StatusCode == HttpStatusCode.OK | fisrtRequest.StatusCode == HttpStatusCode.Redirect)
            {
                var sessionCookie = fisrtRequest.Cookies.FirstOrDefault(c => c.Name == "PHPSESSID");

                // Now make the call to get the classifiation.
                http = new Http();
                var result = await http.Url("http://global.sitesafety.trendmicro.com/result.php")
                    .WithFormValue("urlname", url.Model.Url)
                    .WithFormValue("getinfo", "Check Now")
                    .WithCookie(sessionCookie)
                    .PostAsync<string>();

                var classificationsResult = result.Scrape("labeltitlesmallresult\">(?<classification>\\w*)")
                    .And("labeltitleresult\">(?<security>\\w*)").Result();

                var classification = new WebSiteClassification();
                var security = classificationsResult["security"].Values[0];
                classification.SecurityRisk = security;
                foreach(var c in classificationsResult["classification"].Values)
                    classification.Categories.Add(c);

                return classification;
            }
            // return an empty classification
            return new WebSiteClassification();
        }
    }
}
