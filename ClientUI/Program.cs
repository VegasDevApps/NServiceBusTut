﻿
using ClientUI;
using Sales.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "ClientUI";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("ClientUI");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var transport = endpointConfiguration.UseTransport(new LearningTransport());
transport.RouteToEndpoint(typeof(PlaceOrder), "Sales");
transport.RouteToEndpoint(typeof(CancelOrder), "Sales");
builder.UseNServiceBus(endpointConfiguration);
builder.Services.AddHostedService<InputLoopService>();

await builder.Build().RunAsync();
