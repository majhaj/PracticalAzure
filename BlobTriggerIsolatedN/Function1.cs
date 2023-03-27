using System;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp.Processing;

namespace BlobTriggerIsolatedN
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("CronTriggerFunction")]
        public void RunCronTrigger([TimerTrigger("*/2 * * * *")]TimerInfo timerInfo)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {timerInfo.ScheduleStatus.Next}");
        }

        [Function("Function1")]
        [BlobOutput("results/{name}", Connection = "AzureWebJobsStorage")]
        public byte[] Run([BlobTrigger("samples-workitems/{name}", Connection = "AzureWebJobsStorage")] string myBlob, string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");

            var resized = new MemoryStream();

            using (var image = SixLabors.ImageSharp.Image.Load(myBlob))
            {
                image.Mutate(x => x.Resize(900, 600));
                image.Save(resized, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
            }

            _logger.LogInformation($"{name} has been processed");
            return resized.ToArray();
        }
    }
}
