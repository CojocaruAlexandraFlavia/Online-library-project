using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect_DAW2.Models
{
    public class Order
    {

        [Key]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Campul adresa este obligatoriu")]
        public string Adress { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public double Total { get; set;}       

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int HistoryId { get; set; }
        public virtual History History { get; set; }
        
        public virtual ICollection<Item> Items { get; set; }
    }
}