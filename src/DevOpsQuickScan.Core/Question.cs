namespace DevOpsQuickScan.Core;

public class Question(int id, string text, List<Answer> answers)
{
    public int Id => id;

    public string Text = text;

    public List<Answer> Answers = answers;
    
    public bool IsRevealed { get; set; } = false;
}
   