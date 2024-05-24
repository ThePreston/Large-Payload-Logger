using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Microsoft.LargePayloadLogger.API
{
    public class LPLogger
    {
        private readonly ILogger<LPLogger> _logger;

        private readonly BlobContainerClient _containerClient;

        private readonly EventHubProducerClient _eventHubClient;

        public LPLogger(ILogger<LPLogger> logger, BlobContainerClient client, EventHubProducerClient producerClient)
        {
            _logger = logger;
            _containerClient = client;
            _eventHubClient = producerClient;
        }

        [Function("LPLogger")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var headerValues = req.Headers.ToList();

            var fileUri = await PersistToStorage($"{Guid.NewGuid()}.txt", $" Headers = {string.Join(';', headerValues.ToArray())}; Body = {requestBody}");

            await PersistToEventHub($"file uri = {fileUri}");

            return new OkObjectResult("Message Persisted");
        }

        private async Task<string> PersistToStorage(string fileName, string content)
        {

            var blobClient = _containerClient.GetBlobClient(fileName);

            using (var streamWriter = new StreamWriter(new MemoryStream()))
            {
                streamWriter.Write(content);
                streamWriter.Flush();
                streamWriter.BaseStream.Position = 0;

                await blobClient.UploadAsync(streamWriter.BaseStream, overwrite: true);

            }

            _logger.LogInformation($"Blob created {blobClient.Uri}");

            return blobClient.Uri.ToString();
        }


        private async Task PersistToEventHub(string content)
        {

            var eventBatch = await _eventHubClient.CreateBatchAsync();
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(content)));
            await _eventHubClient.SendAsync(eventBatch);

            _logger.LogInformation($"Persisted To EventHub ");

        }
    }
}
