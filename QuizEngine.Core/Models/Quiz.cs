using System.Text.Json.Serialization;

namespace QuizEngine.Core.Models;

public class Quiz
{
    public required string Title { get; set; }

    [JsonConverter(typeof(QuestionConverter))]
    public required List<IQuestion> Questions { get; set; }
}