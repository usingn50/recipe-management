# Recipe Management

A runnable C# console solution demonstrating the five required collection types:
`Dictionary<int, Recipe>`, `List<string>`, `LinkedList<int>`, `Stack<int>`, and `Queue<string>`.

## Projects

- `RecipeManagement.Core`: models, loader, interface, and manager implementation.
- `RecipeManagement.Application`: console demonstration.
- `RecipeManagement.Tests`: executable xUnit tests.

## Run

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project RecipeManagement.Application
```

The supplied assignment mentions an official starter API in an appendix that was not included with the uploaded document. This project therefore uses a clear, conventional API based on the functional requirements. If an official `IRecipeManager` is later provided, its signatures should replace the local interface before submission.
