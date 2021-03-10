using Microsoft.AspNet.Identity;
using Proiect_DAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_DAW2.Controllers
{
    public class ReviewsController : Controller
    {
        private Proiect_DAW2.Models.ApplicationDbContext db = new Proiect_DAW2.Models.ApplicationDbContext();

        public ActionResult Index()
        {   
            return View();
        }

        [HttpDelete]
        [Authorize(Roles = "User,Colaborator,Administrator")]
        public ActionResult Delete(int id)
        {
            Review rev = db.Reviews.Find(id);
            if (rev.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                db.Reviews.Remove(rev);
                db.SaveChanges();
                return Redirect("/Books/Show/" + rev.BookId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "Books");
            }

        }

        [Authorize(Roles = "User,Colaborator,Administrator")]
        public ActionResult Edit(int id)
        {
            Review rev = db.Reviews.Find(id);

            if (rev.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                return View(rev);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "Books");
            }

        }

        [HttpPut]
        [Authorize(Roles = "User, Colaborator, Administrator")]
        public ActionResult Edit(int id, Review requestReview)
        {
            try
            {
                Review rev = db.Reviews.Find(id);

                if (rev.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
                {
                    if (TryUpdateModel(rev))
                    {
                        rev.Content = requestReview.Content;
                        db.SaveChanges();
                    }
                    return Redirect("/Books/Show/" + rev.BookId);
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                    return RedirectToAction("Index", "Books");
                }
            }
            catch (Exception e)
            {
                return View(requestReview);
            }
        }





    }
}