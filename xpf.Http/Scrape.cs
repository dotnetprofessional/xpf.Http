using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace xpf.Http
{
    public class Scrape
    {
        string Expression { get; set; }
        string Content { get; set; }

        ExpressionGroupCollection Results { get; set; }

        public Scrape(string expression, string content)
        {
            this.Expression = expression;
            this.Content = content;
            this.Results = this.ScrapExpression(expression);
        }

        public Scrape And(string expression)
        {
            this.Expression = expression;
            foreach (var g in this.ScrapExpression(expression))
                this.Results.Add(g);

            return this;
        }

        public ExpressionGroupCollection Result()
        {
            return this.Results;
        }

        /// <summary>
        /// Provided a regular expression will evaluate it agains the raw result
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ExpressionGroupCollection ScrapExpression(string expression)
        {
            // https://msdn.microsoft.com/en-us/library/system.text.regularexpressions.regex.groupnamefromnumber(v=vs.110).aspx
            // Use above sampel to rewrite this taking advantage of the groups names. This needs its own method
            var foundValues = new ExpressionGroupCollection();

            Regex regex = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            List<string> groupNames = new List<string>();
            int index = 1;
            bool nameNotFound = false;
            // Get group names. 
            do
            {
                string name = regex.GroupNameFromNumber(index);
                if (!String.IsNullOrEmpty(name))
                {
                    index++;
                    groupNames.Add(name);
                }
                else
                {
                    nameNotFound = true;
                }
            } while (!nameNotFound);


            var match = regex.Matches(this.Content);
            foreach (string group in groupNames)
            {
                var expressionGroup = new ExpressionGroup();
                // Incase a subsequent expression uses the same group name use the previous collection
                if (this.Results != null && this.Results.Contains(group))
                    expressionGroup = this.Results[group];

                expressionGroup.Name = group;
                if (match.Count > 0)
                    for (int i = 0; i < match.Count; i++)
                    {
                        var values = match[i].Groups[group];
                        for (int c = 0; c < values.Captures.Count; c++)
                        {
                            expressionGroup.Values.Add(values.Captures[c].Value);
                        }
                    }
                foundValues.Add(expressionGroup);
            }

            return foundValues;
        }

    }
}