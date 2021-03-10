using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect_DAW2.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu")]
        [RegularExpression(@"[a-zA-Z ]{3,50}", ErrorMessage = "Introduceti o valoare valida pentru categorie!")]
        [StringLength(50, ErrorMessage ="Numele categoriei nu poate avea mai mult de 50 de caractere",MinimumLength = 3)]
        public string CategoryName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
