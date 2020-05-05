using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Models
{
    public class CreateCurbsideOrder
    {

        public string For { get; set; }
        public List<string> Items { get; set; }
    }
}

