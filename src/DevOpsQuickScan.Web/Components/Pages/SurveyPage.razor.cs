using DevOpsQuickScan.Domain;
using DevOpsQuickScan.Web.Sessions;
using DevOpsQuickScan.Web.Surveys;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class SurveyPage : ComponentBase
{
    [Parameter] public string SessionName { get; set; } = string.Empty;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Inject] private SessionService SessionService { get; set; } = default!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;


    private string _inviteLink;
    private List<Participant> _participants = new();
    private HashSet<ParticipantAnswer> _votes = new();

    private Question? _currentQuestion;

    protected override async Task OnInitializedAsync()
    {
        var sessionId = await SessionService.Start(
            SessionName,
            NavigationManager.ToAbsoluteUri("/hub/voting").ToString(),
            Path.Combine("Surveys", "survey-01.json"));

        _inviteLink = NavigationManager.BaseUri + $"join?session={sessionId}";
        SessionService.OnParticipantJoined += async participant =>
        {
            await InvokeAsync(() =>
            {
                if (_participants.Contains(participant)) return;

                _participants.Add(participant);
                StateHasChanged();
            });
        };

        SessionService.OnParticipantAnswered += async (participantAnswer) =>
        {
            await InvokeAsync(() =>
            {
                _votes.Add(participantAnswer);
                StateHasChanged();
            });
        };
        _currentQuestion = SessionService.CurrentQuestion;
    }

    private async Task CopyToClipboard() =>
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", _inviteLink);


    private async Task SendQuestion(Guid questionId) =>
        await SessionService.SendCurrentQuestion();

    private int GetNumberOfVotes(Guid questionId)
    {
        if (!_votes.Any() || !_votes.Any(x => x.QuestionId == questionId))
            return 0;

        return _votes.Count(v => v.QuestionId == questionId);
    }

    private async Task NextQuestion()
    {
        _currentQuestion = SessionService.NextQuestion();
        await SessionService.SendCurrentQuestion();
        StateHasChanged();
    }

    private async Task PreviousQuestion()
    {
        _currentQuestion = SessionService.PreviousQuestion();
        await SessionService.SendCurrentQuestion();
        StateHasChanged();
    }
}