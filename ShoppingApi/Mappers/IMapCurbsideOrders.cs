using ShoppingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Mappers
{
    public interface IMapCurbsideOrders
    {
        Task<CurbsideOrder> PlaceOrder(CreateCurbsideOrder orderToPlace);
        Task<CurbsideOrder> GetOrderById(int id);
    }
}
