using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Web.Surveys;

public interface ISurveyReader
{
    Task<Survey> Read(string fileName);
}