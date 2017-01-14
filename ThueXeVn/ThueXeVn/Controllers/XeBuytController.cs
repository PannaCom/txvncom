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
        public ActionResult Index(string sprovince, string sfrom, string sto, int? page)
        {
            if (sprovince == null) sprovince = "";
            if (sfrom == null) sfrom = "";
            if (sto == null) sto = "";
            var p = (from q in db.find_bus where q.province.Contains(sprovince) && q.bus_from.Contains(sfrom) && q.bus_to.Contains(sto) select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            ViewBag.sprovince = sprovince;
            ViewBag.sfrom = sfrom;
            ViewBag.sto = sto;
            return View(p.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Bus(string sprovince, string sfrom, string sto, int? page)
        {
            if (sprovince == null) sprovince = "";
            if (sfrom == null) sfrom = "";
            if (sto == null) sto = "";
            try { 
                var p = db.find_bus.Where(q=>q.province.Contains(sprovince) && q.bus_from.Contains(sfrom) && q.bus_to.Contains(sto)).FirstOrDefault();
                ViewBag.sprovince = sprovince;
                ViewBag.sfrom = sfrom;
                ViewBag.sto = sto;
                ViewBag.sno = p.bus_no;
                ViewBag.sdes = p.bus_des;
            }
            catch (Exception ex)
            {
                ViewBag.sprovince = "";
                ViewBag.sfrom = "";
                ViewBag.sto = "";
                ViewBag.sno = "";
                ViewBag.sdes = "";
            }
            if (page == null) page = 1;
            ViewBag.url = "http://thuexevn.com/XeBuyt/Bus?sprovince=" + sprovince + "&sfrom=" + sfrom + "&sto=" + sto;
            return View();
        }
        // GET: XeBuyt
        public ActionResult List(string sprovince,string sfrom, string sto,int? page)
        {
            if (sprovince == null) sprovince = "";
            if (sfrom == null) sfrom = "";
            if (sto == null) sto = "";
            var p = (from q in db.find_bus where q.province.Contains(sprovince) && q.bus_from.Contains(sfrom) && q.bus_to.Contains(sto) select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            if (page == null) page = 1;
            ViewBag.page = page;
            ViewBag.sprovince = sprovince;
            ViewBag.sfrom = sfrom;
            ViewBag.sto = sto;
            ViewBag.url = "http://thuexevn.com/XeBuyt/List?sprovince=" + sprovince + "&sfrom=" + sfrom + "&sto=" + sto + "&page=" + page;
            return View(p.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult TimBus(string sprovince, string sfrom, string sto, int? page)
        {
            if (sprovince == null) sprovince = "";
            if (sfrom == null) sfrom = "";
            if (sto == null) sto = "";
            var p = (from q in db.bus_all where q.sprovince.Contains(sprovince) && q.sfrom.Contains(sfrom) && q.sto.Contains(sto) select q).OrderByDescending(o => o.id).Take(1000000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            if (page == null) page = 1;
            ViewBag.page = page;
            ViewBag.sprovince = sprovince;
            ViewBag.sfrom = sfrom;
            ViewBag.sto = sto;
            ViewBag.url = "http://thuexevn.com/XeBuyt/TimBus?sprovince=" + sprovince + "&sfrom=" + sfrom + "&sto=" + sto + "&page=" + page;
            return View(p.ToPagedList(pageNumber, pageSize));
        }
        public string genAllBus()
        {
            try { 
                //Find max id
                long max_id = 0;
                try {
                    long lastId = db.bus_all.Max(o => o.id);
                    var tempBusAll=db.bus_all.Find(lastId);
                    var sprovince = tempBusAll.sprovince;
                    var sfrom = tempBusAll.sfrom;
                    var sto = tempBusAll.sto;
                    max_id = db.find_bus.Where(o => o.bus_from.Contains(sfrom) && o.bus_to.Contains(sto) && o.province == sprovince).FirstOrDefault().id;
                }
                catch (Exception sm)
                {
                    max_id = 0;
                }
                var p = (from q in db.find_bus where q.id > max_id select q).ToList();
                for (int i = 0; i < p.Count; i++) {
                    var find_bus = p[i];
                    string[] bus_from = find_bus.bus_from.Split('-');
                    string[] bus_to = find_bus.bus_to.Split('-');
                    int ii = 0;
                    int jj = 0;                    
                    for (ii = 0; ii < bus_from.Length; ii++)
                    {
                        for (jj = 0; jj < bus_to.Length; jj++)
                        {
                            string temp1 = bus_from[ii].Trim();
                            string temp2 = bus_to[jj].Trim();
                            string temp3 = find_bus.province.Trim();
                            if (!db.bus_all.Any(o => o.sfrom == temp1 && o.sto == temp2 && o.sprovince == temp3))
                            {
                                try { 
                                 db.Database.ExecuteSqlCommand("insert into bus_all(sfrom,sto,sprovince) values(N'" + temp1 + "',N'" + temp2 + "',N'" + temp3 + "')");
                                }
                                catch (Exception sm2)
                                {
                                    
                                }
                            }
                        }
                    }
                
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
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
