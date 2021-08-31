using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeRoulette.Models
{
    public class Recipe
    {
        public long RecipeID { get; set; }
        public string RecipeURL { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string PrepTime { get; set; }
        public string CookTime { get; set; }
        public string TotalTime { get; set; }

        public double Rating { get; set; }
        public string ImageURL { get; set; }

        public ICollection<RecipeTag> RecipeTags { get; set; }

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }

        public Recipe()
        {
            RecipeTags = new HashSet<RecipeTag>();
            RecipeIngredients = new HashSet<RecipeIngredient>();
        }
    }
}
