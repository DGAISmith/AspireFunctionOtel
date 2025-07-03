var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireFunctionOtel_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

var functionService =  builder.AddAzureFunctionsProject<Projects.AspireFunctionOtel_Function>("functionservice");

builder.AddProject<Projects.AspireFunctionOtel_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(functionService)
    .WaitFor(functionService);


builder.Build().Run();
