using Proiect_DAW2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Proiect_DAW2.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required(ErrorMessage = "Campul titlu este obligatoriu")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Campul pret este obligatoriu")]
        public double Price { get; set; }
        public double Rating { get; set; }
        public string Status { get; set; } = "Disponibil";
        [Required(ErrorMessage = "Campul editura este obligatoriu")]
        public string PublishingHouse { get; set; }
        public int Discount { get; set; }
        [Required(ErrorMessage = "Campul editura este obligatoriu")]
        public string Description { get; set; }
        [Required]
        [Range(1, 2020, ErrorMessage = "Introduceti valori pozitive")]
        public int ReleaseYear { get; set; }
        [Range(1, 6000, ErrorMessage = "Introduceti valori pozitive")]
        [Required]
        public int NrPages { get; set; }
        [RegularExpression(@"[a-zA-Z]{3,30}", ErrorMessage = "Limba poate contine doar litere, nu cifre si caractere speciale")]
        [Required]
        public string Language { get; set;}
        public string ImageId { get; set; }
        [Required]
        [Range(1, 200, ErrorMessage = "Introduceti valori pozitive")]
        public int NrExemplaries { get; set; }
        public bool Permission { get; set; } = false;
        public DateTime Date { get; set; }

        [NotMapped]
        public HttpPostedFileBase Image { get; set; }

        [Required(ErrorMessage ="Campul este obligatoriu")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
 
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public IEnumerable<SelectListItem> Categ { get; set; }
        public IEnumerable<SelectListItem> Auth { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int[] SelectedAuthors { get; set; }


    }

}