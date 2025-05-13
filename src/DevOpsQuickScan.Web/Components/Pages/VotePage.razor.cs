using BlazorBootstrap;
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
     
     //-- pie test
     private PieChart pieChart = default!;
     private PieChartOptions pieChartOptions = default!;
     private ChartData chartData = default!;
     private string[]? backgroundColors;

     private int datasetsCount = 0;
     private int dataLabelsCount = 0;

     private Random random = new();

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
          
          // backgroundColors = ColorUtility.CategoricalTwelveColors;
          // chartData = new ChartData { Labels = GetDefaultDataLabels(4), Datasets = GetDefaultDataSets(1) };
          //
          // pieChartOptions = new();
          // pieChartOptions.Responsive = true;
          // pieChartOptions.Plugins.Title!.Text = "2022 - Sales";
          // pieChartOptions.Plugins.Title.Display = true;
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
     
     protected override async Task OnAfterRenderAsync(bool firstRender)
    {
            // await pieChart.InitializeAsync(chartData, pieChartOptions);
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task RandomizeAsync()
    {
        if (chartData is null || chartData.Datasets is null || !chartData.Datasets.Any()) return;

        var newDatasets = new List<IChartDataset>();

        foreach (var dataset in chartData.Datasets)
        {
            if (dataset is PieChartDataset pieChartDataset
                && pieChartDataset is not null
                && pieChartDataset.Data is not null)
            {
                var count = pieChartDataset.Data.Count;

                var newData = new List<double?>();
                for (var i = 0; i < count; i++)
                {
                    newData.Add(random.Next(0, 100));
                }

                pieChartDataset.Data = newData;
                newDatasets.Add(pieChartDataset);
            }
        }

        chartData.Datasets = newDatasets;

        await pieChart.UpdateAsync(chartData, pieChartOptions);
    }

    private async Task AddDatasetAsync()
    {
        if (chartData is null || chartData.Datasets is null) return;

        var chartDataset = GetRandomPieChartDataset();
        chartData = await pieChart.AddDatasetAsync(chartData, chartDataset, pieChartOptions);
    }

    private async Task AddDataAsync()
    {
        if (dataLabelsCount >= 12)
            return;

        if (chartData is null || chartData.Datasets is null)
            return;

        var data = new List<IChartDatasetData>();
        foreach (var dataset in chartData.Datasets)
        {
            if (dataset is PieChartDataset pieChartDataset)
                data.Add(new PieChartDatasetData(pieChartDataset.Label, random.Next(0, 100), backgroundColors![dataLabelsCount]));
        }

        chartData = await pieChart.AddDataAsync(chartData, GetNextDataLabel(), data);

        dataLabelsCount += 1;
    }

    #region Data Preparation

    private List<IChartDataset> GetDefaultDataSets(int numberOfDatasets)
    {
        var datasets = new List<IChartDataset>();

        for (var index = 0; index < numberOfDatasets; index++)
        {
            datasets.Add(GetRandomPieChartDataset());
        }

        return datasets;
    }

    private PieChartDataset GetRandomPieChartDataset()
    {
        datasetsCount += 1;
        return new() { Label = $"Team {datasetsCount}", Data = GetRandomData(), BackgroundColor = GetRandomBackgroundColors() };
    }

    private List<double?> GetRandomData()
    {
        var data = new List<double?>();
        for (var index = 0; index < dataLabelsCount; index++)
        {
            data.Add(random.Next(0, 100));
        }

        return data;
    }

    private List<string> GetRandomBackgroundColors()
    {
        var colors = new List<string>();
        for (var index = 0; index < dataLabelsCount; index++)
        {
            colors.Add(backgroundColors![index]);
        }

        return colors;
    }

    private List<string> GetDefaultDataLabels(int numberOfLabels)
    {
        var labels = new List<string>();
        for (var index = 0; index < numberOfLabels; index++)
        {
            labels.Add(GetNextDataLabel());
            dataLabelsCount += 1;
        }

        return labels;
    }

    private string GetNextDataLabel() => $"Product {dataLabelsCount + 1}";

    private string GetNextDataBackgrounfColor() => backgroundColors![dataLabelsCount];

    #endregion  Data Preparation
}