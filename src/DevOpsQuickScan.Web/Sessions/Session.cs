namespace DevOpsQuickScan.Web.Sessions;

public class Session
{
   public string Id { get; set; }
   public HashSet<Participant> Participants { get; set; } = new();
}