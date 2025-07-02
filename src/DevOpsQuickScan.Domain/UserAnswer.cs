namespace DevOpsQuickScan.Domain;

public class UserAnswer
{
    public string SessionCode { get; set; }
    public Guid UserId { get; set; }
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is UserAnswer other &&
               SessionCode == other.SessionCode &&
               UserId == other.UserId &&
               QuestionId == other.QuestionId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SessionCode, UserId, QuestionId);
    }
}