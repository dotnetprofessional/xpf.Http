using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace xpf.Http
{
    public class Scrape
    {
        string Expression { get; set; }
        string Content { get; set; }

        Dictionary<string, string> GroupsReferenced = new Dictionary<string, string>();
            List<ExpressionGroupCollection> Results { get; set; }

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

        public List<ExpressionGroupCollection> ResultByMatch()
        {
            return this.Results;
        }

        public ExpressionGroupCollection ResultByGroup()
        {
            var expressionGroups = new ExpressionGroupCollection();

            // Create all the referenced groups with no values first
            // This makes using the result easier as the caller need not check 
            // if the group exists.

            foreach(var g in this.GroupsReferenced)
                expressionGroups.Add(new ExpressionGroup{Name = g.Value});

            foreach (var m in this.Results)
            {
                foreach (var g in m)
                {
                    var existingGroup = expressionGroups.FirstOrDefault(groups => groups.Name == g.Name);
                    existingGroup.Values.AddRange(g.Values);
                }
            }
            return expressionGroups;
        }

        /// <summary>
        /// Provided a regular expression will evaluate it agains the raw result
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        List<ExpressionGroupCollection> ScrapExpression(string expression)
        {
            // https://msdn.microsoft.com/en-us/library/system.text.regularexpressions.regex.groupnamefromnumber(v=vs.110).aspx
            // Use above sampel to rewrite this taking advantage of the groups names. This needs its own method
            var matchesFound = new List<ExpressionGroupCollection>();

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
                    if(!this.GroupsReferenced.ContainsKey(name))
                        this.GroupsReferenced.Add(name,name);
                }
                else
                {
                    nameNotFound = true;
                }
            } while (!nameNotFound);


            var matches = regex.Matches(this.Content);
            for (int i = 0; i < matches.Count; i++)
            {
                var expressionGroups = new ExpressionGroupCollection();
                foreach (string group in groupNames)
                {
                    // Incase a subsequent expression uses the same group name use the previous collection
                    var expressionGroup = new ExpressionGroup();
                    expressionGroup.Name = group;
                    var values = matches[i].Groups[group];
                    for (int c = 0; c < values.Captures.Count; c++)
                    {
                        expressionGroup.Values.Add(values.Captures[c].Value);
                    }
                    expressionGroups.Add(expressionGroup);
                }
                matchesFound.Add(expressionGroups);
            }

            return matchesFound;
        }

    }
}