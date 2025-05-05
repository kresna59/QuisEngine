using QuizEngine.Core.Models;

namespace QuizEngine.Core;

public class QuizEngineCore
{
    private readonly Quiz _quiz;
    private QuizSession _session = new();

    public QuizEngineCore(Quiz quiz)
    {
        _quiz = quiz ?? throw new ArgumentNullException(nameof(quiz));
    }

    public void Start()
    {
        _session = new QuizSession
        {
            CurrentState = QuizState.InProgress,
            CurrentQuestionIndex = 0,
            Score = 0
        };
    }

    public bool SubmitAnswer<T>(T answer)
    {
        if (_session.CurrentState != QuizState.InProgress)
            throw new InvalidOperationException("Quiz not in progress");

        var currentQuestion = _quiz.Questions[_session.CurrentQuestionIndex] as IQuestion<T>;
        if (currentQuestion == null)
            throw new InvalidOperationException("Question type mismatch");

        bool isCorrect = currentQuestion.Validate(answer);

        if (isCorrect) _session.Score++;
        _session.MoveNext();

        if (_session.CurrentQuestionIndex >= _quiz.Questions.Count)
            _session.Complete();

        return isCorrect;
    }

    public QuizResult GetResult()
    {
        if (_session.CurrentState != QuizState.Completed)
            throw new InvalidOperationException("Quiz not completed");

        return new QuizResult(_session.Score, _quiz.Questions.Count);
    }
}