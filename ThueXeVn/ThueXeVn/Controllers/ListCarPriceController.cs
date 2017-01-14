using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;

namespace ThueXeVn.Controllers
{
    public class ListCarPriceController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: ListCarPrice
        public ActionResult Index()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            return View(db.pl_car_price.ToList());
        }

        // GET: ListCarPrice/Details/5
        public ActionResult Details(int? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pl_car_price pl_car_price = db.pl_car_price.Find(id);
            if (pl_car_price == null)
            {
                return HttpNotFound();
            }
            return View(pl_car_price);
        }

        // GET: ListCarPrice/Create
        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            ViewBag.pl_car_type = new List<SelectListItem>() { 
                new SelectListItem() { Value = "4", Text = "Xe 4 chỗ"},
                new SelectListItem() { Value = "7", Text = "Xe 7 chỗ" },
                new SelectListItem() { Value = "16", Text = "Xe 16 chỗ" },
                new SelectListItem() { Value = "29", Text = "Xe 29 chỗ" }
            };
            return View();
        }

        //public static List<SelectListItem> ListTypePost()
        //{
        //    List<SelectListItem> newList = new List<SelectListItem>();
        //    //Add select list item to list of selectlistitems
        //    newList.Add(new SelectListItem() { Value = "new", Text = "Tin tức", Selected = true });
        //    newList.Add(new SelectListItem() { Value = "thongbao", Text = "Thông báo" });
        //    newList.Add(new SelectListItem() { Value = "congvan", Text = "Công văn" });
        //    return newList;
        //}

        // POST: ListCarPrice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,pl_car_type,g1,g2,g3,g4,g5,g6,g7,g8")] pl_car_price pl_car_price)
        {
            if (ModelState.IsValid)
            {
                db.pl_car_price.Add(pl_car_price);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pl_car_price);
        }

        // GET: ListCarPrice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            pl_car_price pl_car_price = db.pl_car_price.Find(id);
            ViewBag.pl_car_type = new List<SelectListItem>() { 
                new SelectListItem() { Value = "4", Text = "Xe 4 chỗ"},
                new SelectListItem() { Value = "7", Text = "Xe 7 chỗ" },
                new SelectListItem() { Value = "16", Text = "Xe 16 chỗ" },
                new SelectListItem() { Value = "29", Text = "Xe 29 chỗ" }
            };
            if (pl_car_price == null)
            {
                return HttpNotFound();
            }
            return View(pl_car_price);
        }

        // POST: ListCarPrice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,pl_car_type,g1,g2,g3,g4,g5,g6,g7,g8")] pl_car_price pl_car_price)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pl_car_price).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pl_car_price);
        }

        // GET: ListCarPrice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pl_car_price pl_car_price = db.pl_car_price.Find(id);
            if (pl_car_price == null)
            {
                return HttpNotFound();
            }
            return View(pl_car_price);
        }

        // POST: ListCarPrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pl_car_price pl_car_price = db.pl_car_price.Find(id);
            db.pl_car_price.Remove(pl_car_price);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
