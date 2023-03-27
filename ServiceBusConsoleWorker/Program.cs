using Azure.Messaging.ServiceBus;

Console.WriteLine("Service Bus Consumer");

var connectionString = "";

var client = new ServiceBusClient(connectionString);

var processor = client.CreateProcessor("user-registered", "subscription2");

processor.ProcessMessageAsync += MessageHendler;
processor.ProcessErrorAsync += ErrorHandler;

Console.WriteLine("Processing started");
await processor.StopProcessingAsync();

Console.ReadKey();
Task ErrorHandler(ProcessErrorEventArgs arg)
{
    Console.WriteLine(arg.ErrorSource);
    Console.WriteLine(arg.Exception.ToString());

    return Task.CompletedTask;
}

Task MessageHendler(ProcessMessageEventArgs arg)
{
    string body = arg.Message.Body.ToString();
    Console.WriteLine(body);

    return Task.CompletedTask;
}