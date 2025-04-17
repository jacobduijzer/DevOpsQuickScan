namespace DevOpsQuickScan.Domain.Old;

public class SessionService : ISessionService
{
    public string Name { get; private set; }
    
    public void Start(string name) => Name = name;
    
    public Participant Add(string name)
    {
       Participant participant = new (Guid.NewGuid().ToString(), name);
       return participant;
    }
}