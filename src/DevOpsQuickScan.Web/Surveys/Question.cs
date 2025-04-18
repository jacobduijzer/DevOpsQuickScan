namespace DevOpsQuickScan.Web.Surveys;

public class Question
{
   public Guid Id { get; set; } 
   public string Text { get; set; }
   public int Order { get; set; }
   
   public List<Answer> Answers { get; set; }
}