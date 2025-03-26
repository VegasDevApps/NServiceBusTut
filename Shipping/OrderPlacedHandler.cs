using Microsoft.Extensions.Logging;
using Sales.Messages;

namespace Shipping;

public class OrderPlacedHandler(ILogger<OrderBilledHandler> logger)
    : IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OrderPlaced, OrderId = {orderId} - Charging credit card...", message.OrderId);

        return Task.CompletedTask;
    }
}
