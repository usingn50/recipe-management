using RecipeManagement.Core.Models;

namespace RecipeManagement.Core;

public interface IRecipeManager
{
    void AddRecipe(Recipe recipe);
    Recipe? GetRecipe(int id);
    bool RemoveRecipe(int id);
    IEnumerable<Recipe> GetAllRecipes();

    void AddRecipeIngredientsToShoppingList(int recipeId);
    IReadOnlyList<string> GetShoppingList();
    void ClearShoppingList();

    void AddRecipeToPlan(int recipeId);
    bool RemoveRecipeFromPlan(int recipeId);
    IReadOnlyList<int> GetCookingPlan();

    bool RestoreLastRemovedRecipe();

    bool StartCookingSession(int recipeId);
    string? PeekNextInstruction();
    string? CompleteNextInstruction();
}
