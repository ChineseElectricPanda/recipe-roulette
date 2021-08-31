using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeRoulette.Models
{
    public class RecipeTag
    {
        public long RecipeTagID { get; set; }
        public Tag Tag { get; set; }
    }
}
