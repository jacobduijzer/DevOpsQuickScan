var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.DevOpsQuickScan_Web>("web");
builder.Build().Run();