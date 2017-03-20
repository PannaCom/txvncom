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
    public class driversController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: drivers
        public ActionResult Index(string k,int? page)
        {
            if (k == null) k = "";
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            var p = (from q in db.drivers where q.name.Contains(k) || q.phone.Contains(k) || q.car_number.Contains(k) select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            if (page == null || page==0) page = 1;
            ViewBag.page = page;
            ViewBag.k = k;
            return View(p.ToPagedList(pageNumber, pageSize));
        }

        // GET: drivers/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // GET: drivers/Create
        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            return View();
        }

        // POST: drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,driver_type,phone,email,car_model,car_made,car_years,car_size,car_number,car_type,car_price,total_moneys,province,date_time,code,address")] driver driver)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                db.drivers.Add(driver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(driver);
        }

        // GET: drivers/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            string phone=driver.phone;
            //string car_number=driver.car_number;
            var p = db.list_online.Where(o => o.phone == phone).FirstOrDefault();
            try { 
                ViewBag.lon = p.lon;
                ViewBag.lat = p.lat;
            }
            catch (Exception ex)
            {
                ViewBag.lon = 105.3724793;
                ViewBag.lat = 20.9740874;
            }
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,driver_type,phone,email,car_model,car_made,car_years,car_size,car_number,car_type,car_price,total_moneys,province,date_time,code,address, lon, lat")] driver driver)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                db.Entry(driver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(driver);
        }

        //[HttpPost]
        //public void Edit(string name, string phone, string car_number, string car_made, string car_model, int? car_size, int car_year, string car_type, int? car_price, string address, double lon, double lat)
        //{
        //    if (Config.getCookie("logged") == "") return Response.Redirect("/Home/Login");
        //    try
        //    {
        //        driver r = new driver();
        //        r.name = name;
        //        r.phone = phone;
        //        r.car_number = car_number;
        //        r.car_made = car_made;
        //        r.car_model = car_model;
        //        r.car_size = car_size;
        //        r.car_years = car_year;
        //        r.car_type = car_type;
        //        r.car_price = car_price;
        //        r.address = address;
        //        //r.code = "1";
        //        db.drivers.Add(r);
        //        db.SaveChanges();
        //        list_online lo = new list_online();
        //        lo.car_number = car_number;
        //        lo.date_time = DateTime.Now;
        //        lo.geo = Config.CreatePoint(lat, lon);
        //        lo.lat = lat;
        //        lo.lon = lon;
        //        lo.phone = phone;
        //        lo.status = 0;
        //        db.list_online.Add(lo);
        //        db.SaveChanges();
        //        //lo.lon = lon;
        //        //lo.lat = lat;
        //        //lo.geo = Config.CreatePoint(lat, lon);
        //        //Config.mail("muabanraovat63@gmail.com", "vnnvh80@gmail.com", "Tài xế đăng ký " + phone, "Huynguyenviet1", "Họ tên: " + name + ", số điện thoại " + phone + ", Biển số xe:" + car_number + ", Thông tin xe: " + car_made + "," + car_model + ", số chỗ " + car_size + ", năm sản xuất " + car_year + ", Địa chỉ " + address);

        //        return "1";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "0";
        //    }
        //}

        // GET: drivers/Delete/5
        public ActionResult Delete(long? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            driver driver = db.drivers.Find(id);
            db.drivers.Remove(driver);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region module quản trị nộp tiền khởi tạo tài khoản cho tài xế - Nguyen Van Nam
        public string getModalInitDriver(long? id)
        {
            string html = "";
            var driver = db.drivers.Find(id);
            if (driver == null)
            {
                html = "<p>Không tìm thấy tài xế để nộp tiền</p>";
            }
            else
            {
                html += "<form class=\"form-horizontal\" method=\"post\" id=\"form_init_acc_driver\" name=\"form_init_acc_driver\" enctype=\"multipart/form-data\">"
                   + "<h4>Tài xế: " + driver.name +"</h4>"
                   + "<input type=\"hidden\" name=\"driver_id\" id=\"driver_id\" value=\""+ driver.id + "\" />"
                   + "<div class=\"form-group\">"
                   + "<div class=\"col-md-12\">"
                   + "<label class=\"control-label\">Số tiền tài xế nộp: </label>"
                   + "<input type=\"number\" name=\"total_money\" id=\"total_money\" class=\"form-control\" placeholder=\"Số tiền tài xế nộp\" />"
                   + "</div>"
                   + "</div>"
                   + "<button type=\"button\" class=\"btn btn-info\" id=\"btn_noptien\" onclick=\"noptientodriver("+ id +");\">Nộp tiền</button>"
                   + "</form>";

                //html += "<script>$(document).ready(function() {"
                //        + "$(\"#form_init_acc_driver\").submit(function(e) {"
                //        + "var url = \"/drivers/adddrivermoney\"; // the script where you handle the form input."
                //        + "if ($('#total_money').val() !== \"\") {"
                //        + "$.ajax({type: \"POST\", url: url, data: $(\"#form_init_acc_driver\").serialize(),"
                //        + "success: function(data){ if (data === 1) { alert('Đã nộp tiền tạo tài khoản cho tài xế.');"
                //        + "console.log($(this).html());} console.log(data);}});}else {alert(\"Vui lòng nhập số tiền tạo tài khoản nộp.\");}e.preventDefault();});})</script>";
            }
            
            return html;
        }

        [HttpPost]
        public ActionResult adddrivermoney(long? driver_id, int? total_money)
        {
            int _saved = 0;
            var _taixent = db.drivers_money.Where(x => x.driver_id == driver_id).FirstOrDefault();
            // chưa nộp thì thêm mới
            try
            {
                if (_taixent == null)
                {
                    drivers_money addmoney = new drivers_money();
                    addmoney.driver_id = driver_id ?? null;
                    addmoney.total_money = total_money ?? null;
                    addmoney.status = 1;
                    addmoney.date_pay = DateTime.Now;
                    db.drivers_money.Add(addmoney);
                }
                else
                {
                    // đã nộp thì cập nhật
                    _taixent.total_money = total_money ?? null;
                    db.Entry(_taixent).State = EntityState.Modified;
                }                
                db.SaveChanges();
                _saved = 1;
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }          
            
            return Json(_saved, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _checkInitmoney(long? id)
        {
            string danop = "Chưa nộp tiền";
            if (id != null)
            {
                var _checked = db.drivers_money.Where(x => x.driver_id == id).FirstOrDefault();
                if (_checked != null)
                {
                    if (_checked.total_money > 0)
                    {
                        danop = "Đã nộp: " + _checked.total_money + " đồng.";
                    }
                    
                }
            }            
            return PartialView("_checkInitmoney", danop);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult LoadStatusListOnline(string phone)
        {
            var data = db.list_online.Where(x => x.phone == phone).Select(x => x.status).FirstOrDefault().ToString();
            return PartialView("_LoadStatusListOnline", data);
        }

        [HttpPost]
        public string kichhoatdriver(string id)
        {
            try
            {
                if (Config.getCookie("logged") == "") return "0";
                db.Database.ExecuteSqlCommand("update list_online set status=0 where phone=N'" + id + "'");
                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }

        }

        [HttpPost]
        public string tamdungdriver(string id)
        {
            try
            {
                if (Config.getCookie("logged") == "") return "0";
                db.Database.ExecuteSqlCommand("update list_online set status=1 where phone=N'" + id + "'");
                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }

        }

        
    }
}
