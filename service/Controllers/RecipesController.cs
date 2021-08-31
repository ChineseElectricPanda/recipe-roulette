using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeRoulette.Models;

namespace RecipeRoulette.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly RecipeRouletteContext _context;

        public RecipesController(RecipeRouletteContext context)
        {
            _context = context;
        }

        // GET: api/Recipes/Random/3
        [HttpGet("Random/{num}")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRandomRecipes(int num)
        {
            if (num > 10)
            {
                return BadRequest("Cannot request more than 10 recipes at once");
            }

            return await _context.Recipes
                .OrderBy(r => Guid.NewGuid())
                .Take(num)
                .Include(r => r.RecipeTags.OrderBy(rt => rt.RecipeTagID))
                    .ThenInclude(rt => rt.Tag)
                .Include(r => r.RecipeIngredients.OrderBy(ri => ri.RecipeIngredientID))
                    .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            return await _context.Recipes
                .Include(r => r.RecipeTags.OrderBy(rt => rt.RecipeTagID))
                    .ThenInclude(rt => rt.Tag)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(long id)
        {
            var recipe = await _context.Recipes
                .Where(r => r.RecipeID == id)
                .Include(r => r.RecipeTags.OrderBy(rt => rt.RecipeTagID))
                    .ThenInclude(rt => rt.Tag)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync<Recipe>();

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        // PUT: api/Recipes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(long id, Recipe recipe)
        {
            if (id != recipe.RecipeID)
            {
                return BadRequest();
            }

            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Recipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipe", new { id = recipe.RecipeID }, recipe);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(long id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeExists(long id)
        {
            return _context.Recipes.Any(e => e.RecipeID == id);
        }
    }
}
