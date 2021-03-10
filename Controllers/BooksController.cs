using Microsoft.AspNet.Identity;
using Proiect_DAW2.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Proiect_DAW2.Controllers
{
    public class BooksController : Controller
    {
        private Proiect_DAW2.Models.ApplicationDbContext db = new Proiect_DAW2.Models.ApplicationDbContext();

        private int _perPage = 12;

        public ActionResult Index()
        {
            SetAccessRights();
            List<Book> mylist = db.Books.ToList();
            ViewBag.lista = mylist;
            var books2 = from b in db.Books.Include("Category").Include("Authors") select b;
            var books = from b in db.Books.Include("Category").Include("Authors").OrderBy(p => p.Title) select b;
            var categ = from c in db.Categories select c;
            ViewBag.Categories = categ;
          
            if (!(User.IsInRole("Administrator")))
            {
                books2 = books2.Where(p => p.Permission == true);
                books = books.Where(p => p.Permission == true);
            }
                                
                ViewBag.Books = books2;
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.Message = TempData["message"];
                }
               
            
            foreach(var b in ViewBag.Books)
            {
                var x = b.Title.Length;
                if(x > 22)
                {
                    b.Title = b.Title.Substring(0, 21);
                    b.Title = b.Title + "...";
                }

            }
            
            var totalItems = books.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));
            System.Diagnostics.Debug.WriteLine("Books controler first page " + currentPage);
            var offset = 0;
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }
            var paginatedBooks = books.Skip(offset).Take(this._perPage);

            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Books = paginatedBooks;
           
            return View();

        } 

        public ActionResult OrderAscPrice()
        {
            var books = from b in db.Books.Include("Category").Include("Authors") select b;
            books = books.OrderBy(p => p.Price - ((p.Discount * p.Price )/ 100));
            SetAccessRights();
            var totalItems = books.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));
            var offset = 0;
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }
            var paginatedBooks = books.Skip(offset).Take(this._perPage);

            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Books = paginatedBooks;
            foreach (var b in ViewBag.Books)
            {
                var x = b.Title.Length;
                if (x > 22)
                {
                    b.Title = b.Title.Substring(0, 21);
                    b.Title = b.Title + "...";
                }

            }
            var categ = from c in db.Categories select c;
            ViewBag.Categories = categ;
            return View();
        }

        public ActionResult OrderDescPrice()
        {
            var books = from b in db.Books.Include("Category").Include("Authors") select b;
            books = books.OrderByDescending(p => p.Price - ((p.Discount * p.Price) / 100));
            SetAccessRights();
            var totalItems = books.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));
            var offset = 0;
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }
            var paginatedBooks = books.Skip(offset).Take(this._perPage);

            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Books = paginatedBooks;
            foreach (var b in ViewBag.Books)
            {
                var x = b.Title.Length;
                if (x > 22)
                {
                    b.Title = b.Title.Substring(0, 21);
                    b.Title = b.Title + "...";
                }

            }
            var categ = from c in db.Categories select c;
            ViewBag.Categories = categ;
            return View();
        }

        public ActionResult OrderAscStars()
        {
            var books = from b in db.Books.Include("Category").Include("Authors") select b;
            books = books.OrderBy(p => p.Rating);
            SetAccessRights();
            var totalItems = books.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));
            var offset = 0;
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }
            var paginatedBooks = books.Skip(offset).Take(this._perPage);

            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Books = paginatedBooks;
            foreach (var b in ViewBag.Books)
            {
                var x = b.Title.Length;
                if (x > 22)
                {
                    b.Title = b.Title.Substring(0, 21);
                    b.Title = b.Title + "...";
                }

            }
            var categ = from c in db.Categories select c;
            ViewBag.Categories = categ;
            return View();
        }

        public ActionResult OrderDescStars()
        {
            var books = from b in db.Books.Include("Category").Include("Authors") select b;
            books = books.OrderByDescending(p => p.Rating);
            SetAccessRights();
            var totalItems = books.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));
            var offset = 0;
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }
            var paginatedBooks = books.Skip(offset).Take(this._perPage);

            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Books = paginatedBooks;
            foreach (var b in ViewBag.Books)
            {
                var x = b.Title.Length;
                if (x > 22)
                {
                    b.Title = b.Title.Substring(0, 21);
                    b.Title = b.Title + "...";
                }

            }
            var categ = from c in db.Categories select c;
            ViewBag.Categories = categ;
            return View();
        }


        [Authorize(Roles = "Colaborator, Administrator")]
        public ActionResult New()
        {
            SetAccessRights();
            Book book = new Book();
            book.Categ = GetAllCategories();
            book.Auth = GetAllAuthors();
            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "Colaborator, Administrator")]
        public ActionResult New(Book book)
        {

            string uploadedFileName = book.Image.FileName;
            string uploadedFileExtension = Path.GetExtension(uploadedFileName);
            if (uploadedFileExtension.ToLower() == ".png" || uploadedFileExtension.ToLower() == ".jpg" || uploadedFileExtension.ToLower() == ".jpeg")
            {            
                string uploadFolderPath = Server.MapPath("~//Files//");       
                if (!Directory.Exists(uploadFolderPath))
                {           
                    Directory.CreateDirectory(uploadFolderPath);
                }
               book.Image.SaveAs(uploadFolderPath + uploadedFileName);
               book.ImageId = uploadedFileName;                    
            }
            SetAccessRights();
            book.Categ = GetAllCategories();
            book.Auth = GetAllAuthors();
            try
            {
                if (ModelState.IsValid)
                {
                    book.Authors = new Collection<Author>();
                    foreach(var selectedAuthorId in book.SelectedAuthors)
                    {
                        Author dbAuthor = db.Authors.Find(selectedAuthorId);
                        book.Authors.Add(dbAuthor);
                    }            
                    book.UserId = User.Identity.GetUserId();
                    book.Date = DateTime.Now;
                    db.Books.Add(book);                  
                    db.SaveChanges();                  
                    TempData["message"] = "Cartea a fost adaugata!";
                    return RedirectToAction("Index");
                }
                else
                {
                    book.Auth = GetAllAuthors();
                    return View(book);
                }

            }
            catch (Exception e)
            {
                book.Auth = GetAllAuthors();
                return View(book);
            }
        }


        public ActionResult Show(int id)
        {
            SetAccessRights();
            Book book = db.Books.Find(id);
            var reviews = book.Reviews;
            double suma = 0;
            foreach (var rev in reviews)
            {
                suma += rev.Stars;
            }
            var count = reviews.Count();
            if (count > 0)
            {
                book.Rating = Math.Truncate(suma / count * 100) / 100;
                db.SaveChanges();
            }
            else
            {
                book.Rating = 0.00;
                db.SaveChanges();
            }
           
            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "User, Colaborator, Administrator")]
        public ActionResult Show(Review review)
        {
            SetAccessRights();
            review.Date = DateTime.Now;
            review.UserId = User.Identity.GetUserId();
            Book b = db.Books.Find(review.BookId);        
            try
            {            
                if (ModelState.IsValid)
                {
                    if (User.IsInRole("Administrator") || User.IsInRole("User") || User.IsInRole("Colaborator"))
                    {
                        db.Reviews.Add(review);
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["message"] = "Inregistrati-va inainte sa adaugati review!";
                        return RedirectToAction("Login");
                    }
                    return Redirect("/Books/Show/" + review.BookId);
                }

                else
                {
                    Book bb = db.Books.Find(review.BookId);                   
                                   
                    return View(bb);
                }
            }

            catch (Exception e)
            {
                Book bb = db.Books.Find(review.BookId);
               
                return View(bb);
            }
        }
        
        [Authorize(Roles = "Administrator, Colaborator")]
        public ActionResult Edit(int id)
        {
            SetAccessRights();
            Book book = db.Books.Find(id);
            if (book.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                book.Categ = GetAllCategories();
                book.Auth = GetAllAuthors();
                List<int> currentSelection = new List<int>();
                foreach (var author in book.Authors)
                {
                    currentSelection.Add(author.AuthorId);
                }
                book.SelectedAuthors = currentSelection.ToArray();
                book.UserId = User.Identity.GetUserId();
                return View(book);
            }
            else
            {
                TempData["message"] = "Nu aveti drepturi de editare asupra acestei carti!";
                return RedirectToAction("Index");
            }
            
        }

        [HttpPut]
        [Authorize(Roles ="Administrator, Colaborator")]
        public ActionResult Edit(int id, Book requestBook)
        {
            SetAccessRights();
            requestBook.Categ = GetAllCategories();
            requestBook.Auth = GetAllAuthors();
            Book book = db.Books.Find(id);
            if (book.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {   
                try
                {
                    if (ModelState.IsValid)
                    {
                        foreach (Author currentAuthor in book.Authors.ToList())
                        {
                            book.Authors.Remove(currentAuthor);
                        }

                        foreach (var selectedAuthorId in requestBook.SelectedAuthors)
                        {
                            Author dbAuthor = db.Authors.Find(selectedAuthorId);
                            book.Authors.Add(dbAuthor);
                        }

                        if (TryUpdateModel(book))
                        {
                            book.Description = requestBook.Description;
                            book.Title = requestBook.Title;
                            book.Price = requestBook.Price;
                            book.Rating = requestBook.Rating;
                            book.PublishingHouse = requestBook.PublishingHouse;
                            book.Discount = requestBook.Discount;
                            book.Status = requestBook.Status;
                            book.ReleaseYear = requestBook.ReleaseYear;
                            book.NrPages = requestBook.NrPages;
                            book.Language = requestBook.Language;
                            book.NrExemplaries = requestBook.NrExemplaries;
                            db.SaveChanges();
                            TempData["message"] = "Cartea a fost modificata!";
                            return RedirectToAction("Show/" + book.BookId);
                        }
                        else
                        {
                            return View(requestBook);
                        }
                    }
                    else
                    {
                        return View(requestBook);
                    }
                }
                catch (Exception e)
                {
                    return View(requestBook);
                }

            }
            else
            {
                TempData["message"] = "Nu aveti drepturi de editare asupra acestei carti!";
                return RedirectToAction("Index");
            }
            
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator, Colaborator")]
        public ActionResult Delete(int id)
        {
            SetAccessRights();
            Book book = db.Books.Find(id);
            if (book.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                db.Books.Remove(book);
                db.SaveChanges();
                TempData["message"] = "Cartea a fost stearsa!";
                return RedirectToAction("Index");
            }   

            else
            {
                TempData["message"] = "Nu aveti dreptul de a sterge aceasta carte!";
                return RedirectToAction("Index");
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();
            var categories = from cat in db.Categories
                             select cat;
            foreach (var category in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllAuthors()
        {
            var selectList = new List<SelectListItem>();
            var authors = from aut in db.Authors select aut;
            foreach (var aut in authors)
            {
                selectList.Add(new SelectListItem
                {
                    Value = aut.AuthorId.ToString(),
                    Text = aut.FirstName.ToString() + " "+ aut.LastName.ToString()
                });
            }
            return selectList;
        }
     
        private void SetAccessRights()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Colaborator") || User.IsInRole("Administrator"))
            {
                ViewBag.afisareButoane = true;
            }
            var x= User.Identity.GetUserName();

            var y = from b in db.Users where b.UserName == "admin@admin.com" select b;

            ViewBag.idAdmin = "8be8d104-8e01-41bb-85f9-a7decabfcb4f";
            ViewBag.esteAdmin = User.IsInRole("Administrator");
            ViewBag.esteColaborator = User.IsInRole("Colaborator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }

        public ActionResult Approval (int id)
        {
            var book = db.Books.Find(id);
            book.Permission = true;
            db.SaveChanges();
            SetAccessRights();
            var books = db.Books;
            ViewBag.Books = books.ToList();
            TempData["message"] = "Carte acceptata :)";
            return RedirectToAction("Index");
        }

        public ActionResult Decline(int id)
        {
            var book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            SetAccessRights();
            var books = db.Books;
            ViewBag.Books = books.ToList();
            TempData["message"] = "Carte respinsa :(";
            return RedirectToAction("Index");
        }
    }
}