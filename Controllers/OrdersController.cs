using Microsoft.AspNet.Identity;
using Proiect_DAW2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_DAW2.Controllers
{
    [Authorize(Roles = "Administrator , User, Colaborator")]
    public class OrdersController : Controller
    {
        private Proiect_DAW2.Models.ApplicationDbContext db = new Proiect_DAW2.Models.ApplicationDbContext();

        public ActionResult Index()
        {
            List<Item> cart = (List<Item>)Session["cart"];
            ViewBag.Cart = cart;
            SetAccessRights();
            return View();
        }

        public ActionResult Buy(int id)
        {
            var ok = false;
            foreach (var v in db.Orders)
            {
                if (v.OrderStatus == "Activa" && v.UserId == User.Identity.GetUserId())
                {
                    ok = true;
                }

            }
            if (ok == false)
            {
                Order order = new Order();
                order.UserId = User.Identity.GetUserId();
                order.OrderDate = DateTime.Now;
                order.Adress = "NULL";
                order.OrderStatus = "Activa";
                db.Orders.Add(order);

                var buff = false;
                foreach (var t in db.Histories)
                {
                    if (t.UserId == User.Identity.GetUserId())
                    {
                        buff = true;
                        order.HistoryId = t.HistoryId;
                    }

                }
                if (buff == false)
                {
                    History history = new History();
                    history.UserId = User.Identity.GetUserId();
                    order.HistoryId = history.HistoryId;
                    db.Histories.Add(history);
                }

                if (Session["cart"] == null)
                {
                    List<Item> cart = new List<Item>();
                    Book book = db.Books.Find(id);
                    Item item = new Item
                    {
                        Book = book,
                        Quantity = 1,
                        OrderId = order.OrderId
                    };
                    cart.Add(item);
                    db.Items.Add(item);
                    Session["cart"] = cart;
                }
            }

            else
            {
                var orderId = -1;
                foreach (var o in db.Orders)
                {
                    if (o.OrderStatus == "Activa" && o.UserId == User.Identity.GetUserId())
                    {
                        orderId = o.OrderId;
                    }
                }
                if (Session["cart"] == null)
                {
                    List<Item> cart = new List<Item>();
                    Book book = db.Books.Find(id);
                    Item item = new Item
                    {
                        Book = book,
                        Quantity = 1,
                        OrderId = orderId
                    };
                    cart.Add(item);
                    db.Items.Add(item);
                    Session["cart"] = cart;

                }

                else
                {
                    List<Item> cart = (List<Item>)Session["cart"];
                    Book book = db.Books.Find(id);
                    int index = Exist(id);
                    if (index != -1)
                    {
                        cart[index].Quantity++;
                    }
                    else
                    {
                        Item item = new Item
                        {
                            Book = book,
                            Quantity = 1,
                            OrderId = orderId
                        };
                        cart.Add(item);
                        db.Items.Add(item);
                    }
                    Session["cart"] = cart;
                }


            }
            SetAccessRights();
            db.SaveChanges();
            return RedirectToAction("Index", "Books");

        }

        public ActionResult Show(int id)
        {
            Order order = db.Orders.Find(id);
            SetAccessRights();
            return View(order);
        }

        public ActionResult FinishOrder()
        {
            foreach(var v in db.Orders)
            {
                if (v.OrderStatus == "Activa" && v.UserId == User.Identity.GetUserId())
                {
                    if (v.Items.Count() == 0)
                    {
                        TempData["order"] = "Nu puteti plasa o comanda fara produse";
                        return RedirectToAction("ShowCart");
                    }
                }
            }
            SetAccessRights();
            return View();
        }

        [HttpPost]
        public ActionResult FinishOrder(Order order)
        {
              
            double total = 0;
            List<Item> cart = (List<Item>)Session["cart"];

            foreach (var v in db.Orders)
            {
                if (v.OrderStatus == "Activa" && v.UserId == User.Identity.GetUserId())
                {
                    v.Adress = order.Adress;

                    foreach (Item item in cart)
                    {
                        total += (item.Book.Price - (item.Book.Price * item.Book.Discount / 100)) * item.Quantity;
                      

                    }
                    total += 15; 
                    v.Total = total;
                    v.OrderStatus = "In procesare";
                    foreach (var t in db.Histories)
                    {
                        if (t.UserId == User.Identity.GetUserId())
                        {                       
                            t.Orders.Add(v);
                        }
                      
                    }

                    foreach (var x in v.Items)
                    {
                        System.Diagnostics.Debug.WriteLine(x.Book.Title + ' ' + x.Quantity);
                    }

                }
              

            }
                foreach(Item item in cart)
                {
                    Book book = db.Books.Find(item.BookId);
                    book.NrExemplaries = book.NrExemplaries - item.Quantity;
                }
                db.SaveChanges();
                
                TempData["order"] = "Comanda a fost plasata";
                Session["cart"] = null;


            SetAccessRights();


                return RedirectToAction("Index", "Books");
          }
        
        public ActionResult IncreaseQuantity(int id)
        {

            foreach (var v in db.Orders)
            {
                if (v.OrderStatus == "Activa" && v.UserId == User.Identity.GetUserId())
                {
                        foreach(Item item in v.Items)
                        {
                        if (item.ItemId == id)
                        {
                            if(item.Book.NrExemplaries > item.Quantity)
                            {
                                item.Quantity++;
                            }
                            else
                            {
                                TempData["cantitate"] = "Stoc insuficient";
                            }
                        }
                    }
                }
            }


            List<Item> cart = (List<Item>)Session["cart"];
            Item i = cart.Find(x => x.ItemId == id);
            if(i.Book.NrExemplaries > i.Quantity){
                i.Quantity++;
            }
            else
            {
                TempData["cantitate"] = "Stoc insuficient";
            }
            Session["cart"] = cart;
            SetAccessRights();
            db.SaveChanges();
            return RedirectToAction("ShowCart");
        }

        public ActionResult DecreaseQuantity(int id)
        {

            foreach (var v in db.Orders)
            {
                if (v.OrderStatus == "Activa" && v.UserId == User.Identity.GetUserId())
                {
                    foreach (Item item in v.Items)
                    {
                        if (item.ItemId == id)
                        {
                            if (item.Quantity > 0)
                            {
                                item.Quantity--;
                            }
                            else
                            {
                                TempData["cantitate"] = "Cantitatea este deja 0";
                            }
                        }
                    }
                }
            }
            List<Item> cart = (List<Item>)Session["cart"];
            Item i = cart.Find(x => x.ItemId == id);
            if (i.Quantity > 0)
            {
                i.Quantity--;
            }
            else
            {
                TempData["cant"] = "Cantitatea este deja 0";
            }
            Session["cart"] = cart;
            SetAccessRights();
            db.SaveChanges();
            return RedirectToAction("ShowCart");
        }



        public int Exist(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Book.BookId.Equals(id))
                    return i;
            return -1;
        }

        public ActionResult Remove(int id)
        {

            List<Item> cart = (List<Item>)Session["cart"];
            int index = Exist(id);
            cart.RemoveAt(index);
            Session["cart"] = cart;

            Item i = new Item();
            int vId = 0;
            foreach (var v in db.Orders)
            {
                if (v.OrderStatus == "Activa" && v.UserId == User.Identity.GetUserId())
                {
                    vId = v.OrderId;
                    foreach(var it in v.Items)
                    {
                        if(it.BookId == id)
                        {
                            i = it;
                            break;
                        }
                    }    
                    
                }
            }

            Order newO = db.Orders.Find(vId);
            db.Items.Remove(i);          
            db.SaveChanges();
            SetAccessRights();
            return View("ShowCart");
        }

        public ActionResult ShowCart()
        {
            SetAccessRights();
            if (Session["cart"] == null)
            {
                return View();
            }

            List<Item> cart = (List<Item>)Session["cart"];
            ViewBag.Cart = cart;
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


