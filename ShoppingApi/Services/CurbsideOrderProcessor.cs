using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingApi.Data;
using ShoppingApi.Hubs;
using ShoppingApi.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



namespace ShoppingApi.Services
{
    public class CurbsideOrderProcessor : BackgroundService
    {
        private readonly ILogger<CurbsideOrderProcessor> Logger;
        private readonly CurbsideChannel TheChannel;
        private readonly IServiceProvider ServiceProvider;
        private readonly IMapper Mapper;
        private readonly IHubContext<CurbsideHub> CurbsideHub;
        

        public CurbsideOrderProcessor(ILogger<CurbsideOrderProcessor> logger, CurbsideChannel theChannel, IServiceProvider serviceProvider, IMapper mapper, IHubContext<CurbsideHub> curbsideHub)
        {
            Logger = logger;
            TheChannel = theChannel;
            ServiceProvider = serviceProvider;
            Mapper = mapper;
            CurbsideHub = curbsideHub;
        }



        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var order in TheChannel.ReadAllAsync())
            {
                Logger.LogInformation("Got an Order to Process" + order.OrderId);
                using var scope = ServiceProvider.CreateScope();



                var context = scope.ServiceProvider.GetRequiredService<ShoppingDataContext>();



                var savedOrder = await context.CurbsideOrders.SingleAsync(o => o.Id == order.OrderId);



                var items = savedOrder.Items.Split(',').Count();



                for (var t = 0; t < items; t++)
                {
                    Logger.LogInformation("Processing an Item");
                    if (order.ClientId != null)
                    {
                        await CurbsideHub.Clients.Client(order.ClientId).SendAsync("OrderItemProcessed",
                            new { id = order.OrderId, itemId = t + 1 }
                            );
                    }
                    await Task.Delay(1000);
                }
                savedOrder.Status = CurbsideOrderStatus.Processed;
                await context.SaveChangesAsync();



                // Tell the client that made this request through a socket that it has been processed.
                if (order.ClientId != null)
                {
                    await CurbsideHub.Clients.Client(
                        order.ClientId).SendAsync("OrderProcessed", Mapper.Map<CurbsideOrder>(savedOrder));
                }
            }
        }
    }
}