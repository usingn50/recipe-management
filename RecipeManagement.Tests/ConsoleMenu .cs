using System;
using System.Collections.Generic;
using RecipeManagement.Core;
using RecipeManagement.Core.Models;

namespace RecipeManagement.Application
{
    public class ConsoleMenu
    {
        private readonly RecipeManager _manager;

        public ConsoleMenu(RecipeManager manager)
        {
            _manager = manager;
        }

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("\n--- Recipe Management Menu ---");
                Console.WriteLine("1. List all recipes");
                Console.WriteLine("2. Find recipe by id");
                Console.WriteLine("3. Add recipe to shopping list");
                Console.WriteLine("4. View shopping list");
                Console.WriteLine("5. Clear shopping list");
                Console.WriteLine("6. Add recipe to cooking plan");
                Console.WriteLine("7. View cooking plan");
                Console.WriteLine("8. Remove recipe from cooking plan");
                Console.WriteLine("9. Restore last removed recipe");
                Console.WriteLine("10. Start cooking");
                Console.WriteLine("11. Show next instruction");
                Console.WriteLine("12. Complete next instruction");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                string? choice = Console.ReadLine();

                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        foreach (var r in _manager.GetRecipes())
                            Console.WriteLine(r.Id + ": " + r.Name);
                        break;

                    case "2":
                        Console.Write("Enter id: ");
                        int findId = int.Parse(Console.ReadLine());
                        var found = _manager.FindRecipe(findId);
                        if (found == null)
                        {
                            Console.WriteLine("Not found.");
                        }
                        else
                        {
                            Console.WriteLine("Name: " + found.Name);
                            Console.Write("Ingredients: ");
                            foreach (var item in found.Ingredients)
                                Console.Write(item + " ");
                            Console.WriteLine();
                            Console.Write("Instructions: ");
                            foreach (var item in found.Instructions)
                                Console.Write(item + " | ");
                            Console.WriteLine();
                        }
                        break;

                    case "3":
                        Console.Write("Enter recipe id: ");
                        int shopId = int.Parse(Console.ReadLine());
                        if (_manager.AddRecipeToShoppingList(shopId))
                            Console.WriteLine("Added.");
                        else
                            Console.WriteLine("Recipe not found.");
                        break;

                    case "4":
                        var list = _manager.GetShoppingList();
                        if (list.Count == 0) Console.WriteLine("Shopping list is empty.");
                        else foreach (var item in list) Console.WriteLine("- " + item);
                        break;

                    case "5":
                        _manager.ClearShoppingList();
                        Console.WriteLine("Shopping list cleared.");
                        break;

                    case "6":
                        Console.Write("Enter recipe id: ");
                        int planId = int.Parse(Console.ReadLine());
                        if (_manager.AddToCookingPlan(planId))
                            Console.WriteLine("Added to plan.");
                        else
                            Console.WriteLine("Cannot add.");
                        break;

                    case "7":
                        var plan = _manager.GetCookingPlan();
                        if (plan.Count == 0) Console.WriteLine("Cooking plan is empty.");
                        else
                        {
                            Console.Write("Plan: ");
                            foreach (var id in plan) Console.Write(id + " ");
                            Console.WriteLine();
                        }
                        break;

                    case "8":
                        Console.Write("Enter recipe id: ");
                        int removeId = int.Parse(Console.ReadLine());
                        if (_manager.RemoveFromCookingPlan(removeId))
                            Console.WriteLine("Removed.");
                        else
                            Console.WriteLine("Not in plan.");
                        break;

                    case "9":
                        if (_manager.RestoreLastRemovedRecipe())
                            Console.WriteLine("Restored.");
                        else
                            Console.WriteLine("Nothing to restore.");
                        break;

                    case "10":
                        Console.Write("Enter recipe id: ");
                        int cookId = int.Parse(Console.ReadLine());
                        if (_manager.StartCooking(cookId))
                            Console.WriteLine("Cooking started.");
                        else
                            Console.WriteLine("Recipe not found.");
                        break;

                    case "11":
                        string? next = _manager.PeekNextInstruction();
                        if (next == null) Console.WriteLine("No instruction.");
                        else Console.WriteLine("Next: " + next);
                        break;

                    case "12":
                        string? done = _manager.CompleteNextInstruction();
                        if (done == null) Console.WriteLine("No instruction.");
                        else Console.WriteLine("Done: " + done);
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}