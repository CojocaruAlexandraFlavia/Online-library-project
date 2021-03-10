using Microsoft.AspNet.Identity;
using Proiect_DAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_DAW2.Controllers
{
    public class HomeController : Controller
    {
        private Proiect_DAW2.Models.ApplicationDbContext db = new Proiect_DAW2.Models.ApplicationDbContext();
        private int _perPage = 12;

        public ActionResult Index(string searchName)
        {
            SetAccessRights();
            var books = from b in db.Books.Include("Category").Include("Authors") select b;
            var books2 = from b in db.Books.Include("Category").Include("Authors") where b.Discount > 1 select b;
            books = books.OrderByDescending(p => p.Date).Take(12);


            List<Book> list_books = books.ToList<Book>();

            ViewBag.Books = list_books;
            foreach (var b in ViewBag.Books)
            {
                var x = b.Title.Length;
                if (x > 20)
                {
                    b.Title = b.Title.Substring(0, 19);
                    b.Title = b.Title + "...";
                }
            }

            List<Book> list_books2 = books2.ToList<Book>();
            IEnumerable<Book> list_books3 = list_books2.Take(12);
            list_books2 = list_books3.ToList();
            ViewBag.Books2 = list_books2;
            foreach (var b in ViewBag.Books2)
            {
                var x = b.Title.Length;
                if (x > 20)
                {
                    b.Title = b.Title.Substring(0, 19);
                    b.Title = b.Title + "...";
                }
            }

            return View();


        }




        public ActionResult Cauta(String SearchName)
        {
            SetAccessRights();
            var books_ord = from b in db.Books.Include("Category").Include("Authors").OrderBy(p => p.Title) select b;

                books_ord = books_ord.Where(p => p.Title.Contains(SearchName) || p.Authors.Any(x => (x.FirstName.Contains(SearchName)) || p.Authors.Any(y => (y.LastName.Contains(SearchName)))));
                if (books_ord.Count() == 0)
                {
                    ViewBag.noData = true;
                    TempData["noData"] = "Niciun rezultat conform cautarii";
                    return View();
                }
                  ViewBag.SearchResult = books_ord;
            foreach (var b in ViewBag.SearchResult)
            {
                var x = b.Title.Length;
                if (x > 22)
                {
                    b.Title = b.Title.Substring(0, 21);
                    b.Title = b.Title + "...";
                }

            }
            return View();
           
        }
            
        

        public ActionResult OrderAscPrice()
        {
            var books = TempData["ordoneaza"] as IEnumerable<Book>;
            System.Diagnostics.Debug.WriteLine(books.Count());
            books = books.OrderBy(p => p.Price - ((p.Discount * p.Price) / 100));
            SetAccessRights();
            ViewBag.Books = books;
            foreach (var b in ViewBag.Books)
            {
                var x = b.Title.Length;
                if (x > 22)
                {
                    b.Title = b.Title.Substring(0, 21);
                    b.Title = b.Title + "...";
                }

            }
            return View();

        }


        public ActionResult OrderDescPrice()
        {
            var books = TempData["ordoneaza"] as IEnumerable<Book>;
            System.Diagnostics.Debug.WriteLine(books.Count());
            books = books.OrderByDescending(p => p.Price - ((p.Discount * p.Price) / 100));
            SetAccessRights();
            ViewBag.Books = books;
            foreach (var b in ViewBag.Books)
            {
                var x = b.Title.Length;
                if (x > 22)
                {
                    b.Title = b.Title.Substring(0, 21);
                    b.Title = b.Title + "...";
                }

            }
            return View();
        }


        public ActionResult OrderAscStars()
        {
            var books = TempData["ordoneaza"] as IEnumerable<Book>;
            System.Diagnostics.Debug.WriteLine(books.Count());
            books = books.OrderBy(p => p.Rating);
            SetAccessRights();
            ViewBag.Books = books;
            foreach (var b in ViewBag.Books)
            {
                var x = b.Title.Length;
                if (x > 22)
                {
                    b.Title = b.Title.Substring(0, 21);
                    b.Title = b.Title + "...";
                }

            }
            return View();
        }

        public ActionResult OrderDescStars()
        {
            var books = TempData["ordoneaza"] as IEnumerable<Book>;
            System.Diagnostics.Debug.WriteLine(books.Count());
            books = books.OrderByDescending(p => p.Rating);
            SetAccessRights();
            ViewBag.Books = books;
            return View();
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            SetAccessRights();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            SetAccessRights();
            return View();
        }

        private void SetAccessRights()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Colaborator") || User.IsInRole("Administrator"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Administrator");
            ViewBag.esteColaborator = User.IsInRole("Colaborator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            List<Item> cart = (List<Item>)Session["cart"];
            ViewBag.Cart = cart;
        }
    }
}