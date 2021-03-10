using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_DAW2.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public int OrderId { get; set; }
    }
}