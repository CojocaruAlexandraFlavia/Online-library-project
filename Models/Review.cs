using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_DAW2.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu")]
        public int Stars { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }

}