using RecipeManagement.Core.Models;

namespace RecipeManagement.Core;

public class RecipeManager : IRecipeManager
{
    private readonly Dictionary<int, Recipe> recipes = new();
    private readonly List<string> shoppingList = new();
    private readonly LinkedList<int> cookingPlan = new();
    private readonly Stack<int> removalHistory = new();
    private readonly Queue<string> instructionQueue = new();
    private int jsoncount;

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

    public void AddRecipeToPlan(int recipeId)
    {
        EnsureRecipeExists(recipeId);
        if (!cookingPlan.Contains(recipeId))
        {
            cookingPlan.AddLast(recipeId);
        }
    }

    public bool RemoveRecipeFromPlan(int recipeId)
    {
        if (!cookingPlan.Remove(recipeId))
        {
            return false;
        }

        removalHistory.Push(recipeId);
        return true;
    }

    public IReadOnlyList<int> GetCookingPlan() => cookingPlan.ToList().AsReadOnly();

    public bool RestoreLastRemovedRecipe()
    {
        if (removalHistory.Count == 0)
        {
            return false;
        }

        cookingPlan.AddLast(removalHistory.Pop());
        return true;
    }

    public bool StartCookingSession(int recipeId)
    {
        var recipe = GetRecipe(recipeId) ?? throw new KeyNotFoundException($"Recipe {recipeId} was not found.");
        instructionQueue.Clear();
        foreach (var instruction in recipe.Instructions)
        {
            instructionQueue.Enqueue(instruction);
        }

        return true;
    }

    public string? PeekNextInstruction() => instructionQueue.TryPeek(out var instruction) ? instruction : null;

    public string? CompleteNextInstruction() => instructionQueue.TryDequeue(out var instruction) ? instruction : null;

    private void EnsureRecipeExists(int recipeId)
    {
        if (!recipes.ContainsKey(recipeId))
        {
            throw new KeyNotFoundException($"Recipe {recipeId} was not found.");
        }
    }
}
