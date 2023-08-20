using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventHubsReceiver
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Create a blob container client that the event processor will use 
            BlobContainerClient storageClient = new BlobContainerClient(
                "DefaultEndpointsProtocol=https;AccountName=teststorageforlogic2023;AccountKey=AGIHM6mm3H/vUaJftx1A6xH7ePGWQCasSmkU1SEEKVnRwbm5NVpZJEMToQIVeojMfuPCmaAV/oiJ+AStYjrGCA==;EndpointSuffix=core.windows.net", "testcon1");

            // Create an event processor client to process events in the event hub
            var processor = new EventProcessorClient(
                storageClient,
                EventHubConsumerClient.DefaultConsumerGroupName,
                "Endpoint=sb://testeh2023.servicebus.windows.net/;SharedAccessKeyName=test1;SharedAccessKey=HAlVR258Fax2xULFCTHiniDBQF+XGdDEn+AEhAIuf/4=;EntityPath=myhub1",
                "myhub1");

            // Register handlers for processing events and handling errors
            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;

            // Start the processing
            await processor.StartProcessingAsync();

            // Wait for 30 seconds for the events to be processed
            await Task.Delay(TimeSpan.FromSeconds(30));

            // Stop the processing
            await processor.StopProcessingAsync();

            Task ProcessEventHandler(ProcessEventArgs eventArgs)
            {
                // Write the body of the event to the console window
                Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
                return Task.CompletedTask;
            }

            Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
            {
                // Write details about the error to the console window
                Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
                Console.WriteLine(eventArgs.Exception.Message);
                return Task.CompletedTask;
            }
        }
    }
}
