using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeRoulette.Models.Scraper
{
    public class ScrapeBatchResult
    {
        public ICollection<string> succeededUrls { get; set; }
        public IDictionary<string, string> failedUrls { get; set; }

        public ScrapeBatchResult()
        {
            succeededUrls = new List<string>();
            failedUrls = new Dictionary<string, string>();
        }
    }
}
