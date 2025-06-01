using AngleSharp.Dom;
using Bunit;
using DevOpsQuickScan.Domain;
using DevOpsQuickScan.Web.Components.CustomComponents;

namespace DevOpsQuickScan.UnitTests.Web.Components;

public class ParticipantQuestionComponentTests : TestContext
{
    private QuestionWithAnswers _questionWithAnswers = new QuestionWithAnswers(new Question(1, "What is your favorite color?",
        "https://example.com", [
            new Answer(1, "Red"),
            new Answer(2, "Green"),
            new Answer(3, "Blue")
        ]));
    [Fact]
    public void RendersQuestionCorrectly()
    {
        // ARRANGE
        var component = RenderComponent<ParticipantQuestionComponent>(parameters =>
            parameters.Add(p => p.CurrentQuestion, _questionWithAnswers));
        
        // ACT
        var buttons = component.FindAll("button"); 

        // ASSERT
        Assert.Equal(4, buttons.Count);
        Assert.Equal("Red", buttons[0].InnerHtml);
        Assert.Equal("Green", buttons[1].InnerHtml);
        Assert.Equal("Blue", buttons[2].InnerHtml);
        Assert.True(buttons.Last().IsDisabled());

    }

    [Theory] 
    [InlineData(1, 0, 2)] 
    [InlineData(0, 1, 2)] 
    [InlineData(2, 0, 1)]
    public void ClickSelectsOnlyOneButton(int clickButton, int notSelectedButton1, int notSelectedButton2)
    {
        // ARRANGE
        var component = RenderComponent<ParticipantQuestionComponent>(parameters =>
            parameters.Add(p => p.CurrentQuestion, _questionWithAnswers));
        
        // ACT
        component.FindAll("button")[clickButton].Click(); 

        // ASSERT
        Assert.Contains("active", component.FindAll("button")[clickButton].ClassList);
        Assert.DoesNotContain("active", component.FindAll("button")[notSelectedButton1].ClassList);
        Assert.DoesNotContain("active", component.FindAll("button")[notSelectedButton2].ClassList);
    }
    
    [Theory] 
    [InlineData(0)] 
    [InlineData(1)] 
    [InlineData(2)]
    public void SelectAnswerEnablesSendButton(int clickButton)
    {
        // ARRANGE
        var component = RenderComponent<ParticipantQuestionComponent>(parameters =>
            parameters.Add(p => p.CurrentQuestion, _questionWithAnswers));
        
        // ACT
        component.FindAll("button")[clickButton].Click(); 
        
        // ASSERT
        Assert.False(component.FindAll("button").Last().IsDisabled());
    }
    
    [Theory] 
    [InlineData(0)] 
    [InlineData(1)] 
    [InlineData(2)]
    public void CantSelectNewAnswerAfterSendingTheAnswer(int clickButton)
    {
        // ARRANGE
        var component = RenderComponent<ParticipantQuestionComponent>(parameters =>
            parameters.Add(p => p.CurrentQuestion, _questionWithAnswers));
        
        // ACT
        component.FindAll("button")[clickButton].Click(); 
        component.FindAll("button").Last().Click();
        
        // ASSERT
        Assert.Equal(4, component.FindAll("button").Count(x => x.IsDisabled()));
    }
}