using System;
using System.Text.RegularExpressions;

namespace xpf.Http.Original
{
    public class UriDetail
    {
        string _title;
        string _description;
        string _keywords;
        bool _supportsFlash;

        public UriDetail(string url, string htmlContent)
        {
            this.Url = url;
            this.Html = htmlContent;

            // Reset other fields to empty strings rather than have them as nulls
            this._title = string.Empty;
            this._description = string.Empty;
            this._keywords = string.Empty;
        }

        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(this._title))
                    this._title= this.GetMatchText(this.Html, @"<title>([\s\S]*)</title>");

                return this._title; 
                
            }
        }

        public string Description
        {
            get
            {
                if(string.IsNullOrEmpty(this._description))
                    this._description = this.GetMatchText(this.Html, "<meta name=\"description\"(?:.*)content=\"(.*)\"");

                return this._description;
            }
        }

        public string Url { get; private set; }

        public string Html { get; private set; }

        public string Keywords
        {
            get
            {
                if(string.IsNullOrEmpty(this._keywords))
                    this._keywords = this.GetMatchText(this.Html, "<meta name=\"keywords\" content=\"(.*)\"");

                return this._keywords;
            }
        }

        bool _supportsFlashSet = false;
        public bool SupportsFlash
        {
            get
            {
                if(!this._supportsFlashSet)
                    this._supportsFlash = !string.IsNullOrWhiteSpace(this.GetMatchText(this.Html, @"(\.swf|flashplayer)"));

                return this._supportsFlash;
            }
        }


        string GetMatchText(string text, string pattern)
        {
            Match match = Regex.Match(text, pattern);
            if (match.Success && match.Groups.Count == 2)
            {
                string value = match.Groups[1].Value;
                value = value.Replace(Environment.NewLine, "").Trim();
                return value;
            }
            return "";
        }

    }
}