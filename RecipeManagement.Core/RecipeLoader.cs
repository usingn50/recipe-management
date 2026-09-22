using System.Text.Json;
using RecipeManagement.Core.Models;

namespace RecipeManagement.Core;

public static class RecipeLoader
{
    public static List<Recipe> Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Recipe data file was not found.", filePath);
        }

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Recipe>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Recipe>();
    }
}
