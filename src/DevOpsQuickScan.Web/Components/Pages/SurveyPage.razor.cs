using DevOpsQuickScan.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class SurveyPage : ComponentBase
{
    [Parameter] public string SessionName { get; set; } = string.Empty;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private SessionService SessionService { get; set; } = default!;
    [Inject] private CommunicationService CommunicationService { get; set; } = default!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;

    private string? _inviteLink;

    private List<Participant> _participants = new ();
    
    private QuestionWithAnswers? _currentQuestion;

    protected override async Task OnInitializedAsync()
    {
        var hubUri = NavigationManager.ToAbsoluteUri("/hub/voting");
        SessionService.OnParticipantJoined += async participant =>
        {
            await InvokeAsync(() =>
            {
                if (_participants.Contains(participant)) return;
        
                _participants.Add(participant);
                StateHasChanged();
            });
        };
        
        var sessionId = await SessionService.CreateSession(Guid.NewGuid(), SessionName, hubUri);
        await SessionService.Start();
        
        _inviteLink = NavigationManager.BaseUri + $"join?sessionid={sessionId}";
        _currentQuestion = SessionService.CurrentQuestion();

        // CurrentSessionService.OnParticipantAnswered += async (participantAnswer) =>
        // {
        //     await InvokeAsync(() =>
        //     {
        //         _votes.Add(participantAnswer);
        //         StateHasChanged();
        //     });
        // };
        // _currentQuestion = CurrentSessionService.CurrentQuestion;
    }

    public string? QrCodeImage { get; set; }
    
    private async Task CopyToClipboard() =>
        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", _inviteLink);


    private async Task SelectQuestion(int questionId) =>
        await SessionService.AskQuestion(questionId);

    private async Task RevealAnswers(int questionId) =>
        throw new NotImplementedException();

    private void NextQuestion()
    {
        _currentQuestion = SessionService.NextQuestion();
        StateHasChanged();
    }

    private void PreviousQuestion()
    {
        _currentQuestion = SessionService.PreviousQuestion();
        StateHasChanged();
    }
    
    private async Task OpenQrCodePage()
    {
        var url = NavigationManager.ToAbsoluteUri($"/qrcodepage/{Uri.EscapeDataString(_inviteLink)}").ToString();
        await JsRuntime.InvokeVoidAsync("window.open", url, "_blank");
    }
    
}