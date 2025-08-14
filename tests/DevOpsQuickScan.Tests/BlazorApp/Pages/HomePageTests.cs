using Bunit;
using Bunit.Rendering;
using DevOpsQuickScan.BlazorApp.Components.Pages;
using DevOpsQuickScan.BlazorApp.Components.Partials;
using DevOpsQuickScan.Core;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DevOpsQuickScan.Tests.BlazorApp.Pages;

public class HomePageTests : TestContext
{
    [Fact]
    public void LoadHomePage()
    {
        // ARRANGE
        SessionService sessionService = CreateSessionService();
        Mock<IUserIdService> mockUserIdService = CreateMockUserIdService();
        Services.AddSingleton<SessionService>(sessionService);
        Services.AddSingleton<IUserIdService>(mockUserIdService.Object);
        
        // ACT
        var homePage = RenderComponent<Home>();
        
        // ASSERT
        Assert.NotEmpty(sessionService.Participants);
        Assert.Contains("test-user-id", sessionService.Participants);
        Assert.NotNull(homePage.FindComponent<NoQuestionComponent>());
        Assert.Throws<ComponentNotFoundException>(() => homePage.FindComponent<QuestionComponent>());
        Assert.Throws<ComponentNotFoundException>(() => homePage.FindComponent<RevealComponent>());
    }

    [Fact]
    public async Task ShowsQuestionWhenQuestionIsAsked()
    {
        // ARRANGE
        SessionService sessionService = CreateSessionService();
        Mock<IUserIdService> mockUserIdService = CreateMockUserIdService();
        Services.AddSingleton<SessionService>(sessionService);
        Services.AddSingleton<IUserIdService>(mockUserIdService.Object);
        
        await sessionService.Initialize();
        var homePage = RenderComponent<Home>();
        
        // ACT
        sessionService.AskQuestion(1);
        
        // ASSERT
        Assert.NotNull(homePage.FindComponent<QuestionComponent>());
        Assert.Throws<ComponentNotFoundException>(() => homePage.FindComponent<NoQuestionComponent>());
        Assert.Throws<ComponentNotFoundException>(() => homePage.FindComponent<RevealComponent>()); 
    }

    [Fact]
    public async Task ShowsRevealComponentWhenAnswersAreRevealed()
    {
        // ARRANGE
        SessionService sessionService = CreateSessionService();
        Mock<IUserIdService> mockUserIdService = CreateMockUserIdService();
        Services.AddSingleton<SessionService>(sessionService);
        Services.AddSingleton<IUserIdService>(mockUserIdService.Object);
        
        await sessionService.Initialize();
        var homePage = RenderComponent<Home>();
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

    private static Mock<IUserIdService> CreateMockUserIdService()
    {
        var mockUserIdService = new Mock<IUserIdService>();
        mockUserIdService
            .Setup(m => m.GetAsync())
            .ReturnsAsync("test-user-id");

        return mockUserIdService;
    }
}