using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;
using PagedList.Mvc;
using PagedList;

namespace ThueXeVn.Controllers
{
    public class calllogController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: calllog
        public ActionResult Index()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            return View(db.call_log.ToList());
        }

        public ActionResult taixegoi(string phone)
        {
            var data = db.drivers.Where(x => x.phone == phone).FirstOrDefault();
            return PartialView("_taixegoi", data);
        }

        public ActionResult callcustomer(int? pg)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;

            var data = db.call_driver_log.Where(x => x.from_number != null && x.to_number != null).Select(x => x).OrderByDescending(x => x.id).ToList();

            //var sql = "";


            //var data = (from a in db.call_driver_log join b in db.drivers on a.from_number equals b.phone select new call_driver_log_model() { 
            //    id = a.id,
            //    from_number = a.from_number,
            //    to_number = a.to_number,
            //    date_time = a.date_time,
            //    driver_name = b.name
            //}).ToList().OrderByDescending(x=>x.date_time).ToList();


            return View(data.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult deletecalllog(long? id)
        {
            if (Config.getCookie("logged") == "") return Json("0", JsonRequestBehavior.AllowGet); ;
            string delete = "";
            try
            {
                var calllog = db.call_driver_log.Find(id);
                if (calllog != null)
                {
                    db.call_driver_log.Remove(calllog);
                    db.SaveChanges();
                    delete = "1";
                }
            }
            catch (Exception ex)
            {
                delete = "0";
                Config.SaveTolog(ex.ToString());
            }
            return Json(delete, JsonRequestBehavior.AllowGet);
        }

        // GET: calllog/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            call_log call_log = db.call_log.Find(id);
            if (call_log == null)
            {
                return HttpNotFound();
            }
            return View(call_log);
        }

        // GET: calllog/Create
        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            return View();
        }

        // POST: calllog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,phone,calls")] call_log call_log)
        {
            if (ModelState.IsValid)
            {
                db.call_log.Add(call_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(call_log);
        }

        // GET: calllog/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            call_log call_log = db.call_log.Find(id);
            if (call_log == null)
            {
                return HttpNotFound();
            }
            return View(call_log);
        }

        // POST: calllog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,phone,calls")] call_log call_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(call_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(call_log);
        }

        // GET: calllog/Delete/5
        public ActionResult Delete(long? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            call_log call_log = db.call_log.Find(id);
            if (call_log == null)
            {
                return HttpNotFound();
            }
            return View(call_log);
        }

        // POST: calllog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            call_log call_log = db.call_log.Find(id);
            db.call_log.Remove(call_log);
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
