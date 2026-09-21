using RecipeManagement.Core.Models;

namespace RecipeManagement.Core;

public class RecipeManager : IRecipeManager
{
    private readonly Dictionary<int, Recipe> recipes = new();
    private readonly List<string> shoppingList = new();

    public void AddRecipe(Recipe recipe)
    {
        ArgumentNullException.ThrowIfNull(recipe);
        if (!recipes.TryAdd(recipe.Id, recipe))
        {
            throw new ArgumentException($"A recipe with ID {recipe.Id} already exists.", nameof(recipe));
        }
    }

    public Recipe? GetRecipe(int id) => recipes.GetValueOrDefault(id);

    public bool RemoveRecipe(int id) => recipes.Remove(id);

    public IEnumerable<Recipe> GetAllRecipes() => recipes.Values;

    public void AddRecipeIngredientsToShoppingList(int recipeId)
    {
        var recipe = GetRecipe(recipeId) ?? throw new KeyNotFoundException($"Recipe {recipeId} was not found.");
        shoppingList.AddRange(recipe.Ingredients);
    }

    public IReadOnlyList<string> GetShoppingList() => shoppingList.AsReadOnly();

    public void ClearShoppingList() => shoppingList.Clear();

    public void AddRecipeToPlan(int recipeId) => throw new NotImplementedException();

    public bool RemoveRecipeFromPlan(int recipeId) => throw new NotImplementedException();

    public IReadOnlyList<int> GetCookingPlan() => Array.Empty<int>();

    public bool RestoreLastRemovedRecipe() => false;

    public bool StartCookingSession(int recipeId) => throw new NotImplementedException();

    public string? PeekNextInstruction() => null;

    public string? CompleteNextInstruction() => null;
}
