namespace DevOpsQuickScan.Domain.Questions;

public class Question
{
    public required int Id { get; set; }

    public required string Text { get; set; }
    
    public required string Category { get; set; }

    public required string Link { get; set; }

    public required List<Answer> Answers { get; set; }
    
    public bool IsRevealed { get; set; } = false;
}
   