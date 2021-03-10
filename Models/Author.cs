using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect_DAW2.Models
{

    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "Campul nume este obligatoriu")]
        [StringLength(50, ErrorMessage = "Prenumele nu poate avea mai mult de 50 caractere")]
        public string FirstName { get; set; }
        [StringLength(50, ErrorMessage = "Numele nu poate avea mai mult de 50 caractere")]
        [Required(ErrorMessage = "Campul prenume este obligatoriu")]
        public string LastName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public bool IsChecked { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int[] SelectedBooks { get; set; }

                 
    }

}


