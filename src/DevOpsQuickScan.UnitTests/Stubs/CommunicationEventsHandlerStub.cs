using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.UnitTests.Stubs;

public class CommunicationEventsHandlerStub : ICommunicationEvents
{
    public event Action<Participant>? OnParticipantJoined;
    public event Action<QuestionWithAnswers>? OnQuestionAsked;

    public Task Start(Guid sessionId, Uri hubUri)
    {
       return Task.CompletedTask;
    }

    public Task Start(Uri hubUri)
    {
        return Task.CompletedTask;
    }

    public Task Join(Guid sessionId, string displayName)
    {
        var participant = new Participant(Guid.NewGuid().ToString(), sessionId, displayName);
        OnParticipantJoined?.Invoke(participant);
        return Task.CompletedTask;
    }

    public Task AskQuestion(Guid sessionId, QuestionWithAnswers questionWithAnswers)
    {
        throw new NotImplementedException();
    }

    public Task AskQuestion(string sessionId, QuestionWithAnswers questionWithAnswers)
    {
        throw new NotImplementedException();
    }

    public async ValueTask DisposeAsync()
    {
        // TODO release managed resources here
    }
}