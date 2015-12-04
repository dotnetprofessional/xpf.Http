using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace xpf.Http
{
    public class BearerTokenProvider : IBearerTokenProvider
    {
        string Ns { get; set; }
        string Realm { get; set; }

        static Dictionary<string, BearerToken> _tokens = new Dictionary<string, BearerToken>();

        public BearerTokenProvider(string ns, string realm)
        {
            Ns = ns;
            Realm = realm;
        }

        public async Task<BearerToken> AcquireTokenAsync(string username, string password)
        {
            BearerToken token;
            var key = this.GetUniqueKey(username, password);
            if (_tokens.ContainsKey(key))
            {
                token = _tokens[key];
                if (!token.HasExpired)
                    return token;
                
                _tokens.Remove(key);
            }

            // No current token so get a new one
            token = await this.GetAcsBearerToken(username, password);
            // Record it for later use
            _tokens.Add(key, token);
            return token;
        }

        async Task<BearerToken> GetAcsBearerToken(string username, string password)
        {
            var http = new Http();
            var getUrl1 = await
                http.Navigate(
                    string.Format(
                        "https://{0}.accesscontrol.windows.net/v2/metadata/IdentityProviders.js?protocol=javascriptnotify&version=1.0&state=Passive&realm={1}",
                        this.Ns, this.Realm))
                    .UserAgent.IE11
                    .GetAsync<string>();

            var resultScrap = getUrl1.Scrape("LoginUrl\":\"(?<login>[^\"]*)")
                .And("wctx=(?<wctx>[^\"&]*)")
                .ResultByGroup();

            var login = resultScrap["login"].SafeValue;
            var wctx = resultScrap["wctx"].SafeValue;

            // Now navigate to the LoginUrl
            var getUrl2 = await getUrl1.Navigate(login).GetAsync<string>();
            var urlPostExpression = "urlPost:'(?<postUrl>[^']*)";

            var getUrl2Scrape = getUrl2.Scrape(urlPostExpression)
                .And("PPFT(?:[^v]*)value=\"(?<PPFT>[^\"]*)")
                .ResultByGroup();

            var postUrl = getUrl2Scrape["postUrl"].SafeValue;
            var ppft = getUrl2Scrape["PPFT"].SafeValue;

            var mspok = getUrl2.Cookies["MSPOK"].Value;



            // Now supply the credentials
            // Start a new session
            http = new Http();
            var postUrl1 = await http.Navigate(postUrl).Encoding.Gzip.UserAgent.IE11
                .WithFormValue("login", username)
                .WithFormValue("passwd", password)
                .WithFormValue("PPFT", ppft)
                .WithCookie("MSPOK", mspok)
                .PostAsync<string>();

            var postUrl1Scrape = postUrl1
                .Scrape("value=\"(?<wst><wst(?:[^\"])*)")
                .ResultByGroup();

            var wst = postUrl1Scrape["wst"].SafeValue;
            var wstDecoded = WebUtility.HtmlDecode(wst);

            var finalResult = await postUrl1.Navigate(string.Format("https://{0}.accesscontrol.windows.net/v2/wsfederation?wa=wsignin1.0", this.Ns))
                .Encoding.Gzip
                .WithFormValue("wa", "wsignin1.0")
                .WithFormValue("wctx", wctx)
                .WithFormValue("wresult", wstDecoded)
                .PostAsync<string>();

            var finalResultScrape = finalResult
                .Scrape("securityToken\":\"(?<token>[^\"]*)")
                .And("expires\":(?<expires>[\\d]*)")
                .And("created\":(?<created>[\\d]*)")
                .And("tokenType\":\"(?<tokenType>[^\"]*)")
                .ResultByGroup();

            var token = finalResultScrape["token"].SafeValue;
            return new BearerToken
            {
                Token = finalResultScrape["token"].SafeValue,
                Created = this.convertToDateTime(finalResultScrape["created"].SafeValue),
                Expires = this.convertToDateTime(finalResultScrape["expires"].SafeValue),
                Type = finalResultScrape["tokenType"].SafeValue,
            };
            //return token;
        }

        DateTime convertToDateTime(string duration)
        {
            return new DateTime(1970,1,1).AddSeconds(Convert.ToInt32(duration));
        }

        string GetUniqueKey(string username, string password)
        {
            return username + ":" + password;
        }
    }
}