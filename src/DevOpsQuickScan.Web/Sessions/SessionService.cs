using DevOpsQuickScan.Domain;
using DevOpsQuickScan.Web.Surveys;

namespace DevOpsQuickScan.Web.Sessions;

public class SessionService(
    IHubConnectionWrapper hubConnectionWrapper, 
    ISessionStore sessionStore,
    ISurveyReader surveyReader) 
{
    public event Action<Participant>? OnParticipantJoined;
    public event Action<ParticipantAnswer>? OnParticipantAnswered;
    
    public Question? CurrentQuestion => Survey?.Questions.ElementAtOrDefault(CurrentQuestionIndex);
    public HashSet<Participant> Participants { get; } = new HashSet<Participant>();
    public HashSet<ParticipantAnswer> Answers { get; } = new HashSet<ParticipantAnswer>();
    public Survey? Survey;
    public int CurrentQuestionIndex = 0;
    private string? _sessionId;

    public async Task<string> Start(string sessionName, string hubUrl, string surveyFile)
    {
        _sessionId = sessionStore.CreateSession(sessionName);
        Survey = await surveyReader.Read(surveyFile);
        await hubConnectionWrapper.Start(_sessionId, hubUrl);
        await hubConnectionWrapper.JoinSession(_sessionId, "Facilitator");

        hubConnectionWrapper.OnParticipantJoined += ParticipantJoined;
        hubConnectionWrapper.OnNewAnswer += ParticipantAnsweredQuestion;

        return _sessionId;
    }

    public Question NextQuestion()
    {
        if (Survey?.Questions == null || CurrentQuestionIndex >= Survey.Questions.Count - 1) 
            return default;
        
        CurrentQuestionIndex++;
        return CurrentQuestion!;
    }
    
    public Question PreviousQuestion()
    {
        if (Survey?.Questions == null || CurrentQuestionIndex <= 0) 
            return default;
        
        CurrentQuestionIndex--;
        return CurrentQuestion!;
    }

    public async Task SendCurrentQuestion()
    {
        await hubConnectionWrapper.SendQuestion(_sessionId!, CurrentQuestion!);
    }
    
    private void ParticipantJoined(Participant participant)
    {
        if (!Participants.Add(participant))
            return;

        OnParticipantJoined?.Invoke(participant);
    }

    private void ParticipantAnsweredQuestion(ParticipantAnswer participantAnswer)
    {
        if (!Answers.Add(participantAnswer))
            return;

        OnParticipantAnswered?.Invoke(participantAnswer);
    }
}