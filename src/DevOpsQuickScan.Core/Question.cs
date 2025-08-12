namespace DevOpsQuickScan.Core;

public class Question
{
    public int Id { get; set; }

    public string Text { get; set; }
    
    public string Category { get; set; }

    public string Link { get; set; }

    public List<Answer> Answers { get; set; }
    
    public bool IsRevealed { get; set; } = false;
}
   