using DevOpsQuickScan.Application;
using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Api.Application;

[ExtendObjectType("queries")]
public class SessionQueries
{
    public string Get(string sessionCode) => sessionCode;

    [GraphQLDescription("Get the previous question.")]
    public async Task<Question?> NextQuestion(string sessionCode, [Service] NextQuestionQueryHandler handler)
    {
        return await handler.Handle(new NextQuestionQueryHandler.NextQuestionQuery(sessionCode));
    }
    
    [GraphQLDescription("Get the next question.")]
    public async Task<Question?> PreviousQuestion(string sessionCode, [Service] PreviousQuestionQueryHandler handler)
    {
        return await handler.Handle(new PreviousQuestionQueryHandler.PreviousQuestionQuery(sessionCode));
    }
}