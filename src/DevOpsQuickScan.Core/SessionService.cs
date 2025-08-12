using System.Data;

namespace DevOpsQuickScan.Core;

public class SessionService(QuestionsService questions)
{
    public event Action<RevealedQuestion>? OnAnswerReceived;
    public event Action<SessionState, Question>? OnQuestionAsked;
    public event Action<SessionState, RevealedQuestion>? OnAnswersRevealed;

    public SessionState CurrentState { get; private set; }
    public Question? CurrentQuestion { get; private set; }

    public List<Question> Questions => questions.All;

    private List<AnswerSubmission> _submissions = [];

    public void AskQuestion(int questionId)
    {
        if (CurrentQuestion?.Id == questionId)
            return;

        CurrentQuestion = questions.All.First(x => x.Id == questionId);
        CurrentState = SessionState.AnsweringQuestions;
        OnQuestionAsked?.Invoke(CurrentState, CurrentQuestion);
    }

    public void AnswerQuestion(string userId, int questionId, int answerId)
    {
        if (CurrentQuestion!.IsRevealed || HasAnsweredCurrentQuestion(userId, questionId))
            return;

        _submissions.Add(new AnswerSubmission(userId, questionId, answerId));
        var revealedQuestion = RevealedQuestion(questionId);
        OnAnswerReceived?.Invoke(revealedQuestion);
    }

    private bool HasAnsweredCurrentQuestion(string userId, int questionId) =>
        _submissions.Any(x => x.UserId == userId && x.QuestionId == questionId);

    public int? GetAnswer(string userId, int questionId) =>
        _submissions.FirstOrDefault(x => x.UserId == userId && x.QuestionId == questionId)?.AnswerId;

    public void RevealQuestion(int questionId)
    {
        questions.RevealQuestion(questionId);
        
        var revealedQuestion = RevealedQuestion(questionId);

        CurrentState = SessionState.RevealingAnswers;
        OnAnswersRevealed?.Invoke(CurrentState, revealedQuestion);
    }

    public RevealedQuestion RevealedQuestion(int questionId)
    {
        var question = questions.All.First(x => x.Id == questionId);
        var revealedQuestion = new RevealedQuestion
        {
            Question = question.Text,
            Link = question.Link,
            Answers = question.Answers.Select(answer => new RevealedAnswer
            {
                Text = answer.Text,
                NumberOfVotes = NumberOfAnswers(questionId, answer.Id)
            }).ToList()
        };
        return revealedQuestion;
    }

    private int NumberOfAnswers(int questionId, int answerId) =>
        _submissions.Count(submission => submission.QuestionId == questionId && submission.AnswerId == answerId);

    public void ResetQuestion(int questionId)
    {
        questions.ResetQuestion(questionId);
        _submissions.RemoveAll(x => x.QuestionId == questionId);
    }
}