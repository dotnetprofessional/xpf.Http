namespace xpf.Http
{
    public class PageDetail<T>
    {
        public PageDetail(HttpResponse<T> response)
        {
            var metaDataRegEx = "name=\"{0}\"[\\s]*content=\"(?<{0}>[^\"]*)";
            // Get all the attributes of interest
            var details = response
                .Scrape(@"<title>(?<title>[^<]*)</title>")
                .And(string.Format(metaDataRegEx, "description"))
                .And(string.Format(metaDataRegEx, "keywords"))
                .And("(?<flash>\\.swf|flashplayer)")
                .ResultByGroup();

            // Reset other fields to empty strings rather than have them as nulls
            this.Title = details["title"].SafeValue;
            this.Description = details["description"].SafeValue;
            this.Keywords = details["keywords"].SafeValue;
            this.SupportsFlash = details["flash"].Values.Count > 0;
        }

        public string Title { get; private set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public bool SupportsFlash { get; set; }
    }
}