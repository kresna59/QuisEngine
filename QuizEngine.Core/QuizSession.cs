namespace QuizEngine.Core;

public class QuizSession
{
    public QuizState CurrentState { get; set; } = QuizState.NotStarted;
    public int CurrentQuestionIndex { get; set; } = 0;
    public int Score { get; set; } = 0;

    public void MoveNext() => CurrentQuestionIndex++;
    public void Complete() => CurrentState = QuizState.Completed;
}