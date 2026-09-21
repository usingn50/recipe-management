using RecipeManagement.Core;
using RecipeManagement.Core.Models;

namespace RecipeManagement.Tests;

public class RecipeManagerTests
{
    private static Recipe CreateRecipe(int id = 1, string name = "Pancakes") => new()
    {
        Id = id,
        Name = name,
        Ingredients = new List<string> { "Flour", "Milk" },
        Instructions = new List<string> { "Mix", "Cook" },
        Nutrition = new NutritionInfo { Calories = 350 }
    };

    [Fact]
    public void AddRecipeAndGetRecipe_ReturnsRecipeById()
    {
        var manager = new RecipeManager();
        var recipe = CreateRecipe();

        manager.AddRecipe(recipe);

        Assert.Same(recipe, manager.GetRecipe(1));
        Assert.Null(manager.GetRecipe(99));
    }

    [Fact]
    public void AddRecipe_WithDuplicateId_ThrowsArgumentException()
    {
        var manager = new RecipeManager();
        manager.AddRecipe(CreateRecipe());

        Assert.Throws<ArgumentException>(() => manager.AddRecipe(CreateRecipe(1, "Another")));
    }

    [Fact]
    public void RemoveRecipe_ReturnsTrueWhenFoundAndFalseWhenMissing()
    {
        var manager = new RecipeManager();
        manager.AddRecipe(CreateRecipe());

        Assert.True(manager.RemoveRecipe(1));
        Assert.False(manager.RemoveRecipe(1));
        Assert.Null(manager.GetRecipe(1));
    }

    [Fact]
    public void ShoppingList_CopiesIngredientsInOriginalOrderAndCanBeCleared()
    {
        var manager = new RecipeManager();
        manager.AddRecipe(CreateRecipe());

        manager.AddRecipeIngredientsToShoppingList(1);

        Assert.Equal(new[] { "Flour", "Milk" }, manager.GetShoppingList());
        manager.ClearShoppingList();
        Assert.Empty(manager.GetShoppingList());
    }

    [Fact]
    public void ShoppingList_MissingRecipeThrowsKeyNotFoundException()
    {
        var manager = new RecipeManager();

        Assert.Throws<KeyNotFoundException>(() => manager.AddRecipeIngredientsToShoppingList(1));
    }

    [Fact]
    public void CookingPlan_UsesUniqueIdsAndPreservesInsertionOrder()
    {
        var manager = new RecipeManager();
        manager.AddRecipe(CreateRecipe(1));
        manager.AddRecipe(CreateRecipe(2, "Soup"));

        manager.AddRecipeToPlan(1);
        manager.AddRecipeToPlan(2);
        manager.AddRecipeToPlan(1);

        Assert.Equal(new[] { 1, 2 }, manager.GetCookingPlan());
    }

    [Fact]
    public void CookingPlan_RemoveReturnsFalseForMissingId()
    {
        var manager = new RecipeManager();
        manager.AddRecipe(CreateRecipe());
        manager.AddRecipeToPlan(1);

        Assert.True(manager.RemoveRecipeFromPlan(1));
        Assert.False(manager.RemoveRecipeFromPlan(1));
        Assert.Empty(manager.GetCookingPlan());
    }

    [Fact]
    public void RestoreLastRemovedRecipe_UsesMostRecentRemovalAndRestoresToEnd()
    {
        var manager = new RecipeManager();
        manager.AddRecipe(CreateRecipe(1));
        manager.AddRecipe(CreateRecipe(2, "Soup"));
        manager.AddRecipeToPlan(1);
        manager.AddRecipeToPlan(2);

        manager.RemoveRecipeFromPlan(1);
        manager.RemoveRecipeFromPlan(2);

        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.Equal(new[] { 2 }, manager.GetCookingPlan());
        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.Equal(new[] { 2, 1 }, manager.GetCookingPlan());
    }

    [Fact]
    public void RestoreLastRemovedRecipe_WithEmptyStackReturnsFalse()
    {
        var manager = new RecipeManager();

        Assert.False(manager.RestoreLastRemovedRecipe());
    }

    [Fact]
    public void CookingSession_PeeksAndCompletesInstructionsInQueueOrder()
    {
        var manager = new RecipeManager();
        manager.AddRecipe(CreateRecipe());

        Assert.True(manager.StartCookingSession(1));
        Assert.Equal("Mix", manager.PeekNextInstruction());
        Assert.Equal("Mix", manager.CompleteNextInstruction());
        Assert.Equal("Cook", manager.PeekNextInstruction());
        Assert.Equal("Cook", manager.CompleteNextInstruction());
        Assert.Null(manager.PeekNextInstruction());
        Assert.Null(manager.CompleteNextInstruction());
    }

    [Fact]
    public void EmptyInstructionQueue_ReturnsNullBeforeAnySession()
    {
        var manager = new RecipeManager();

        Assert.Null(manager.PeekNextInstruction());
        Assert.Null(manager.CompleteNextInstruction());
    }

    [Fact]
    public void ShoppingListAndCookingPlan_WorkTogetherForSameRecipe()
    {
        var manager = new RecipeManager();
        manager.AddRecipe(CreateRecipe());

        manager.AddRecipeIngredientsToShoppingList(1);
        manager.AddRecipeToPlan(1);

        Assert.Equal(new[] { "Flour", "Milk" }, manager.GetShoppingList());
        Assert.Equal(new[] { 1 }, manager.GetCookingPlan());
    }
}
