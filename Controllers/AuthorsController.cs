using Microsoft.AspNet.Identity;
using Proiect_DAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_DAW2.Controllers
{
    public class AuthorsController : Controller
    {
        private Proiect_DAW2.Models.ApplicationDbContext db = new Proiect_DAW2.Models.ApplicationDbContext();
        private int _perPage = 30;
        // GET: Categories
        public ActionResult Index()
        {
            
            var authors = db.Authors.OrderBy(p => p.LastName);

    
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
        

            var totalItems = authors.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));
            var offset = 0;
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }
            var paginatedAuthors = authors.Skip(offset).Take(this._perPage);

            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
          
            ViewBag.Authors = paginatedAuthors;
            SetAccessRights();

            return View();
        }

        [Authorize(Roles = "Colaborator,Administrator")]
        public ActionResult New()
        {
            SetAccessRights();
            Author author = new Author();
            return View(author);
        }

        [HttpPost]
        [Authorize(Roles = "Colaborator,Administrator")]
        public ActionResult New(Author author)
        {
            SetAccessRights();
            try
            {
                if (ModelState.IsValid)
                {
                    author.UserId = User.Identity.GetUserId();
                    db.Authors.Add(author);
                    db.SaveChanges();
                    TempData["message"] = "Autorul a fost adaugat!";

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(author);
                }
            }
           catch(Exception e)
            {
                return View(author);
            }
           
        }

        [Authorize(Roles = "Colaborator,Administrator")]
        public ActionResult AnotherAuthor()
        {
            SetAccessRights();
            Author author = new Author();
            return View(author);
        }

        [HttpPost]
        [Authorize(Roles = "Colaborator,Administrator")]
        public ActionResult AnotherAuthor(Author author)
        {
            SetAccessRights();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Authors.Add(author);
                    db.SaveChanges();
                    TempData["message"] = "Autorul a fost adaugat!";
                    return RedirectToAction("New", "Books");
                }
                else
                {
                    return View(author);
                }
            }
            catch (Exception e)
            {
                return View(author);
            }

        }

        //[Authorize(Roles = "User,Colaborator,Administrator")]
        public ActionResult Show(int id)
        {
            Author author = db.Authors.Find(id);
            SetAccessRights();
            return View(author);
        }

        [Authorize(Roles = "Administrator, Colaborator")]
        public ActionResult Edit(int id)
        {
            SetAccessRights();
            Author author = db.Authors.Find(id);
            if (author.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {         
                return View(author);
            }
            else
            {
                TempData["message"] = "Nu aveti drepturi de editare asupra acestui autor!";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrator, Colaborator")]
        public ActionResult Edit(int id, Author requestAuthor)
        {
            SetAccessRights();
            if (requestAuthor.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                try
                {
                    Author author = db.Authors.Find(id);
                    if (TryUpdateModel(author))
                    {
                        author.FirstName = requestAuthor.FirstName;
                        author.LastName = requestAuthor.LastName;
                        db.SaveChanges();
                    }
                    TempData["message"] = "Autorul a fost editat!";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    return View();
                }
            }
            else
            {
                TempData["message"] = "Nu aveti drepturi de editare asupra acestui autor!";
                return RedirectToAction("Index");

            }

        }

        [HttpDelete]
        [Authorize(Roles = "Administrator, Colaborator")]
        public ActionResult Delete(int id)
        {
            SetAccessRights();
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
            db.SaveChanges();
            TempData["message"] = "Autorul a fost sters!";
            return RedirectToAction("Index");
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
        }

    }
}