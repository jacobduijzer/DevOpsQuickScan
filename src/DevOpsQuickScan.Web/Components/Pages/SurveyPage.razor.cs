using DevOpsQuickScan.Application;
using DevOpsQuickScan.Domain;
using DevOpsQuickScan.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class SurveyPage : ComponentBase
{
     [Parameter]
     public string? SessionId { get; set; }
     
     [Inject]
     private NavigationManager NavigationManager { get; set; } = default!;
     
     [Inject]
     private ISessionStore SessionStore { get; set; } = default!;
     
     [Inject]
     private SurveyReader SurveyReader { get; set; } = default!;
     
     [Inject]
     private IJSRuntime JSRuntime { get; set; } = default!;

     private HubConnection? _hubConnection;

     private string _sessionName;
     private string _inviteLink;
     private HashSet<Vote> _votes = new();
     private Survey _surveyData;
     
     protected override async Task OnInitializedAsync()
     {
         if (string.IsNullOrEmpty(SessionId))
         {
             throw new InvalidOperationException("SessionId cannot be null or empty.");
         }

         _sessionName = SessionStore.GetSessionName(SessionId)!;
         _inviteLink = NavigationManager.BaseUri + $"join?session={SessionId}";
         
         _hubConnection = new HubConnectionBuilder()
             .WithUrl(NavigationManager.ToAbsoluteUri("/hub/voting"))
             .WithAutomaticReconnect()
             .Build();

         // _hubConnection.On<string>("ParticipantJoined", async (userId) =>
         // {
         //     await InvokeAsync(() =>
         //     {
         //         participants.Add(userId);
         //         StateHasChanged();
         //     });
         // });

         // _hubConnection.On<string>("ParticipantLeft", async (userId) =>
         // {
         //     await InvokeAsync(() =>
         //     {
         //         participants.Remove(userId);
         //         StateHasChanged();
         //     });
         // });

         _hubConnection.On<Vote>("VoteReceived", async (vote) =>
         {
             await InvokeAsync(() =>
             {
                 _votes.Add(vote);
                 StateHasChanged();
             });
         });

         await _hubConnection.StartAsync();
         await _hubConnection.InvokeAsync("StartSession", SessionId);
         //await _hubConnection.InvokeAsync("JoinSession", SessionId);

         _surveyData = await SurveyReader.Read(Path.Combine("Surveys", "survey-01.json"));
     }
     
     private async Task CopyToClipboard() =>
         await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", _inviteLink);
     
     
     private async Task SendQuestion(Guid questionId)
     {
         var question = _surveyData.Questions.FirstOrDefault(q => q.Id == questionId);
         await _hubConnection.InvokeAsync("SendQuestion", question);
     }

     private int GetNumberOfVotes(Guid questionId)
     {
         if (!_votes.Any() || !_votes.Any(x => x.QuestionId == questionId))
             return 0;

         return _votes.Count(v => v.QuestionId == questionId);
     }
     

     private int _currentQuestionIndex = 0;

     private void NextQuestion()
     {
         if (_currentQuestionIndex < _surveyData.Questions.Count - 1)
         {
             _currentQuestionIndex++;
             StateHasChanged();
         }
     }

     private void PreviousQuestion()
     {
         if (_currentQuestionIndex > 0)
         {
             _currentQuestionIndex--;
             StateHasChanged();
         }
     }
     
}