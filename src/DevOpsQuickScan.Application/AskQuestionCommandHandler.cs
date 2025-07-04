using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Application;

public class AskQuestionCommandHandler(SessionService sessionService)
{
    public record AskQuestionCommand(string SessionCode);

    public async Task<Question> Handle(AskQuestionCommand command)
    {
        await sessionService.Restore(command.SessionCode);
        return await sessionService.AskQuestion();
    }
}