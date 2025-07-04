using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Api.Application;

public class QuestionType : ObjectType<Question>
{
    protected override void Configure(IObjectTypeDescriptor<Question> descriptor)
    {
    }
}