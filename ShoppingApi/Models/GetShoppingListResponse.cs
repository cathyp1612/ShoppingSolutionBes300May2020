using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Models
{
    public class GetShoppingListResponse
    {
        public List<ShoppingListItemResponse> Data { get; set; }
    }
    public class ShoppingListItemResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Purchased { get; set; }
        public string PurchasedFrom { get; set; }
    }
}
