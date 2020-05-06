using Microsoft.AspNetCore.Mvc;
using ShoppingApi.Data;
using ShoppingApi.Mappers;
using ShoppingApi.Migrations;
using ShoppingApi.Models;
using ShoppingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class CurbsideOrdersController : ControllerBase
    {
        private readonly IMapCurbsideOrders CurbsideMapper;
        private readonly CurbsideChannel TheChannel;
        private readonly ShoppingDataContext Context;

        public CurbsideOrdersController(IMapCurbsideOrders curbsideMapper, CurbsideChannel theChannel, ShoppingDataContext context)
        {
            CurbsideMapper = curbsideMapper;
            TheChannel = theChannel;
            Context = context;
        }

        [HttpPost("curbsideordersync")]
        public async Task<ActionResult> PlaceOrderSynchronously([FromBody] CreateCurbsideOrder orderToPlace)
        {
            var temp = await CurbsideMapper.PlaceOrder(orderToPlace);
            for (var t = 0; t < temp.Items.Count; t++)
            {
                Thread.Sleep(1000);
            }
            temp.Status = CurbsideOrderStatus.Processed;
            var order = await Context.SaveChangesAsync();
            return Ok(temp); // not going to map it... just want you to see.



        }


        [HttpPost("curbsideorders")]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateCurbsideOrder orderToPlace)
        {
            CurbsideOrder response = await CurbsideMapper.PlaceOrder(orderToPlace);

            await TheChannel.AddCurbsideOrder(new CurbsideChannelRequest { OrderId = response.Id });


            return Ok(response);
        }

        [HttpGet("curbsideorders/{id:int}")]
        public async Task<ActionResult<CurbsideOrder>> GetById(int id)
        {
            CurbsideOrder response = await CurbsideMapper.GetOrderById(id);
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }



        }
    }
    
    }

