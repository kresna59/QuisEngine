namespace QuizEngine.Core;

public interface IQuestion<T>
{
    string Text { get; set; }
    T CorrectAnswer { get; set; }
    bool Validate(T userAnswer);
}

public interface IQuestion  // Non-generic base interface
{
    string Text { get; set; }
}

public class MultipleChoiceQuestion : IQuestion<string>
{
    public required string Text { get; set; }
    public required List<string> Options { get; set; }
    public required string CorrectAnswer { get; set; }

    public bool Validate(string userAnswer)
        => string.Equals(userAnswer, CorrectAnswer, StringComparison.OrdinalIgnoreCase);
}

public class TrueFalseQuestion : IQuestion<bool>
{
    public required string Text { get; set; }
    public required bool CorrectAnswer { get; set; }

    public bool Validate(bool userAnswer)
        => userAnswer == CorrectAnswer;
}