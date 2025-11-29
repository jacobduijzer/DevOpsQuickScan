namespace DevOpsQuickScan.Domain.Sessions;

public class SessionService(IQuestionReader questionReader, ISessionReader sessionReader, ISessionWriter sessionWriter)
{
    public async Task<Guid> Create(string sessionName, string email)
    {
        var questions = await questionReader.Read();
        
        Session session = new (sessionName)
        {
            Questions = questions
        };

        await sessionWriter.Write(session);
        
        return session.Id;
    }

    public void Open(string sessionId)
    {
        throw new NotImplementedException();
    }
    
}