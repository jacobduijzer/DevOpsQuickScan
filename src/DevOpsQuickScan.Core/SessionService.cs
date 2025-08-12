using System.Text;

namespace DevOpsQuickScan.Core;

public class SessionService(QuestionsService questions)
{
    public event Action<RevealedQuestion>? OnAnswerReceived;
    public event Action<SessionState, Question>? OnQuestionAsked;
    public event Action<SessionState, RevealedQuestion>? OnAnswersRevealed;
    public event Action? OnParticipantJoined;
    public SessionState CurrentState { get; private set; }
    public Question? CurrentQuestion { get; private set; }
    public List<Question> Questions { get; private set; } = [];

    private readonly List<AnswerSubmission> _submissions = [];

    public List<string> Participants { get; private set; } = [];

    public async Task Initialize() =>
        Questions = await questions.Load();
    
    public void Join(string userId) 
    {
        if (Participants.Contains(userId))
            return;

        Participants.Add(userId);
        OnParticipantJoined?.Invoke();
    }
    
    public void Remove(string userId) 
    {
        if (!Participants.Contains(userId))
            return;

        Participants.Remove(userId);
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
        var question = Questions.FirstOrDefault(q => q.Id == questionId);
        if (question is not null)
            question.IsRevealed = true;
        
        var revealedQuestion = RevealedQuestion(questionId);

        CurrentState = SessionState.RevealingAnswers;
        OnAnswersRevealed?.Invoke(CurrentState, revealedQuestion);
    }

    public RevealedQuestion RevealedQuestion(int questionId)
    {
        var question = Questions.First(x => x.Id == questionId);
        var revealedQuestion = new RevealedQuestion(question)
        {
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
        var question = Questions.FirstOrDefault(q => q.Id == questionId);
        if (question is not null)
            question.IsRevealed = false;
        
        CurrentQuestion = null;
        _submissions.RemoveAll(x => x.QuestionId == questionId);
    }
    
    public string ExportSessionReportCsv()
    {
        var sb = new StringBuilder();
        sb.AppendLine("QuestionText,Answer 1,Votes,Answer 2, Votes, Answer 3, Votes, Answer 4, Votes, Answer 5, Votes, Total Votes");
       
        foreach (var question in Questions)
        {
            var line = $"{EscapeCsv(question.Text)}";
            foreach (var answer in question.Answers)
            {
                var numberOfVotes = NumberOfAnswers(question.Id, answer.Id);
                line += $",{EscapeCsv(answer.Text)},{numberOfVotes}";
            }

            line += $",{_submissions.Count(q => q.QuestionId == question.Id)}";

            sb.AppendLine(line);
        }

        return sb.ToString();
    }

    private string EscapeCsv(string? value) =>
        value is null ? "" : $"\"{value.Replace("\"", "\"\"")}\"";

    public void Reset()
    {
        CurrentState = SessionState.NotStarted;
        CurrentQuestion = null;
        _submissions.Clear();
        Participants.Clear();
        OnQuestionAsked?.Invoke(CurrentState, null);
    }
}