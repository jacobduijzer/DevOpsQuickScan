namespace DevOpsQuickScan.Web.Surveys;

public class Survey
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Active { get; set; }
    
    public List<Question> Questions { get; set; }
}