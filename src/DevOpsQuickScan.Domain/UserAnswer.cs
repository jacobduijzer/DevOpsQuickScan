namespace DevOpsQuickScan.Domain;

public class UserAnswer
{
    public Guid SessionId { get; set; }
    public Guid UserId { get; set; }
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is UserAnswer other &&
               SessionId == other.SessionId &&
               UserId == other.UserId &&
               QuestionId == other.QuestionId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SessionId, UserId, QuestionId);
    }
}