using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;
using PagedList;
using Newtonsoft.Json;
namespace ThueXeVn.Controllers
{
    public class XeBuytController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: XeBuyt
        public ActionResult Index(int? page)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            var p = (from q in db.find_bus select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            return View(p.ToPagedList(pageNumber, pageSize));
        }
        // GET: XeBuyt
        public ActionResult List(string sfrom, string sto,int? page)
        {
            if (sfrom == null) sfrom = "";
            if (sto == null) sto = "";
            var p = (from q in db.find_bus where q.bus_from.Contains(sfrom) && q.bus_to.Contains(sto) select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            ViewBag.sfrom = sfrom;
            ViewBag.sto = sto;
            return View(p.ToPagedList(pageNumber, pageSize));
        }
        public string Find(string keyword)
        {
            var p=(from q in db.bus_keyword where q.keyword.Contains(keyword) select q.keyword).Take(100).ToList();
            return JsonConvert.SerializeObject(p);
        }
        // GET: XeBuyt/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            find_bus find_bus = db.find_bus.Find(id);
            if (find_bus == null)
            {
                return HttpNotFound();
            }
            return View(find_bus);
        }

        // GET: XeBuyt/Create
        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            return View();
        }

        // POST: XeBuyt/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,bus_no,bus_des,bus_from,bus_to,bus_type,province")] find_bus find_bus)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                db.find_bus.Add(find_bus);
                db.SaveChanges();
                string[] bus_from=find_bus.bus_from.Split('-');
                string[] bus_to = find_bus.bus_to.Split('-');
                int i = 0;
                string temp="";
                for (i = 0; i < bus_from.Length; i++)
                {
                    temp=bus_from[i].Trim();
                    if (!db.bus_keyword.Any(o => o.keyword == temp))
                    {
                        db.Database.ExecuteSqlCommand("insert into bus_keyword(keyword) values(N'" + temp + "')");
                    }
                }
                for (i = 0; i < bus_to.Length; i++)
                {
                    temp = bus_to[i].Trim();
                    if (!db.bus_keyword.Any(o => o.keyword == temp))
                    {
                        db.Database.ExecuteSqlCommand("insert into bus_keyword(keyword) values(N'" + temp + "')");
                    }
                }
                return RedirectToAction("Index");
            }

            return View(find_bus);
        }

        // GET: XeBuyt/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            find_bus find_bus = db.find_bus.Find(id);
            if (find_bus == null)
            {
                return HttpNotFound();
            }
            return View(find_bus);
        }

        // POST: XeBuyt/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,bus_no,bus_des,bus_from,bus_to,bus_type,province")] find_bus find_bus)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                db.Entry(find_bus).State = EntityState.Modified;
                db.SaveChanges();
                string[] bus_from = find_bus.bus_from.Split('-');
                string[] bus_to = find_bus.bus_to.Split('-');
                int i = 0;
                string temp = "";
                for (i = 0; i < bus_from.Length; i++)
                {
                    temp = bus_from[i].Trim();
                    if (!db.bus_keyword.Any(o => o.keyword == temp))
                    {
                        db.Database.ExecuteSqlCommand("insert into bus_keyword(keyword) values(N'" + temp + "')");
                    }
                }
                for (i = 0; i < bus_to.Length; i++)
                {
                    temp = bus_to[i].Trim();
                    if (!db.bus_keyword.Any(o => o.keyword == temp))
                    {
                        db.Database.ExecuteSqlCommand("insert into bus_keyword(keyword) values(N'" + temp + "')");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(find_bus);
        }

        // GET: XeBuyt/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            find_bus find_bus = db.find_bus.Find(id);
            if (find_bus == null)
            {
                return HttpNotFound();
            }
            return View(find_bus);
        }

        // POST: XeBuyt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            find_bus find_bus = db.find_bus.Find(id);
            db.find_bus.Remove(find_bus);
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
