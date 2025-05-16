namespace DevOpsQuickScan.Domain;

public interface ISessionRepository
{
   Task Save(Session session);

   Task<Session> Load(Guid sessionId);
}