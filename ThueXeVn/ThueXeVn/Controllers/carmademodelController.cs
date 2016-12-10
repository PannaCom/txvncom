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
    public class carmademodelController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: carmademodel
        public ActionResult Index()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            return View(db.car_made_model.ToList());
        }

        // GET: carmademodel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            car_made_model car_made_model = db.car_made_model.Find(id);
            if (car_made_model == null)
            {
                return HttpNotFound();
            }
            return View(car_made_model);
        }

        // GET: carmademodel/Create
        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            ViewBag.list_car = db.list_car.Select(x => new SelectListItem() { Value = x.name, Text = x.name });
            return View();
        }

        // POST: carmademodel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,made,model")] car_made_model car_made_model)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                db.car_made_model.Add(car_made_model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(car_made_model);
        }

        // GET: carmademodel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            car_made_model car_made_model = db.car_made_model.Find(id);
            ViewBag.list_car = db.list_car.Select(x => new SelectListItem() { Value = x.name, Text = x.name });
            if (car_made_model == null)
            {
                return HttpNotFound();
            }
            return View(car_made_model);
        }

        // POST: carmademodel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,made,model")] car_made_model car_made_model)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                db.Entry(car_made_model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car_made_model);
        }

        // GET: carmademodel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            car_made_model car_made_model = db.car_made_model.Find(id);
            if (car_made_model == null)
            {
                return HttpNotFound();
            }
            return View(car_made_model);
        }

        // POST: carmademodel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            car_made_model car_made_model = db.car_made_model.Find(id);
            db.car_made_model.Remove(car_made_model);
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
