using Microsoft.Extensions.Logging;
using Sales.Messages;

namespace Sales;

public class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) 
    : Saga<BuyersRemorseData>,
    IAmStartedByMessages<PlaceOrder>,
    IHandleTimeouts<BuyersRemorseIsOver>,
    IHandleMessages<CancelOrder>
{
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);
        Data.OrderId = message.OrderId;

        logger.LogInformation("Starting cool down period for order #{OrderId}.", Data.OrderId);
        await RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<PlaceOrder>(msg => msg.OrderId)
            .ToMessage<CancelOrder>(msg => msg.OrderId);
    }

    public async Task Timeout(BuyersRemorseIsOver timeout, IMessageHandlerContext context)
    {
        logger.LogInformation("Cooling down period for order #{OrderId} has elapsed.", Data.OrderId);

        var orderPlaced = new OrderPlaced
        {
            OrderId = Data.OrderId
        };

        await context.Publish(orderPlaced);

        MarkAsComplete();
    }

    public Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order #{OrderId} was cancelled.", message.OrderId);

        //TODO: Possibly publish an OrderCancelled event?

        MarkAsComplete();

        return Task.CompletedTask;
    }
}
public class BuyersRemorseData : ContainSagaData
{
    public string OrderId { get; set; }
}

public class BuyersRemorseIsOver
{
}