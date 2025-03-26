using Sales.Messages;
using Microsoft.Extensions.Logging;

namespace Sales;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) 
    : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {orderId}", message.OrderId);
        
        if(Random.Shared.Next(0, 5) == 0)
        {
            // Some bug occur
            throw new Exception("Big Baga Boom!");
        }

        var orderPlaced = new OrderPlaced
        {
            OrderId = message.OrderId
        };
        
        return context.Publish(orderPlaced);
    }
}
