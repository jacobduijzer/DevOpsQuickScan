using DevOpsQuickScan.Web.Surveys;

namespace DevOpsQuickScan.UnitTests;

public class SurveyReaderTests
{
    [Fact]
    public async Task ThrowsWhenFileDoesNotExist()
    {
        // ARRANGE
        SurveyReader surveyReader = new SurveyReader();

        // ACT
        Func<Task> action = () => surveyReader.Read("non-existing-file.json");

        // ASSERT
        await Assert.ThrowsAsync<FileNotFoundException>(action);
    }

    [Fact]
    public async Task CanReadSurvey()
    {
        // ARRANGE
        SurveyReader surveyReader = new();

        // ACT
        var survey = await surveyReader.Read("survey-01.json");

        // ASSERT
        Assert.NotNull(survey);
    }

    [Fact]
    public async Task CanParseSurveyToObjects()
    {
        // ARRANGE
        SurveyReader surveyReader = new();

        // ACT
        Survey survey = await surveyReader.Read("survey-01.json");

        // ASSERT
        Assert.NotNull(survey);
        Assert.Equal(3, survey.Questions.Count);
        Assert.Equal(5, survey.Questions[0].Answers.Count);
    }
}