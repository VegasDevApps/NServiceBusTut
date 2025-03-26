using System;
using Billing.Messages;
using Microsoft.Extensions.Logging;

namespace Shipping;

public class OrderBilledHandler(ILogger<OrderBilledHandler> logger) 
    : IHandleMessages<OrderBilled>
{
    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OrderBilled, OrderId = {orderId} - Shipping...", message.OrderId);

        return Task.CompletedTask;
    }
}
