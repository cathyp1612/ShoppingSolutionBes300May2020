using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ShoppingApi.Mappers;
using ShoppingApi.Models;
using ShoppingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ShoppingApi.Hubs
{
    public class CurbsideHub : Hub
    {
        private IMapCurbsideOrders CurbsideMapper;
        private ILogger<CurbsideHub> Logger;
        private CurbsideChannel TheChannel; 
        private IMapper  Mapper;

        public CurbsideHub(IMapCurbsideOrders curbsideMapper, ILogger<CurbsideHub> logger, CurbsideChannel theChannel, IMapper mapper)
        {
            CurbsideMapper = curbsideMapper;
            Logger = logger;
            TheChannel = theChannel;
            Mapper = mapper;
        }

        public async Task PlaceOrder(CreateCurbsideOrder orderToBePlaced)
        {
            var response = await CurbsideMapper.PlaceOrder(orderToBePlaced);
            await TheChannel.AddCurbsideOrder(new CurbsideChannelRequest { OrderId = response.Id, ClientId=Context.ConnectionId});
            await Clients.Caller.SendAsync("OrderPlaced", Mapper.Map<CurbsideOrder>(response));

        }
    }
}
