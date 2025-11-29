using DevOpsQuickScan.Domain.Questions;

namespace DevOpsQuickScan.Domain.Sessions;

public class Session(string name)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = name;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public List<Question> Questions { get; set; } = [];
}