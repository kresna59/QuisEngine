using Microsoft.AspNetCore.Mvc;
using QuizEngine.Core;
using QuizEngine.Core.Models;

namespace QuizEngine.Api.Controllers;

[ApiController]
[Route("api/quiz")]
public class QuizController : ControllerBase
{
    private QuizEngineCore? _engine;
    private readonly ILogger<QuizController> _logger;

    public QuizController(ILogger<QuizController> logger)
    {
        _logger = logger;
    }

    [HttpPost("start")]
    public IActionResult Start([FromBody] Quiz quiz)
    {
        try
        {
            _engine = new QuizEngineCore(quiz);
            _engine.Start();

            _logger.LogInformation("Quiz started with {QuestionCount} questions", quiz.Questions.Count);

            return Ok(new
            {
                Message = "Quiz started successfully",
                FirstQuestion = quiz.Questions.FirstOrDefault()?.Text,
                TotalQuestions = quiz.Questions.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start quiz");
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("answer")]
    public IActionResult SubmitAnswer([FromBody] AnswerRequest request)
    {
        if (_engine is null)
        {
            return BadRequest(new { Error = "Quiz not started. Call /start first." });
        }

        try
        {
            bool isCorrect = request.AnswerType switch
            {
                "bool" => _engine.SubmitAnswer(bool.Parse(request.Value)),
                "string" => _engine.SubmitAnswer(request.Value),
                _ => throw new ArgumentException($"Unsupported answer type: {request.AnswerType}")
            };

            return Ok(new
            {
                IsCorrect = isCorrect,
                Message = isCorrect ? "Correct answer!" : "Incorrect answer"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing answer");
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("result")]
    public ActionResult<QuizResult> GetResult()
    {
        if (_engine is null)
        {
            return BadRequest(new { Error = "Quiz not started. Call /start first." });
        }

        try
        {
            var result = _engine.GetResult();
            _logger.LogInformation("Quiz completed with score: {Score}/{Total}",
                result.Score, result.TotalQuestions);

            return result;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Attempt to get result before quiz completion");
            return BadRequest(new { Error = ex.Message });
        }
    }
}

public class AnswerRequest
{
    public required string AnswerType { get; set; }
    public required string Value { get; set; }
}