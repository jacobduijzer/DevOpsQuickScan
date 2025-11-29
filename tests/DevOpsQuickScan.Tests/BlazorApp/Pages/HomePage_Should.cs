using Bunit;
using Bunit.Rendering;
using DevOpsQuickScan.BlazorApp.Components.Pages;
using DevOpsQuickScan.BlazorApp.Components.Partials;
using DevOpsQuickScan.Core;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace DevOpsQuickScan.Tests.BlazorApp.Pages;

public class HomePageShould : BunitContext 
{
    [Fact]
    public void Load()
    {
        // ARRANGE
        SessionService sessionService = CreateSessionService();
        var userIdService = CreateSubstituteUserIdService();
        Services.AddSingleton<SessionService>(sessionService);
        Services.AddSingleton<IUserIdService>(userIdService);
        
        // ACT
        var homePage = Render<Home>();
        
        // ASSERT
        Assert.NotEmpty(sessionService.Participants);
        Assert.Contains("test-user-id", sessionService.Participants);
        Assert.NotNull(homePage.FindComponent<NoQuestionComponent>());
        Assert.Throws<ComponentNotFoundException>(() => homePage.FindComponent<QuestionComponent>());
        Assert.Throws<ComponentNotFoundException>(() => homePage.FindComponent<RevealComponent>());
    }

    [Fact]
    public async Task Show_Question_When_Question_Is_Asked()
    {
        // ARRANGE
        SessionService sessionService = CreateSessionService();
        var userIdService = CreateSubstituteUserIdService();
        Services.AddSingleton<SessionService>(sessionService);
        Services.AddSingleton<IUserIdService>(userIdService);
        
        await sessionService.Initialize();
        var homePage = Render<Home>();
        
        // ACT
        sessionService.AskQuestion(1);
        
        // ASSERT
        Assert.NotNull(homePage.FindComponent<QuestionComponent>());
        Assert.Throws<ComponentNotFoundException>(() => homePage.FindComponent<NoQuestionComponent>());
        Assert.Throws<ComponentNotFoundException>(() => homePage.FindComponent<RevealComponent>()); 
    }

    [Fact]
    public async Task Reveal_Answers_When_Question_Is_Revealed()
    {
        // ARRANGE
        SessionService sessionService = CreateSessionService();
        var userIdService = CreateSubstituteUserIdService();
        Services.AddSingleton<SessionService>(sessionService);
        Services.AddSingleton<IUserIdService>(userIdService);
        
        await sessionService.Initialize();
        var homePage = Render<Home>();
        sessionService.AskQuestion(1);
        
        // ACT
        sessionService.RevealQuestion(1);
        
        // ASSERT
        Assert.NotNull(homePage.FindComponent<RevealComponent>());
        Assert.Throws<ComponentNotFoundException>(() => homePage.FindComponent<NoQuestionComponent>());
        Assert.Throws<ComponentNotFoundException>(() => homePage.FindComponent<QuestionComponent>());  
    }

    private static SessionService CreateSessionService() =>
        new (new QuestionsService("Core"), new ExportService());

    private static IUserIdService CreateSubstituteUserIdService()
    {
        var userIdService = Substitute.For<IUserIdService>();
        userIdService.GetAsync().Returns("test-user-id");
        return userIdService;
    }
}