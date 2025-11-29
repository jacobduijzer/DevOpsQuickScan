using AngleSharp.Dom;
using Bunit;
using DevOpsQuickScan.BlazorApp.Components.Partials;
using DevOpsQuickScan.Core;
using DevOpsQuickScan.Domain.Questions;
using Microsoft.AspNetCore.Components;

namespace DevOpsQuickScan.Tests.BlazorApp.Partials;

public class Question_Component_Should : TestContext
{
    [Fact]
    public void Render_Question_Correctly()
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

        // ACT 
        var component = Render<QuestionComponent>(parameters =>
        {
            parameters.Add(p => p.Question, question);
            parameters.Add(p => p.PreviousAnswer, null);
            parameters.Add(p => p.OnQuestionAnswered,
                EventCallback.Factory.Create<QuestionAnswered>(this, _ => { }));
        });

        // ASSERT
        Assert.Equal("Interests", component.Find("span.me-2").TextContent);
        Assert.Equal("http://link", component.Find("a").GetAttribute("href"));
        Assert.Equal("What is your favorite color?", component.Find("h5.card-title").TextContent);

        var inputs = component.FindAll("input");
        Assert.Equal(5, inputs.Count);
        Assert.Equal("1", inputs[0].GetAttribute("value"));
        Assert.Equal("2", inputs[1].GetAttribute("value"));
        Assert.Equal("3", inputs[2].GetAttribute("value"));
        Assert.Equal("4", inputs[3].GetAttribute("value"));
        Assert.Equal("5", inputs[4].GetAttribute("value"));

        Assert.False(inputs[0].IsChecked());
        Assert.False(inputs[1].IsChecked());
        Assert.False(inputs[2].IsChecked());
        Assert.False(inputs[3].IsChecked());
        Assert.False(inputs[4].IsChecked());

        var labels = component.FindAll("span.flex-grow-1");
        Assert.Equal(5, labels.Count);
        Assert.Equal("Red", labels[0].TextContent);
        Assert.Equal("Orange", labels[1].TextContent);
        Assert.Equal("Yellow", labels[2].TextContent);
        Assert.Equal("Green", labels[3].TextContent);
        Assert.Equal("Purple", labels[4].TextContent);

        var button = component.Find("button");
        Assert.True(button.IsDisabled());
    }

    [Fact]
    public void Render_Correctly_Without_Question()
    {
        // ACT
        var component = Render<QuestionComponent>(parameters =>
        {
            parameters.Add(p => p.Question, null);
            parameters.Add(p => p.PreviousAnswer, null);
            parameters.Add(p => p.OnQuestionAnswered,
                EventCallback.Factory.Create<QuestionAnswered>(this, _ => { }));
        });

        // ASSERT
        Assert.NotNull(component);
    }

    [Fact]
    public void Enable_Button_When_Answer_Is_Selected()
    {
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

        var component = Render<QuestionComponent>(parameters =>
        {
            parameters.Add(p => p.Question, question);
            parameters.Add(p => p.PreviousAnswer, null);
            parameters.Add(p => p.OnQuestionAnswered,
                EventCallback.Factory.Create<QuestionAnswered>(this, _ => { }));
        });

        // ACT
        component.Find("input[value='3']").Click();

        // ASSERT
        Assert.True(component.Find("input[value='3']").IsChecked());
        Assert.False(component.Find("button").IsDisabled());
    }

    [Fact]
    public void Submit_Answers()
    {
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

        QuestionAnswered? answeredQuestion = null;

        var component = Render<QuestionComponent>(parameters =>
        {
            parameters.Add(p => p.Question, question);
            parameters.Add(p => p.PreviousAnswer, null);
            parameters.Add(p => p.OnQuestionAnswered,
                EventCallback.Factory.Create<QuestionAnswered>(this, result => { answeredQuestion = result; }));
        });
        var input = component.Find("input[value='3']");
        var button = component.Find("button");

        // ACT 
        input.Click();
        button.Click();
        component.Render();

        // ASSERT
        Assert.NotNull(answeredQuestion);
        Assert.Equal(1, answeredQuestion.QuestionId);
        Assert.Equal(3, answeredQuestion.AnswerId);
        Assert.True(button.IsDisabled());
    }

    [Fact]
    public void Load_With_Preselected_Answer_And_Disabled_Button()
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

        // ACT 
        var component = Render<QuestionComponent>(parameters =>
        {
            parameters.Add(p => p.Question, question);
            parameters.Add(p => p.PreviousAnswer, 3);
            parameters.Add(p => p.OnQuestionAnswered,
                EventCallback.Factory.Create<QuestionAnswered>(this, result => { }));
        });

        // ASSERT
        Assert.True(component.Find("input[value='3']").IsChecked());
        Assert.Throws<ElementNotFoundException>(() => component.Find("button"));
        Assert.Equal("Answer submitted, waiting for results…", component.Find(".card-footer span > em").TextContent);
    }
}