using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect_DAW2.Models
{
    public class History
    {
        [Key]
        public int HistoryId { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
       
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}