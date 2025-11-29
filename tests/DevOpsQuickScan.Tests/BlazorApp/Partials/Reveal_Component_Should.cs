using Bunit;
using DevOpsQuickScan.BlazorApp.Components.Partials;
using DevOpsQuickScan.Core;
using DevOpsQuickScan.Domain.Questions;

namespace DevOpsQuickScan.Tests.BlazorApp.Partials;

public class Reveal_Component_Should : TestContext
{
    [Fact]
    public void Render_Component_Correctly()
    {
        // ARRANGE
        var question = new Question()
        {
            Id = 1,
            Text = "What is your favorite color?",
            Category = "Interests",
            Link = "http://link",
            Answers =
            [
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
        var component = Render<RevealComponent>(parameters =>
        {
            parameters.Add(p => p.QuestionWithAnswers, revealedQuestion);
        });
    
        // ASSERT
        Assert.Equal("Interests", component.Find("span.me-2").TextContent);
        Assert.Equal("http://link", component.Find("a").GetAttribute("href"));
        Assert.Equal("What is your favorite color?", component.Find("h5.card-title").TextContent);
        
        var answers = component.FindAll(".mb-3 > .d-flex.justify-content-between.mb-1");
        Assert.Equal(5, answers.Count);
        Assert.Equal("Red", answers[0].FirstChild!.TextContent);
        Assert.Equal("66%", answers[0].LastChild!.TextContent);
        Assert.Equal("Orange", answers[1].FirstChild!.TextContent);
        Assert.Equal("33%", answers[1].LastChild!.TextContent);
        Assert.Equal("Yellow", answers[2].FirstChild!.TextContent);
        Assert.Equal("0%", answers[2].LastChild!.TextContent);
        Assert.Equal("Green", answers[3].FirstChild!.TextContent);
        Assert.Equal("0%", answers[3].LastChild!.TextContent);
        Assert.Equal("Purple", answers[4].FirstChild!.TextContent);
        Assert.Equal("0%", answers[4].LastChild!.TextContent);
        
        Assert.Equal("15 votes total", component.Find(".card-footer .text-muted").TextContent); 
    }
    
    [Fact]
    public void Render_Component_Without_Question()
    {
        // ACT 
        var component = Render<RevealComponent>(parameters =>
        {
            parameters.Add(p => p.QuestionWithAnswers, null);
        });
        
        // ARRANGE
        Assert.NotNull(component);
    }
}