using DevOpsQuickScan.Domain;
using Microsoft.Extensions.Logging;

namespace DevOpsQuickScan.Application;

public class CreateSessionCommandHandler(
    SessionService sessionService,
    IQuestionsRepository questionses,
    ILogger<CreateSessionCommandHandler> logger)
{
    public record CreateSessionCommand(string SessionName);

    public async Task<string> Handle(CreateSessionCommand command)
    {
        var allQuestions = await questionses.All();
        return await sessionService.Start(command.SessionName, allQuestions!);
    }
}