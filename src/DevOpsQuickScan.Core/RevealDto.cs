namespace DevOpsQuickScan.Core;

public class RevealDto
{
   public string QuestionText { get; set; } = string.Empty;

   public List<AnswerDto> Answers { get; set; } = [];
}

public class AnswerDto
{
   public string Text { get; set; } = string.Empty;
   public int NumberOfVotes { get; set; } = 0;
}