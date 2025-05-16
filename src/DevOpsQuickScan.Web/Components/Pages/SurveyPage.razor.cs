using DevOpsQuickScan.Domain;
using DevOpsQuickScan.Web.Sessions;
using DevOpsQuickScan.Web.Surveys;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Question = DevOpsQuickScan.Web.Surveys.Question;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class SurveyPage : ComponentBase
{
    [Parameter] public string SessionName { get; set; } = string.Empty;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Inject] private IQuestionRepository QuestionRepository { get; set; } = default!;
    [Inject] private CurrentSessionService CurrentSessionService { get; set; } = default!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;


    private string _inviteLink;
    private List<Participant> _participants = new();
    private HashSet<ParticipantAnswer> _votes = new();

    private Question? _currentQuestion;

    protected override async Task OnInitializedAsync()
    {
        var testData = await QuestionRepository.QuestionData();
        
        var sessionId = await CurrentSessionService.Start(
            SessionName,
            NavigationManager.ToAbsoluteUri("/hub/voting").ToString(),
            Path.Combine("Surveys", "survey-01.json"));

        _inviteLink = NavigationManager.BaseUri + $"join?session={sessionId}";
        
        CurrentSessionService.OnParticipantJoined += async participant =>
        {
            await InvokeAsync(() =>
            {
                if (_participants.Contains(participant)) return;

                _participants.Add(participant);
                StateHasChanged();
            });
        };

        CurrentSessionService.OnParticipantAnswered += async (participantAnswer) =>
        {
            await InvokeAsync(() =>
            {
                _votes.Add(participantAnswer);
                StateHasChanged();
            });
        };
        _currentQuestion = CurrentSessionService.CurrentQuestion;
    }

    public string QRCodeImage { get; set; }
    
    private async Task CopyToClipboard() =>
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", _inviteLink);


    private async Task SendQuestion(Guid questionId) =>
        await CurrentSessionService.SendCurrentQuestion();

    private int GetNumberOfVotes(Guid questionId)
    {
        if (!_votes.Any() || !_votes.Any(x => x.QuestionId == questionId))
            return 0;

        return _votes.Count(v => v.QuestionId == questionId);
    }

    private async Task NextQuestion()
    {
        _currentQuestion = CurrentSessionService.NextQuestion();
        await CurrentSessionService.SendCurrentQuestion();
        StateHasChanged();
    }

    private async Task PreviousQuestion()
    {
        _currentQuestion = CurrentSessionService.PreviousQuestion();
        await CurrentSessionService.SendCurrentQuestion();
        StateHasChanged();
    }
    
    private async Task OpenQRCodePage()
    {
        var url = NavigationManager.ToAbsoluteUri($"/qrcodepage/{Uri.EscapeDataString(_inviteLink)}").ToString();
        await JSRuntime.InvokeVoidAsync("window.open", url, "_blank");
    }
}