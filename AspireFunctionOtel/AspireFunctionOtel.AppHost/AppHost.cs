using AspireFunctionOtel.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireFunctionOtel_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

var serviceBus = builder
    .AddAzureServiceBus("service-bus")
    .RunAsEmulator()
    .WithMessageCommand();

var queue = serviceBus.AddServiceBusQueue("queue1");

var functionService =  builder.AddAzureFunctionsProject<Projects.AspireFunctionOtel_Function>("functionservice")
    .WithReference(serviceBus)
    .WaitFor(queue);

builder.AddProject<Projects.AspireFunctionOtel_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(functionService)
    .WaitFor(functionService);

builder.Build().Run();
