using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeRoulette.Models
{
    public class RecipeIngredient
    {
        public long RecipeIngredientID { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
