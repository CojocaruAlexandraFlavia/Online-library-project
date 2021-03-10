using Microsoft.AspNet.Identity;
using Proiect_DAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_DAW2.Controllers
{
    public class HistoriesController : Controller
    {
        private Proiect_DAW2.Models.ApplicationDbContext db = new Proiect_DAW2.Models.ApplicationDbContext();
       

        [Authorize(Roles = "User, Administrator, Colaborator")]
        public ActionResult Show(int? id)
        {
            if(id == null)
            {
                ViewBag.istoricGol = true;
                return View();
            }
            History history = db.Histories.Find(id);
            return View(history);
        }


        [Authorize(Roles ="User, Colaborator, Administrator")]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var i = 0;
            var id2 = false;
            foreach(var history in db.Histories)
            {
                if (history.UserId == userId)
                {
                    i = history.HistoryId;
                    foreach(var order in history.Orders)
                    {
                        if (order.OrderId == id)
                        {
                            id2 = true;
                            
                        }
                    }
                }
            }
            if(id2 == true)
            {
                Order deleteo = db.Orders.Find(id);
                db.Orders.Remove(deleteo);
                db.SaveChanges();
                TempData["message"] = "Comanda a fost stearsa";
                return RedirectToAction("/Show/" + i);

            }
            
           
            TempData["message"] = "Comanda nu a fost stearsa";
            return RedirectToAction("/Show/" + i);

        }
        [Authorize(Roles = "User, Colaborator, Administrator")]
        [Authorize(Roles = "User")]
        public ActionResult Delete (int id)
        {
            History history = db.Histories.Find(id);
            db.Histories.Remove(history);
            db.SaveChanges();
            TempData["message"] = "Istoricul a fost sters";
            return RedirectToAction("Home");
            
        }

    }
}