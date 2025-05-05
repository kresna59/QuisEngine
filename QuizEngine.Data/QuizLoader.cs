using Newtonsoft.Json;
using QuizEngine.Core.Models;

namespace QuizEngine.Data;

public static class QuizLoader
{
    public static Quiz LoadFromJson(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<Quiz>(json)
            ?? throw new InvalidOperationException("Failed to load quiz");
    }
}