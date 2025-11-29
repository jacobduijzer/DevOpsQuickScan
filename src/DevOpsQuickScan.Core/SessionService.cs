using System.Text;
using DevOpsQuickScan.Domain.Questions;

namespace DevOpsQuickScan.Core;

public class SessionService(QuestionsService questions, ExportService exports)
{
    public event Action<QuestionWithAnswers>? OnAnswerReceived;
    public event Action<SessionState, Question?>? OnQuestionAsked;
    public event Action<SessionState, QuestionWithAnswers>? OnAnswersRevealed;
    public event Action? OnParticipantJoined;
    public SessionState CurrentState { get; private set; }
    public Question? CurrentQuestion { get; private set; }
    public List<Question> Questions { get; private set; } = [];

    private readonly List<AnswerSubmission> _submissions = [];

    public List<string> Participants { get; } = [];

    public async Task Initialize() =>
        Questions = await questions.Load();

    public void Join(string participantId)
    {
        if (Participants.Contains(participantId))
            return;

        Participants.Add(participantId);
        OnParticipantJoined?.Invoke();
    }

    public void Remove(string participantId)
    {
        if (!Participants.Contains(participantId))
            return;

        Participants.Remove(participantId);
        OnParticipantJoined?.Invoke();
    }

    public void AskQuestion(int questionId)
    {
        if (CurrentQuestion?.Id == questionId)
            return;

        CurrentQuestion = Questions.First(x => x.Id == questionId);
        CurrentState = SessionState.AnsweringQuestions;
        OnQuestionAsked?.Invoke(CurrentState, CurrentQuestion);
    }

    public void AnswerQuestion(string participantId, int questionId, int answerId)
    {
        if (CurrentQuestion!.IsRevealed || HasAnsweredCurrentQuestion(participantId, questionId))
            return;

        _submissions.Add(new AnswerSubmission(participantId, questionId, answerId));
        var questionWithAnswers = QuestionWithAnswers(questionId);
        OnAnswerReceived?.Invoke(questionWithAnswers);
    }

    private bool HasAnsweredCurrentQuestion(string participantId, int questionId) =>
        _submissions.Any(x => x.ParticipantId == participantId && x.QuestionId == questionId);

    public int? GetAnswerId(string participantId, int questionId) =>
        _submissions.FirstOrDefault(x => x.ParticipantId == participantId && x.QuestionId == questionId)?.AnswerId;

    public void RevealQuestion(int questionId)
    {
        // TODO: Check if question is already revealed
        // Currently not possible, we are not keeping track of revealed questions
        var question = Questions.First(q => q.Id == questionId);

        question.IsRevealed = true;

        var questionWithAnswers = QuestionWithAnswers(questionId);

        CurrentState = SessionState.RevealingAnswers;
        OnAnswersRevealed?.Invoke(CurrentState, questionWithAnswers);
    }

    public QuestionWithAnswers QuestionWithAnswers(int questionId)
    {
        var question = Questions.First(x => x.Id == questionId);
        var revealedQuestion = new QuestionWithAnswers(question)
        {
            Answers = question.Answers.Select(answer => new RevealedAnswer
            {
                AnswerId = answer.Id,
                NumberOfVotes = NumberOfAnswers(questionId, answer.Id)
            }).ToList()
        };
        return revealedQuestion;
    }

    private int NumberOfAnswers(int questionId, int answerId) =>
        _submissions.Count(submission => submission.QuestionId == questionId && submission.AnswerId == answerId);

    public void ResetQuestion(int questionId)
    {
        var question = Questions.First(q => q.Id == questionId);
        question.IsRevealed = false;
        _submissions.RemoveAll(x => x.QuestionId == questionId);

        if (CurrentQuestion?.Id != questionId) return;

        CurrentState = SessionState.NotStarted;
        CurrentQuestion = null;
        OnQuestionAsked?.Invoke(CurrentState, null);
    }

    public string ExportSessionReportCsv() =>
        exports.ExportToCsv(Questions, _submissions);

    public void Reset()
    {
        CurrentState = SessionState.NotStarted;
        CurrentQuestion = null;
        _submissions.Clear();
        Participants.Clear();
        OnQuestionAsked?.Invoke(CurrentState, null);
    }
}