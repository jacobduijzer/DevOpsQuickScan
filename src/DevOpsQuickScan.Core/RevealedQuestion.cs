namespace DevOpsQuickScan.Core;

public class RevealedQuestion
{
    public string Question { get; set; }
    public string Link { get; set; }
    public List<RevealedAnswer> Answers { get; set; }
}