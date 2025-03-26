using Microsoft.Extensions.Logging;
using Shipping.Messages;

namespace Shipping;

public class ShipOrderHandler(ILogger<ShipOrderHandler> logger) : IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order [{OrderId}] - Successfully shipped.", message.OrderId);
        return Task.CompletedTask;
    }
}
