using RecipeManagement.Application;
using RecipeManagement.Core;

var manager = new RecipeManager();
var dataPath = Path.Combine(AppContext.BaseDirectory, "data", "recipes.json");
if (!File.Exists(dataPath))
{
    dataPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "recipes.json");
}

var loader = new RecipeLoader();
foreach (var recipe in loader.LoadRecipes(dataPath))
{
    manager.AddRecipe(recipe);
}

new ConsoleMenu(manager).Run();
