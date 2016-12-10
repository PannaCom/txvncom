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
    public class listcarController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: listcar
        public ActionResult Index()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            return View(db.list_car.ToList());
        }

        // GET: listcar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            list_car list_car = db.list_car.Find(id);
            if (list_car == null)
            {
                return HttpNotFound();
            }
            return View(list_car);
        }

        // GET: listcar/Create
        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            return View();
        }

        // POST: listcar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,no")] list_car list_car)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                db.list_car.Add(list_car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(list_car);
        }

        // GET: listcar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            list_car list_car = db.list_car.Find(id);
            if (list_car == null)
            {
                return HttpNotFound();
            }
            return View(list_car);
        }

        // POST: listcar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,no")] list_car list_car)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                db.Entry(list_car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(list_car);
        }

        // GET: listcar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            list_car list_car = db.list_car.Find(id);
            if (list_car == null)
            {
                return HttpNotFound();
            }
            return View(list_car);
        }

        // POST: listcar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            list_car list_car = db.list_car.Find(id);
            db.list_car.Remove(list_car);
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
