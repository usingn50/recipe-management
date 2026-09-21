using RecipeManagement.Core;

namespace RecipeManagement.Application;

public class ConsoleMenu
{
    private readonly IRecipeManager manager;

    public ConsoleMenu(IRecipeManager manager)
    {
        this.manager = manager;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\nRecipe Management");
            Console.WriteLine("1. List recipes");
            Console.WriteLine("2. Add ingredients to shopping list");
            Console.WriteLine("3. View shopping list");
            Console.WriteLine("4. Add recipe to cooking plan");
            Console.WriteLine("5. View cooking plan");
            Console.WriteLine("6. Start cooking session");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1": ListRecipes(); break;
                case "2": AddIngredients(); break;
                case "3": ViewShoppingList(); break;
                case "4": AddToPlan(); break;
                case "5": ViewPlan(); break;
                case "6": CookRecipe(); break;
                case "0": return;
                default: Console.WriteLine("Unknown option."); break;
            }
        }
    }

    private void ListRecipes()
    {
        foreach (var recipe in manager.GetAllRecipes())
        {
            Console.WriteLine($"{recipe.Id}: {recipe.Name}");
        }
    }

    private void AddIngredients()
    {
        if (TryReadId(out var id))
        {
            try
            {
                manager.AddRecipeIngredientsToShoppingList(id);
                Console.WriteLine("Ingredients added.");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private void ViewShoppingList()
    {
        foreach (var ingredient in manager.GetShoppingList())
        {
            Console.WriteLine($"- {ingredient}");
        }
    }

    private void AddToPlan()
    {
        if (TryReadId(out var id))
        {
            try
            {
                manager.AddRecipeToPlan(id);
                Console.WriteLine("Recipe added to plan.");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private void ViewPlan()
    {
        Console.WriteLine(string.Join(" -> ", manager.GetCookingPlan()));
    }

    private void CookRecipe()
    {
        if (!TryReadId(out var id))
        {
            return;
        }

        try
        {
            manager.StartCookingSession(id);
            while (manager.PeekNextInstruction() is { } instruction)
            {
                Console.WriteLine(instruction);
                manager.CompleteNextInstruction();
            }
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static bool TryReadId(out int id)
    {
        Console.Write("Recipe ID: ");
        if (int.TryParse(Console.ReadLine(), out id))
        {
            return true;
        }

        Console.WriteLine("Please enter a valid numeric ID.");
        return false;
    }
}
