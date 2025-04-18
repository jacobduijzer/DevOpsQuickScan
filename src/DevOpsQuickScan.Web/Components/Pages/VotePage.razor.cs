using DevOpsQuickScan.Web.Sessions;
using DevOpsQuickScan.Web.Surveys;
using Microsoft.AspNetCore.Components;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class VotePage : ComponentBase
{
     [Parameter]
     public string? SessionId { get; set; }
     
     [Parameter]
     public string? UserName { get; set; }
     
     [Inject]
     private NavigationManager NavigationManager { get; set; } = default!;

     [Inject]
     private IHubConnectionWrapper HubConnectionWrapper { get; set; } = default!;
     
     private string _sessionName = string.Empty;
     private Question? _currentQuestion;
     private Guid _selectedAnswerId;
     private bool _isAnswerSelected = false;

     protected override async Task OnInitializedAsync()
     {
          if (string.IsNullOrEmpty(SessionId))
               throw new InvalidOperationException("SessionId cannot be null or empty.");
          
          HubConnectionWrapper.OnNewQuestion += async question =>
          {
               await InvokeAsync(() =>
               {
                    _currentQuestion = question;
                    _isAnswerSelected = false;
                    StateHasChanged();
               });
          };

          await HubConnectionWrapper.Start(SessionId, NavigationManager.ToAbsoluteUri("/hub/voting").ToString());
          await HubConnectionWrapper.JoinSession(SessionId, UserName);
     }

     private void SelectAnswer(Guid answerId)
     {
          _selectedAnswerId = answerId;
          _isAnswerSelected = true;
     }

     private async Task SubmitVote()
     {
          if (_isAnswerSelected && _currentQuestion != null)
          {
               await HubConnectionWrapper.SendAnswer(SessionId, _currentQuestion.Id, _selectedAnswerId);
               _isAnswerSelected = false;
          }
     }
}