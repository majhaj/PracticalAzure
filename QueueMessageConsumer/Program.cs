
using Azure.Storage.Queues;
using QueueMessageConsumer;

var connectionString = "DefaultEndpointsProtocol=https;AccountName=projectname1sagwc;AccountKey=/7dgIMi6aXaatqfXVaHurj9gVYa6jCcBbj0pC7laF3+y0IcWWKZ8r+8hhJ7uTzuXMbdbMJDC5M0b+AStdz3a/A==;EndpointSuffix=core.windows.net";
var queueName = "returns";

QueueClient queueClient = new QueueClient(connectionString, queueName);

while(true)
{
   var message = queueClient.ReceiveMessage();
    if(message.Value != null)
    {
        var dto = message.Value.Body.ToObjectFromJson<ReturnDto>();
        Process(dto);
        await queueClient.DeleteMessageAsync(message.Value.MessageId, message.Value.PopReceipt);
    }

    await Task.Delay(1000);
}

void Process(ReturnDto dto)
{
    Console.WriteLine($"Processing return with id: {dto.Id}, " +
        $"for user: {dto.User} " +
        $"from address: {dto.Address}");
}