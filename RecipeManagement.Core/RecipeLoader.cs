using System.Text.Json;
using RecipeManagement.Core.Models;

namespace RecipeManagement.Core;

public class RecipeLoader
{
    public List<Recipe> LoadRecipes(string path)
    {
        var json = File.ReadAllText(path);
        var data = JsonSerializer.Deserialize<RecipeDataFile>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return data?.Recipes ?? new List<Recipe>();
    }
}
