using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeRoulette.Models
{
    public class Tag
    {
        public long TagID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Color { get; set; }
        public string BackgroundColor { get; set; }
    }
}
