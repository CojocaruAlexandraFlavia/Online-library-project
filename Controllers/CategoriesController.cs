using Microsoft.AspNet.Identity;
using Proiect_DAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_DAW2.Controllers
{
    public class CategoriesController : Controller
    {
        private Proiect_DAW2.Models.ApplicationDbContext db = new Proiect_DAW2.Models.ApplicationDbContext();
        private int _perPage = 10;
        // GET: Categories
        //[Authorize(Roles ="Administrator, Colaborator, User")]
        public ActionResult Index()
        {
            var categories = db.Categories.OrderBy(p => p.CategoryName);
            SetAccessRights();

            var totalItems = categories.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));
            var offset = 0;
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }
            var paginatedCategories = categories.Skip(offset).Take(this._perPage);

            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Categories = paginatedCategories;

            return View();
        }

        //[Authorize(Roles="Adminstrator")]
        public ActionResult New()
        {
            SetAccessRights();
            return View();
        }

        //[Authorize(Roles = "Adminstrator")]
        [HttpPost]
        public ActionResult New(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            SetAccessRights();
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Administrator , User, Colaborator")]
        public ActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            SetAccessRights();
            return View(category);
        }


        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                Category category = db.Categories.Find(id);
                if (TryUpdateModel(category))
                {
                    category.CategoryName = requestCategory.CategoryName;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index", "Books");
        }

        [Authorize(Roles = "Administrator, Colaborator")]
        public ActionResult AnotherCategory()
        {
            Category category = new Category();
            SetAccessRights();
            return View(category);
        }

        [Authorize(Roles = "Administrator, Colaborator")]
        [HttpPost]
        public ActionResult AnotherCategory(Category category)
        {

            db.Categories.Add(category);
            SetAccessRights();
            db.SaveChanges();
            return RedirectToAction("New", "Books");
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