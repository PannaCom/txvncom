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
namespace ThueXeVn.Controllers
{
    public class codeController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: code
        public ActionResult Index(int? page,string k,int? type)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (k == null) k = "";
            var p = (from q in db.activecodes where (q.code.Contains(k) || q.phone.Contains(k)) select q);
            if (type != null) p = p.Where(o => o.type_code == type);
            p=p.OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            ViewBag.keyword = k;
            int total = db.activecodes.Count();
            ViewBag.total = total;
            int c30t = db.activecodes.Where(o => o.type_code == 30).Count();
            int c30 = db.activecodes.Where(o => o.type_code == 30 && o.phone!=null).Count();
            ViewBag.c30="đã dùng "+c30+"/"+c30t;
            int c90t = db.activecodes.Where(o => o.type_code == 90).Count();
            int c90 = db.activecodes.Where(o => o.type_code == 90 && o.phone != null).Count();
            ViewBag.c90="đã dùng "+c90+"/"+c90t;
            int c180t = db.activecodes.Where(o => o.type_code == 180).Count();
            int c180 = db.activecodes.Where(o => o.type_code == 180 && o.phone != null).Count();
            ViewBag.c180="đã dùng "+c180+"/"+c180t;
            int c365t = db.activecodes.Where(o => o.type_code == 365).Count();
            int c365 = db.activecodes.Where(o => o.type_code == 365 && o.phone != null).Count();
            ViewBag.c365="đã dùng "+c365+"/"+c365t;
            return View(p.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public string generateCode(int type){
            if (Config.getCookie("logged") == "") return "";
            try
            {
                string code = "";
                for (int i = 0; i < 1000; i++)
                {
                    code = Config.randomcode().ToString();
                    activecode ac = new activecode();
                    ac.code = code;
                    ac.type_code = type;
                    db.activecodes.Add(ac);
                    db.SaveChanges();
                }
                return "1";
            }catch(Exception ex){
                return "0";
            }
            return "0";
        }
        // GET: code/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            activecode activecode = db.activecodes.Find(id);
            if (activecode == null)
            {
                return HttpNotFound();
            }
            return View(activecode);
        }

        // GET: code/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: code/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,phone,code,type_code")] activecode activecode)
        {
            if (ModelState.IsValid)
            {
                db.activecodes.Add(activecode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(activecode);
        }

        // GET: code/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            activecode activecode = db.activecodes.Find(id);
            if (activecode == null)
            {
                return HttpNotFound();
            }
            return View(activecode);
        }

        // POST: code/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,phone,code,type_code")] activecode activecode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activecode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(activecode);
        }

        // GET: code/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            activecode activecode = db.activecodes.Find(id);
            if (activecode == null)
            {
                return HttpNotFound();
            }
            return View(activecode);
        }

        // POST: code/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            activecode activecode = db.activecodes.Find(id);
            db.activecodes.Remove(activecode);
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
