using Microsoft.EntityFrameworkCore;

namespace RecipeRoulette.Models
{
    public class RecipeRouletteContext : DbContext
    {
        public RecipeRouletteContext(DbContextOptions<RecipeRouletteContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData(modelBuilder);
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Tag> Tags { get; set; }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().HasData(new Tag[]{
                new Tag { TagID = 1, Name = "GF", FullName = "Gluten-Free",         Color = "#2A2B2A", BackgroundColor = "#FFDED6" },
                new Tag { TagID = 2, Name = "VG", FullName = "Vegan",               Color = "#FFFFFF", BackgroundColor = "#F74639" },
                new Tag { TagID = 3, Name = "V",  FullName = "Vegetarian",          Color = "#FFFFFF", BackgroundColor = "#D56638" },
                new Tag { TagID = 4, Name = "DF", FullName = "Dairy-Free",          Color = "#FFFFFF", BackgroundColor = "#003811" },
                new Tag { TagID = 5, Name = "NS", FullName = "Naturally Sweetened", Color = "#FFFFFF", BackgroundColor = "#2A2B2A" },
            });
        }
    }
}
