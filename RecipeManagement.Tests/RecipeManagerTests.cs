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
}
