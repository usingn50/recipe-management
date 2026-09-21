# Recipe Management

Recipe Management is a small C# console application that demonstrates practical use of fundamental data structures for managing recipes, shopping lists, cooking plans, removal history, and cooking instructions.

## Requirements

- .NET 8 SDK or later

## Build and test

```bash
dotnet build
dotnet test
```

## Run

```bash
dotnet run --project RecipeManagement.Application
```

## Project structure

- `RecipeManagement.Application` — Console application and menu interaction.
- `RecipeManagement.Core` — Recipe models, JSON loading, and management logic.
- `RecipeManagement.Tests` — xUnit tests for the core functionality.
- `data/recipes.json` — Sample recipe data.
