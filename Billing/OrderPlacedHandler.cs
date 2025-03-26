using Billing.Messages;
using Microsoft.Extensions.Logging;
using Sales.Messages;

namespace Billing;

public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger) : IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OrderPlaced, OrderId = {orderId} - Charging credit card...", message.OrderId);

        var orderBilled = new OrderBilled
        {
            OrderId = message.OrderId
        };

        return context.Publish(orderBilled);
    }
}
