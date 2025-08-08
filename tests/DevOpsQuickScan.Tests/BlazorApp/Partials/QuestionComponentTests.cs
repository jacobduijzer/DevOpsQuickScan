using AngleSharp.Dom;
using Bunit;
using DevOpsQuickScan.BlazorApp.Components.Partials;
using DevOpsQuickScan.Core;

namespace DevOpsQuickScan.Tests.BlazorApp.Partials;

public class QuestionComponentTests : TestContext
{
    // [Fact]
    // public void RendersQuestionCorrectly()
    // {
    //     // ARRANGE
    //     var question = new Question(1, "What is your favorite color?", new List<Answer>
    //     {
    //         new Answer(1, "Red"),
    //         new Answer(2, "Green"),
    //         new Answer(3, "Blue")
    //     });
    //     
    //     // ACT 
    //     var component = RenderComponent<QuestionComponent>(parameters =>
    //     {
    //         parameters.Add(p => p.UserId, Guid.NewGuid().ToString());
    //         parameters.Add(p => p.SessionService, new SessionService());
    //         // parameters.Add(p => p.CurrentQuestion, question);
    //     });
    //
    //     // ASSERT
    //     Assert.Equal("What is your favorite color?", component.Find("strong").TextContent);
    //     
    //     var buttons = component.FindAll("button"); 
    //     Assert.Equal(4, buttons.Count);
    //     Assert.Equal("Red", buttons[0].InnerHtml);
    //     Assert.Equal("Green", buttons[1].InnerHtml);
    //     Assert.Equal("Blue", buttons[2].InnerHtml);
    //     Assert.True(buttons[^1].IsDisabled());
    // }
    
}