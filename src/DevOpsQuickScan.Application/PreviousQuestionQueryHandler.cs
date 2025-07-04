using DevOpsQuickScan.Domain;
using Microsoft.Extensions.Logging;

namespace DevOpsQuickScan.Application;

public class PreviousQuestionQueryHandler(SessionService sessionService, ILogger<PreviousQuestionQueryHandler> logger)
{
    public record PreviousQuestionQuery(string SessionCode);

    public async Task<Question?> Handle(PreviousQuestionQuery query)
    {
        await sessionService.Restore(query.SessionCode);
        return await sessionService.PreviousQuestion();
    }
}