using Billing.Messages;
using Microsoft.Extensions.Logging;
using Sales.Messages;
using Shipping.Messages;

namespace Shipping;

public class ShippingPolicy(ILogger<ShippingPolicy> logger)
    : Saga<ShippingPolicyData>,
    IAmStartedByMessages<OrderBilled>, 
    IAmStartedByMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OrderPlaced, OrderId = {OrderId}", message.OrderId);
        this.Data.IsOrderPlaced = true;
        return ProcessOrder(context);
    }

    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OrderBilled, OrderId = {OrderId}", message.OrderId);
        this.Data.IsOrderBilled = true;
        return ProcessOrder(context);
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<OrderBilled>(msg => msg.OrderId)
            .ToMessage<OrderPlaced>(msg => msg.OrderId);
    }

    private async Task ProcessOrder(IMessageHandlerContext context)
{
    if (Data.IsOrderPlaced && Data.IsOrderBilled)
    {
        await context.SendLocal(new ShipOrder() { OrderId = Data.OrderId });
        MarkAsComplete();
    }
}
}
