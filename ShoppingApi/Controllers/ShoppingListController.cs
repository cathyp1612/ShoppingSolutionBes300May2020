using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingApi.Data;
using ShoppingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class ShoppingListController : ControllerBase
    {
        private readonly ShoppingDataContext DataContext;

        public ShoppingListController(ShoppingDataContext dataContext)
        {
            DataContext = dataContext;
        }

        [HttpGet("shoppinglist")]

        public async Task<ActionResult> GetFullShoppingList()
        {
            var response = new GetShoppingListResponse();

            response.Data = await DataContext.ShoppingItems
                    .Select(item => new ShoppingLIstItemResponse
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Purchased = item.Purchased
                    }).ToListAsync();

            return Ok(response);
        }

    }
}
