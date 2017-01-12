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
    public class bookingsController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: bookings
        public ActionResult Index(string k, int? page)
        {
            if (k == null) k = "";
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            var p = (from q in db.bookings where q.name.Contains(k) || q.phone.Contains(k) select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            ViewBag.k = k;
            return View(p.ToPagedList(pageNumber, pageSize));
        }

        // GET: bookings/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: bookings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,phone,email,date_from,date_to,des,driver_phone,status,date_time,car_from,car_to,car_type,car_hire_type,car_size,lon1,lat1,lon2,lat2")] booking booking)
        {
            if (ModelState.IsValid)
            {
                db.bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(booking);
        }

        // GET: bookings/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,phone,email,date_from,date_to,des,driver_phone,status,date_time,car_from,car_to,car_type,car_hire_type,car_size,lon1,lat1,lon2,lat2")] booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        // GET: bookings/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            booking booking = db.bookings.Find(id);
            db.bookings.Remove(booking);
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

        //updatedatxe
        public string updatedatxe(int id_booking, int trangthai)
        {
            try
            {
                // Chỉ cập nhật nếu booking khác trạng thái là 3 - đã đặt thành công
                int status2 = 0;
                try
                {
                    status2 = db.Database.SqlQuery<int>("select status2 from booking where id =" + id_booking).FirstOrDefault();
                }
                catch (Exception)
                {
                    status2 = 0;
                }                
                if (status2 != 3)
                {
                    string sql = "update booking set status2=" + trangthai + " where id=" + id_booking;
                    db.Database.ExecuteSqlCommand(sql);
                    try
                    {
                        if (trangthai == 3)
                        {
                            var _b1 = db.booking_ctv_tiepthi.Where(x => x.booking_id == id_booking).FirstOrDefault();
                            if (_b1 != null)
                            {
                                var _cid1 = _b1.ctv_id;
                                var _ctv = db.ctv_tiepthi.Find(_cid1);
                                if (_ctv != null)
                                {
                                    db.Entry(_ctv).State = EntityState.Modified;
                                    _ctv.point_share = _ctv.point_share == null ? 1 : _ctv.point_share + 1;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                    catch
                    {
                        return "0";
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

    }
}
