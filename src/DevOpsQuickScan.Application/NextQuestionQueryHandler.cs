using DevOpsQuickScan.Domain;
using Microsoft.Extensions.Logging;

namespace DevOpsQuickScan.Application;

public class NextQuestionQueryHandler(SessionService sessionService, ILogger<NextQuestionQueryHandler> logger)
{
    public record NextQuestionQuery(string SessionCode);

    public async Task<Question?> Handle(NextQuestionQuery query)
    {
        await sessionService.Restore(query.SessionCode);
        return await sessionService.NextQuestion();
    }
}