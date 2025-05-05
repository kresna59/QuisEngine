using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuizEngine.Core;

public class QuestionConverter : JsonConverter<IQuestion>
{
    public override IQuestion Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            string type = root.GetProperty("Type").GetString()!;

            return type switch
            {
                "MultipleChoice" => JsonSerializer.Deserialize<MultipleChoiceQuestion>(root.GetRawText(), options)!,
                "TrueFalse" => JsonSerializer.Deserialize<TrueFalseQuestion>(root.GetRawText(), options)!,
                _ => throw new JsonException($"Unknown question type: {type}")
            };
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        IQuestion value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}