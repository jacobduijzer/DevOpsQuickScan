namespace DevOpsQuickScan.Domain;

public class SessionService(
    IQuestionRepository questionRepository,
    ISessionRepository sessionRepository)
{
    public async Task<Guid> Start(Guid facilitatorId, string sessionName)
    {
        Session session = new(facilitatorId, sessionName);
        session.Start();
        await sessionRepository.Save(session);
        return session.Id;
    }
    
    private async Task Load(Guid sessionId) =>
        await sessionRepository.Load(sessionId);
}