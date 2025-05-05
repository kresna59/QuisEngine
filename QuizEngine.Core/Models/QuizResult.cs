namespace QuizEngine.Core.Models;

public class QuizResult
{
    public int Score { get; }
    public int TotalQuestions { get; }
    public double Percentage => TotalQuestions > 0 ?
        Math.Round((double)Score / TotalQuestions * 100, 2) : 0;
    public DateTime CompletedAt { get; } = DateTime.UtcNow;

    public QuizResult(int score, int totalQuestions)
    {
        Score = score;
        TotalQuestions = totalQuestions;
    }

    public override string ToString() =>
        $"You scored {Score}/{TotalQuestions} ({Percentage}%)";
}