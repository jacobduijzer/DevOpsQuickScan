using System.Text;

namespace DevOpsQuickScan.Core;

public class ExportService
{
    public string ExportToCsv(List<Question> questions, List<AnswerSubmission> answers)
    {
        var sb = new StringBuilder();
        sb.AppendLine(
            "QuestionText,Answer 1,Votes,Answer 2, Votes, Answer 3, Votes, Answer 4, Votes, Answer 5, Votes, Total Votes");

        foreach (var question in questions)
        {
            var line = $"{EscapeCsv(question.Text)}";
            foreach (var answer in question.Answers)
            {
                var numberOfVotes = answers.Count(a => a.QuestionId == question.Id &&
                                                       a.AnswerId == answer.Id);
                line += $",{EscapeCsv(answer.Text)},{numberOfVotes}";
            }

            line += $",{answers.Count(q => q.QuestionId == question.Id)}";

            sb.AppendLine(line);
        }

        return sb.ToString();
    }
    
    private static string EscapeCsv(string? value) =>
        value is null ? "" : $"\"{value.Replace("\"", "\"\"")}\"";
    
    
}