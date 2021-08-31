using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeRoulette.Models;
using RecipeRoulette.Models.Scraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeRoulette.Controllers
{
    [Route("/api")]
    [ApiController]
    public class ScrapeRecipeController : ControllerBase
    {
        private readonly ILogger<ScrapeRecipeController> _logger;
        private readonly RecipeRouletteContext _context;

        private readonly HttpClient _httpClient;

        public ScrapeRecipeController(ILogger<ScrapeRecipeController> logger, RecipeRouletteContext context)
        {
            _logger = logger;
            _context = context;
            _httpClient = new HttpClient();
        }

        [HttpPost("ScrapeAll/{startPage}/{endPage}")]
        public async Task<IActionResult> ScrapeAll(int startPage, int endPage)
        {
            ScrapeBatchResult result = new ScrapeBatchResult();

            for (int page = startPage; page <= endPage; page++)
            {
                await ScrapePage(result, page);
            }

            return Ok(result);
        }

        [HttpPost("ScrapeURL")]
        public async Task<IActionResult> ScrapeRecipe(ScrapeRequest scrapeRequest)
        {
            await ScrapeURL(scrapeRequest.URL);
            return Ok();
        }

        private async Task ScrapePage(ScrapeBatchResult result, int page)
        {
            string response = await _httpClient.GetStringAsync("https://minimalistbaker.com/recipe-index/?fwp_paged=" + page);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            // Find all links to recipies on the page.
            foreach (HtmlNode linkNode in doc.QuerySelectorAll("a.post-summary__image"))
            {
                string url = linkNode.GetAttributeValue<string>("href", "");

                try
                {
                    await ScrapeURL(url);
                    result.succeededUrls.Add(url);
                }
                catch (Exception e)
                {
                    result.failedUrls.Add(url, e.ToString());
                }
            }
        }

        private async Task ScrapeURL(string url)
        {
            Recipe existingRecipe = _context.Recipes
                .Where(recipe => (recipe.RecipeURL == url))
                .SingleOrDefault();

            if (existingRecipe != null)
            {
                throw new Exception("Recipe already exists");
            }

            _logger.LogInformation("Scraping {0}", url);
            string response = await _httpClient.GetStringAsync(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            if (doc.QuerySelector(".wprm-recipe-jump") == null)
            {
                throw new Exception("Not a recipe page");
            }

            string title = doc.QuerySelector(".wprm-recipe-name").InnerText;
            string description = doc.QuerySelector(".wprm-recipe-summary").InnerText;

            string prepTime = doc.QuerySelector(".wprm-recipe-prep-time-container .wprm-recipe-time")?.InnerText;
            string cookTime = doc.QuerySelector(".wprm-recipe-cook-time-container .wprm-recipe-time")?.InnerText;
            string totalTime = doc.QuerySelector(".wprm-recipe-total-time-container .wprm-recipe-time").InnerText;
            
            string rating = doc.QuerySelector(".wprm-recipe-rating-average")?.InnerText;
            string imageUrl = doc.QuerySelector(".wp-block-image img").GetAttributeValue<string>("data-cfsrc", "");

            Recipe recipe = new Recipe {
                Name = title,
                Description = description,

                PrepTime = prepTime,
                CookTime = cookTime,
                TotalTime = totalTime,

                RecipeURL = url,
                Rating = rating == null ? 0 : Double.Parse(rating),
                ImageURL = imageUrl,
            };

            // Create the tags if necessary and add them to the recipe object.
            foreach (HtmlNode tagNode in doc.QuerySelectorAll("a.entry-key-categories__circle"))
            {
                string tagName = tagNode.InnerText;

                Tag tag = _context.Tags
                    .Where(tag => (tag.Name == tagName))
                    .SingleOrDefault();

                RecipeTag recipeTag = new RecipeTag { Tag = tag };

                recipe.RecipeTags.Add(recipeTag);
            }

            // Create the ingredients if necessary and create and add the relation to the recipe.
            foreach (HtmlNode ingredientNode in doc.QuerySelectorAll(".wprm-recipe-ingredient"))
            {
                string ingredientName = ingredientNode.QuerySelector(".wprm-recipe-ingredient-name").InnerText.Trim();
                string ingredientQuantity = ingredientNode.QuerySelector(".wprm-recipe-ingredient-amount")?.InnerText.Trim();
                string ingredientUnit = ingredientNode.QuerySelector(".wprm-recipe-ingredient-unit")?.InnerText.Trim();

                Ingredient ingredient = _context.Ingredients
                    .Where(ingredient => (ingredient.Name == ingredientName))
                    .SingleOrDefault();

                if (ingredient == null)
                {
                    ingredient = new Ingredient { Name = ingredientName };
                    _context.Ingredients.Add(ingredient);
                }

                RecipeIngredient recipeIngredient = new RecipeIngredient {
                    Ingredient = ingredient,
                    Quantity = ingredientQuantity,
                    Unit = ingredientUnit
                };

                recipe.RecipeIngredients.Add(recipeIngredient);
                _context.RecipeIngredients.Add(recipeIngredient);
            }

            // Add the recipe and commit.
            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            _logger.LogInformation("Finished Scraping: {0}", url);
        }
    }
}
