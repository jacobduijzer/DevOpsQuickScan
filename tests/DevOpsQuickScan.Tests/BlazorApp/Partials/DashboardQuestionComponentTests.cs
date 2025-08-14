using Bunit;
using DevOpsQuickScan.BlazorApp.Components.Partials;
using DevOpsQuickScan.Core;

namespace DevOpsQuickScan.Tests.BlazorApp.Partials;

public class DashboardQuestionComponentTests : TestContext
{
    [Fact]
    public void ComponentRendersCorrectly()
    {
        // ARRANGE
        var question = new Question()
        {
            Id = 1,
            Text = "What is your favorite color?",
            Category = "Interests",
            Link = "http://link",
            Answers = [
                new Answer(1, "Red"),
                new Answer(2, "Orange"),
                new Answer(3, "Yellow"),
                new Answer(4, "Green"),
                new Answer(5, "Purple")
            ]
        };
        var revealedQuestion = new QuestionWithAnswers(question)
        {
            Answers =
            [
                new RevealedAnswer() { AnswerId = 1, NumberOfVotes = 10 },
                new RevealedAnswer() { AnswerId = 2, NumberOfVotes = 5 }
            ]
        };

        // ACT 
        var component = RenderComponent<DashboardQuestionComponent>(parameters =>
        {
            parameters.Add(p => p.QuestionWithAnswers, revealedQuestion);
        });
        
        // ASSERT
        Assert.Equal("Interests", component.Find("span.me-2").TextContent);
        Assert.Equal("http://link", component.Find("a").GetAttribute("href"));
        Assert.Equal("What is your favorite color?", component.Find("h5.card-title").TextContent);

        var answers = component.FindAll(".list-group .option-item > span");
        Assert.Equal(5, answers.Count);
        Assert.Equal("Red", answers[0].TextContent);
        Assert.Equal("Orange", answers[1].TextContent);
        Assert.Equal("Yellow", answers[2].TextContent);
        Assert.Equal("Green", answers[3].TextContent);
        Assert.Equal("Purple", answers[4].TextContent);
        
        Assert.Equal("15 votes total", component.Find(".card-footer .text-muted").TextContent); 
    }

    [Fact]
    public void RenderingWithoutQuestionWillNotCrash()
    {
        // ACT 
        var component = RenderComponent<DashboardQuestionComponent>(parameters =>
        {
            parameters.Add(p => p.QuestionWithAnswers, null);
        }); 
        
        // ASSERT
        Assert.NotNull(component); 
    }

    [Fact]
    public void RenderingWithoutAnswersWillNotCrash()
    {
        // ARRANGE
        var question = new Question()
        {
            Id = 1,
            Text = "What is your favorite color?",
            Category = "Interests",
            Link = "http://link",
            Answers = [
                new Answer(1, "Red"),
                new Answer(2, "Orange"),
                new Answer(3, "Yellow"),
                new Answer(4, "Green"),
                new Answer(5, "Purple")
            ]
        };
        var revealedQuestion = new QuestionWithAnswers(question);

        // ACT 
        var component = RenderComponent<DashboardQuestionComponent>(parameters =>
        {
            parameters.Add(p => p.QuestionWithAnswers, revealedQuestion);
        }); 
        
        // ASSERT
        Assert.NotNull(component);
    }
}