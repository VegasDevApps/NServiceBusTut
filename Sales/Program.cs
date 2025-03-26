using Microsoft.Extensions.Hosting;

Console.Title = "Sales";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Sales");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
var recoverability = endpointConfiguration.Recoverability();
//recoverability.Delayed(delayed => delayed.NumberOfRetries(0));
var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();