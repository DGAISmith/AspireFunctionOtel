var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireFunctionOtel_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.AspireFunctionOtel_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddAzureFunctionsProject<Projects.AspireFunctionOtel_Function>("aspirefunctionotel-function");

builder.Build().Run();
