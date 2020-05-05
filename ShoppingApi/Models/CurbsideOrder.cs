using ShoppingApi.Data;
using System.Collections.Generic;

namespace ShoppingApi.Models
{
    public class CurbsideOrder
    {
        public int Id { get; set; }
        public string For { get; set; }
        public List<string> Items { get; set; }

        public CurbsideOrderStatus Status { get; set; }
    }
}

