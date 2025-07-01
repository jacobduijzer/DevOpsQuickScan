var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DevOpsQuickScan_Api>("api");

builder.Build().Run();