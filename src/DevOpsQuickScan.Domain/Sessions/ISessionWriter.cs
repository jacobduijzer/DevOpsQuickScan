namespace DevOpsQuickScan.Domain.Sessions;

public interface ISessionWriter
{
    Task Write(Session session);
}