namespace DevOpsQuickScan.Core;

public class Question(int id, string text, string category, string link, List<Answer> answers)
{
    public int Id => id;

    public string Text = text;
    
    public string Category = category;

    public string Link = link;

    public List<Answer> Answers = answers;
    
    public bool IsRevealed { get; set; } = false;
}
   