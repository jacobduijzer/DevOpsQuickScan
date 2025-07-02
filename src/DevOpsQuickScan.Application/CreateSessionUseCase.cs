using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Application;

public class CreateSessionUseCase(
    SessionService sessionService,
    IQuestionsRepository questionses)
{
    public record CreateSessionCommand(string SessionName);

    public async Task<string> Handle(CreateSessionCommand command)
    {
        var allQuestions = await questionses.All();
        return await sessionService.Start(command.SessionName, allQuestions!);
    }
}