var builder = DistributedApplication.CreateBuilder(args);

var blazor = builder.AddProject<Projects.DevOpsQuickScan_BlazorApp>("blazor");

builder.Build().Run();