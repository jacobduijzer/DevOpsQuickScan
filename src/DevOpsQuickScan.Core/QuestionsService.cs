namespace DevOpsQuickScan.Core;

public class QuestionsService
{
    public List<Question> All => _questions;
    
    public void RevealQuestion(int questionId)
    {
        var question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is not null)
            question.IsRevealed = true;
    }
    
    public void ResetQuestion(int questionId)
    {
        var question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is not null)
            question.IsRevealed = false;
    }

    private List<Question> _questions =
    [
        new Question(1, 
            "How does your team ensure that code remains maintainable over time?", 
            "Capabilities that enable a Climate for Learning",
            "https://dora.dev/capabilities/code-maintainability/", [
            new Answer(1, "We don’t have a defined approach"),
            new Answer(2, "We use basic linting and formatting tools"),
            new Answer(3, "We follow code review and documentation guidelines"),
            new Answer(4, "We apply automated maintainability checks and refactoring practices"),
            new Answer(5, "We have a culture of continuous refactoring and evolutionary architecture")
        ]),
        new Question(2, 
            "How does your team ensure that documentation remains accurate, useful, and up to date?", 
            "Capabilities that enable Fast Flow",
            "https://dora.dev/capabilities/documentation-quality/", [
            new Answer(1, "We don’t have much documentation"),
            new Answer(2, "We document as needed, but it’s inconsistent"),
            new Answer(3, "We have clear documentation practices"),
            new Answer(4, "We use automated tools and structured processes"),
            new Answer(5, "We treat documentation as a first-class citizen")
        ]),
        new Question(3, 
            "How empowered is your team to choose the tools and technologies you use?", 
            "Capabilities that enable Fast Feedback",
            "https://dora.dev/capabilities/teams-empowered-to-choose-tools/", [
            new Answer(1, "We have no say in tool selection"),
            new Answer(2, "We can suggest tools, but decisions are made elsewhere"),
            new Answer(3, "We can suggest tools, but decisions are made elsewhere"),
            new Answer(4, "We have significant influence over our tools"),
            new Answer(5, "We fully own our tooling decisions")
        ]),
    ];
}