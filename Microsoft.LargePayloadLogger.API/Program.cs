using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(config => {
        config.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", false, true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddScoped(provider =>
        {
            var blobservice = new BlobServiceClient(hostContext.Configuration.GetConnectionString("BlobStorage"));
            return blobservice.GetBlobContainerClient(hostContext.Configuration["BlobContainerName"]);
        });
        
        services.AddSingleton(provider => {
            return new EventHubProducerClient(hostContext.Configuration.GetConnectionString("EHConnString"), hostContext.Configuration["EventHubName"]);
        });

    }).Build();

await host.RunAsync();