using DevOpsQuickScan.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class SurveyPage : ComponentBase
{
    [Parameter] public string SessionName { get; set; } = string.Empty;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    // [Inject] private IQuestionRepository Questions { get; set; } = default!;
    // [Inject] private CurrentSessionService CurrentSessionService { get; set; } = default!;

    [Inject] private SessionService SessionService { get; set; } = default!;
    [Inject] private CommunicationService CommunicationService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;


    private string _inviteLink;
    // private HashSet<ParticipantAnswer> _votes = new();

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
        
        


        // SessionService.OnParticipantJoined += async participant =>
        // {
        //     await InvokeAsync(() =>
        //     {
        //         if (_participants.Contains(participant)) return;
        //
        //         _participants.Add(participant);
        //         StateHasChanged();
        //     });
        // };

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

    public string QRCodeImage { get; set; }
    
    private async Task CopyToClipboard() =>
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", _inviteLink);


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
    
    private async Task OpenQRCodePage()
    {
        var url = NavigationManager.ToAbsoluteUri($"/qrcodepage/{Uri.EscapeDataString(_inviteLink)}").ToString();
        await JSRuntime.InvokeVoidAsync("window.open", url, "_blank");
    }
}