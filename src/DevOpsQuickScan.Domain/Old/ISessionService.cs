namespace DevOpsQuickScan.Domain.Old;

public interface ISessionService
{
   void Start(string name);

   Participant Add(string name);
}