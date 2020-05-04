using Microsoft.AspNetCore.Mvc;
using ShoppingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class ShoppingListController : ControllerBase
    {
       
        [HttpGet("shoppinglist")]

        public async Task<ActionResult> GetFullShoppingList()
        {
            var response = new GetShoppingListResponse();
            return Ok(response);
        }

    }
}
