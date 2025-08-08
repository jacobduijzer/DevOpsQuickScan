namespace DevOpsQuickScan.Core;

public class SessionService(QuestionsService questions)
{
    public event Action? OnAnswerReceived;
    public event Action<SessionState, int>? OnSessionStateChanged;

    public SessionState CurrentState { get; private set; }
    public Question? CurrentQuestion { get; private set; }

    public List<Question> Questions => questions.All;

    private List<AnswerSubmission> _submissions = [];

    public void AskQuestion(int questionId)
    {
        if (CurrentQuestion?.Id == questionId)
            return;

        CurrentQuestion = questions.All.FirstOrDefault(x => x.Id == questionId);
        CurrentState = SessionState.AnsweringQuestions;
        OnSessionStateChanged?.Invoke(CurrentState, questionId);
    }


    public void AnswerQuestion(string userId, int questionId, int answerId)
    {
        if (CurrentQuestion!.IsRevealed || HasAnsweredCurrentQuestion(userId, questionId))
            return;

        _submissions.Add(new AnswerSubmission(userId, questionId, answerId));
        OnAnswerReceived?.Invoke();
    }

    public bool HasAnsweredCurrentQuestion(string userId, int questionId) =>
        _submissions.Any(x => x.UserId == userId && x.QuestionId == questionId);

    public int GetAnswer(string userId, int questionId) =>
        (int)_submissions.FirstOrDefault(x => x.UserId == userId && x.QuestionId == questionId)?.AnswerId!;

    public void RevealQuestion(int questionId)
    {
        questions.RevealQuestion(questionId);
        CurrentState = SessionState.RevealingAnswers;
        OnSessionStateChanged?.Invoke(CurrentState, questionId);
    }
    
    public int NumberOfAnswers(int questionId, int answerId) =>
        _submissions.Count(submission => submission.QuestionId == questionId && submission.AnswerId == answerId);
    
    // public RevealDto RevealData(int questionId)
    // {
    //     var question = questions.All.FirstOrDefault(x => x.Id == questionId);
    //     if (question is null || !question.IsRevealed)
    //         return new RevealDto(); // throw?
    //
    //     return new RevealDto
    //     {
    //         QuestionText = question.Text,
    //         Answers = question.Answers
    //             .Select(answer => new AnswerDto
    //             {
    //                 Text = answer.Text,
    //                 NumberOfVotes = _submissions.Count(submission =>
    //                     submission.QuestionId == questionId && submission.AnswerId == answer.Id)
    //             }).ToList()
    //     };
    // }

    public void ResetQuestion(int questionId)
    {
        questions.ResetQuestion(questionId);
        _submissions.RemoveAll(x => x.QuestionId == questionId);
    }
}