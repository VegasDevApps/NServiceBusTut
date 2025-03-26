using Sales.Messages;
using Microsoft.Extensions.Hosting;

namespace ClientUI;

public class InputLoopService(IMessageSession messageSession)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var lastOrder = string.Empty;

        while (true)
        {
            Console.WriteLine("Press 'P' to place an order, 'C' to cancel an order, or 'Q' to quit.");
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.P:
                    // Instantiate the command
                    var command = new PlaceOrder { OrderId = Guid.NewGuid().ToString() };
                    // Send the command
                    Console.WriteLine($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                    await messageSession.Send(command, stoppingToken);
                    lastOrder = command.OrderId;
                    break;
                case ConsoleKey.C:
                    var cancelCommand = new CancelOrder
                    {
                        OrderId = lastOrder
                    };
                    await messageSession.Send(cancelCommand);
                    Console.WriteLine($"Sent a CancelOrder command, {cancelCommand.OrderId}");
                    break;
                case ConsoleKey.Q:
                    return;
                default:
                    Console.WriteLine("Unknown input. Please try again.");
                    break;
            }
        }
    }
}
