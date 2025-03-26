using Microsoft.Extensions.Hosting;

Console.Title = "Shipping";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Shipping");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.UseTransport(new LearningTransport());

var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();