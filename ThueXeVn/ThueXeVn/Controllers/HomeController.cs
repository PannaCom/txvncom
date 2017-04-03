using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ThueXeVn.Models;
using PagedList;
using System.Data.Entity;
using System.Device.Location;
using PagedList.Mvc;
using System.IO;
using System.Web.Configuration;

namespace ThueXeVn.Controllers
{
    public class HomeController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();
        public ActionResult Index()
        {
            //var p = (from q in db.news select q).OrderByDescending(o => o.id).Take(3);
            //ViewBag.news = p.ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Customer()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Driver()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Price()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Booking()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Register()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public string CheckLogin(string name, string pass)
        {
            MD5 md5Hash = MD5.Create();
            pass = Config.GetMd5Hash(md5Hash, pass);
            var p = (from q in db.users where q.name.Contains(name) && q.pass.Contains(pass) select q.name).SingleOrDefault();
            if (p != null && p != "")
            {
                //Ghi ra cookie
                Config.setCookie("logged", "logged");
                return "1";
            }
            else
            {
                return "0";
            }
           // return "0";
        }

        // LogOff
        public ActionResult LogOff()
        {
            Response.Cookies["logged"].Expires = DateTime.Now.AddDays(-1);   
            return RedirectToAction("Index");            
        }

        public ActionResult Login()
        {
            //ViewBag.Message = "Your application description page.";
            if (Config.getCookie("logged") != "") return Redirect("/drivers/index");
            return View();
        }

        public ActionResult CarRental()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        [HttpPost]
        public string AddNew(string name,string phone,string email,DateTime? date_from,string des)
        {
            try {
                booking b = new booking();
                b.name = name;
                b.phone = phone;
                b.email = email;
                b.date_from = date_from;
                b.des = des;
                b.status = 0;
                b.date_time = DateTime.Now;
                db.bookings.Add(b);
                db.SaveChanges();
                Config.mail("muabanraovat63@gmail.com", "vnnvh80@gmail.com", "Khách đặt xe " + phone, "Huynguyenviet1", "Họ tên: " + name + ", số điện thoại " + phone + ", ghi chú:" + des);
                return "1";
            }
            catch (Exception ex) { 
                return "0";
            }
        }

        [HttpPost]
        public JsonResult EmailDriverExists(string strEmail, long? lgId)
        {
            if (lgId != null)
            {
                if (db.drivers.Any(x => x.email == strEmail))
                {
                    driver existingTabIdea = db.drivers.Single(x => x.email == strEmail);
                    if (existingTabIdea.id != lgId)
                    {
                        return Json(false);
                    }
                    else
                    {
                        return Json(true);
                    }
                }
                else
                {
                    return Json(true);
                }
            }
            else
            {
                return Json(!db.drivers.Any(x => x.email == strEmail));
            }
        }

        [HttpPost]
        public string Register(int? id, string name, int? driver_type, string email, string phone, string car_number, string car_made, string car_model, int? car_size, int? car_year, string car_type, int? car_price, string address, double? lon, double? lat, string password_tx)
        {
            try
            {
                if (lon == null) lon = 105.8341597;
                if (lat == null) lat = 21.0277644;
                if (id == null || id==-1)
                { 
                    driver r = new driver();
                    r.name = name ?? null;
                    r.driver_type = driver_type ?? null;
                    r.email = email ?? null;
                    r.phone = phone ?? null;
                    r.car_number = car_number ?? null;
                    r.car_made = car_made ?? null;
                    r.car_model = car_model ?? null;
                    r.car_size = car_size ?? null;
                    r.car_years = car_year ?? null;
                    r.car_type = car_type ?? null;
                    r.car_price = car_price ?? null;
                    r.address = address ?? null;
                    r.date_time = DateTime.Now;
                    r.code = "1";
                    r.lon = lon ?? null;
                    r.lat = lat ?? null;
                    r.geo = Config.CreatePoint(lat, lon);
                    //r.code = "1";
                    MD5 md5Hash = MD5.Create();
                    if (password_tx == null || password_tx == "")
                    {
                        password_tx = "chanhniem";
                    }
                    var pass = Config.GetMd5Hash(md5Hash, password_tx);
                    r.pass = pass;

                    db.drivers.Add(r);
                    db.SaveChanges();
                    list_online lo = new list_online();
                    lo.car_number = car_number ?? null;
                    lo.date_time = DateTime.Now;
                    lo.geo=Config.CreatePoint(lat, lon);
                    lo.lat = lat;
                    lo.lon = lon;
                    lo.phone = phone;
                    lo.status = 0;
                    
                    db.list_online.Add(lo);
                    db.SaveChanges();
                }
                else {
                    driver r = db.drivers.Find(id);
                    db.Entry(r).State = EntityState.Modified;
                    r.name = name ?? null;
                    r.driver_type = driver_type ?? null;
                    r.email = email ?? null;
                    r.phone = phone ?? null;
                    r.car_number = car_number ?? null;
                    r.car_made = car_made ?? null;
                    r.car_model = car_model ?? null;
                    r.car_size = car_size ?? null;
                    r.car_years = car_year ?? null;
                    r.car_type = car_type ?? null;
                    r.car_price = car_price ?? null;
                    r.address = address ?? null;
                    r.lon = lon;
                    r.lat = lat;
                    r.geo = Config.CreatePoint(lat, lon);
                    if (r.pass == password_tx)
                    {
                        db.SaveChanges();
                    }
                    else
                    {
                        MD5 md5Hash = MD5.Create();
                        if (password_tx == null || password_tx == "")
                        {
                            password_tx = "chanhniem";
                        }
                        var pass = Config.GetMd5Hash(md5Hash, password_tx);
                        r.pass = pass;
                        db.SaveChanges();
                    }

                    //string strsql = "update list_online set lon=" + lon + ",lat=" + lat + ",geo = '" + r.geo + "' where phone=N'" + phone + "' and car_number=N'" + car_number + "'";
                    //db.Database.ExecuteSqlCommand(strsql);
                    var lo = db.list_online.Where(x => x.phone == phone && x.car_number == car_number).FirstOrDefault();
                    if (lo != null)
                    {
                        lo.car_number = car_number ?? null;
                        lo.date_time = DateTime.Now;
                        lo.geo = Config.CreatePoint(lat, lon);
                        lo.lat = lat ?? null;
                        lo.lon = lon ?? null;
                        lo.phone = phone ?? null;
                        lo.status = 0;
                        db.Entry(lo).State = EntityState.Modified;
                        db.SaveChanges();
                    }                  
                                        
                }
                //lo.lon = lon;
                //lo.lat = lat;
                //lo.geo = Config.CreatePoint(lat, lon);
                //Config.mail("muabanraovat63@gmail.com", "vnnvh80@gmail.com", "Tài xế đăng ký " + phone, "Huynguyenviet1", "Họ tên: " + name + ", số điện thoại " + phone + ", Biển số xe:" + car_number + ", Thông tin xe: " + car_made + "," + car_model + ", số chỗ " + car_size + ", năm sản xuất " + car_year+", Địa chỉ "+address);

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public string getTinhThanh()
        {
            var p = (from q in db.TinhThanhs orderby q.tinhthanhpho select q.tinhthanhpho).Distinct().ToList();
            string tinhthanh = "";
            for (int i = 0; i < p.Count; i++)
            {
                tinhthanh += "<option value=\"" + p[i]+ "\">"+p[i]+"</option>";
            }
            return tinhthanh;
        }
        public string getHangXe()
        {
            var p = (from q in db.list_car select q).OrderBy(o=>o.no).Distinct().ToList();
            string hx = "";
            for (int i = 0; i < p.Count; i++)
            {
                hx += "<option value=\"" + p[i].name + "\">" + p[i].name + "</option>";
            }
            return hx;
        }
        public string getLoaiXe()
        {
            var p = (from q in db.list_car_type select q).OrderBy(o => o.id).Distinct().ToList();
            string lx = "";
            for (int i = 0; i < p.Count; i++)
            {
                lx += "<option value=\"" + p[i].name + "\">" + p[i].name + "</option>";
            }
            return lx;
        }
        public string getQuanHuyen(string keyword)
        {
            var p = (from q in db.TinhThanhs where q.tinhthanhpho.Contains(keyword) orderby q.quanhuyen select q.quanhuyen).Distinct().ToList();
            string tinhthanh = "";
            for (int i = 0; i < p.Count; i++)
            {
                tinhthanh += "<option value=\"" + p[i] + "\">" + p[i] + "</option>";
            }
            return tinhthanh;
        }
        public string generateSiteMap()
        {

            try
            {

                XmlWriterSettings settings = null;
                string xmlDoc = null;

                settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                xmlDoc = HttpRuntime.AppDomainAppPath + "sitemap.xml";//HttpContext.Server.MapPath("../") + 
                float percent = 0.85f;

                string urllink = "";
                using (XmlTextWriter writer = new XmlTextWriter(xmlDoc, Encoding.UTF8))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("urlset");
                    writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://thuexevn.com");
                    writer.WriteElementString("changefreq", "always");
                    writer.WriteElementString("priority", "1");
                    writer.WriteEndElement();
            
                    //tài xế
                   
                    var p4= (from q in db.drivers select q).OrderByDescending(o => o.id).ToList();
                    for (int i = 0; i < p4.Count; i++)
                    {
                        try
                        {

                            writer.WriteStartElement("url");
                            urllink = "http://thuexevn.com/Drivers/Details?id=" + p4[i].id;
                            writer.WriteElementString("loc", urllink);
                            writer.WriteElementString("changefreq", "hourly");
                            percent = 0.70f;
                            writer.WriteElementString("priority", "0.98");
                            writer.WriteEndElement();
                        }
                        catch (Exception ex4)
                        {
                        }
                    }
                   

                    //Xe Buýt
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://thuexevn.com/tin/page=1");
                    writer.WriteElementString("changefreq", "always");
                    writer.WriteElementString("priority", "0.97");
                    writer.WriteEndElement();

                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://thuexevn.com/XeBuyt/List");
                    writer.WriteElementString("changefreq", "always");
                    writer.WriteElementString("priority", "0.97");
                    writer.WriteEndElement();

                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://thuexevn.com/XeBuyt/TimBus");
                    writer.WriteElementString("changefreq", "always");
                    writer.WriteElementString("priority", "0.97");
                    writer.WriteEndElement();


                    var p3 = (from q in db.news select q).OrderByDescending(o => o.id).ToList();
                    for (int i = 0; i < p3.Count; i++)
                    {
                        try
                        {

                            writer.WriteStartElement("url");
                            urllink = "http://thuexevn.com/tin/" + Config.unicodeToNoMark(p3[i].title) + "-" + p3[i].id;
                            writer.WriteElementString("loc", urllink);
                            writer.WriteElementString("changefreq", "hourly");
                            percent = 0.70f;
                            writer.WriteElementString("priority", percent.ToString("0.00"));
                            writer.WriteEndElement();
                        }
                        catch (Exception ex2)
                        {
                        }
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

            }
            catch (Exception extry)
            {
                //StreamWriter sw = new StreamWriter();
            }
            return "ok";
        }
        public class glol
        {
            public long id { get; set; }
            public double lon { get; set; }
            public double lat { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public string car_model { get; set; }
            public int? car_size { get; set; }
            public string car_type { get; set; }
            public string car_made { get; set; }
            public int? car_price { get; set; }
            public double D { get; set; }
        }
        public ActionResult Search(double lon, double lat, string address,string car_type, string car_made, string car_model, int? car_size, int? order)
        {
            string query = "select top 200 id,lon,lat,name,phone,car_model,car_size,car_type,car_made,car_price,GETDATE() as datetime,D from ";
            query += " (select id,name,phone,car_model,car_size,car_type,car_made,car_price,car_number from drivers where code=N'1') as A left join (select car_number,phone as phone2,lon,lat,status,ACOS(SIN(PI()*" + lat + "/180.0)*SIN(PI()*lat/180.0)+COS(PI()*" + lat + "/180.0)*COS(PI()*lat/180.0)*COS(PI()*lon/180.0-PI()*" + lon + "/180.0))*6371 As D from list_online where status=0) as B on A.phone=B.phone2 and A.car_number=B.car_number where D<1000 ";

            if (car_type != null && car_type != "" && car_type != "\"\"")
            {
                query += " and (car_type=N'" + car_type + "') ";
            }
            if (car_made != null && car_made != "" && car_made != "\"\"")
            {
                query += " and (car_made=N'" + car_made + "') ";
            }
            if (car_model != null && car_model != "" && car_model != "\"\"")
            {
                query += " and (car_model like N'%" + car_model + "%') ";
            }
            if (car_size != null && car_size > 0)
            {
                query += " and (car_size=" + car_size + ") ";
            }
            if (order == null || order == 0)
            {
                query += " order by d";
            }
            else
            {
                query += " order by car_price";
            }
            ViewBag.lon = lon;
            ViewBag.lat = lat;
            ViewBag.cartype = car_type;
            ViewBag.carsize= car_size;
            ViewBag.address=address;
            var p = db.Database.SqlQuery<glol>(query);
            int pageSize = 25;
            int pageNumber = 1;
            return View(p.ToPagedList(pageNumber, pageSize)); 
        }
        public string updateCarNumber()
        {
            var p = (from q in db.list_online select q).ToList();
            for (int i = 0; i < p.Count; i++)
            {
                db.Database.ExecuteSqlCommand("update drivers set car_number=N'" + p[i].car_number + "' where phone=N'" + p[i].phone + "'");
            }
            return "1";
        }

        // List gioi thieu new
        public ActionResult LoadNewTop()
        {
            var data = (from s in db.news orderby s.datetime descending select s).ToList().Take(10).ToList();
            return PartialView("_LoadNewTop", data);
        }

        public ActionResult LoadBooking()
        {
            var taixe = Config.getCookie("taixelogged");
            var query = "select * from booking";
            var data = db.Database.SqlQuery<booking>(query).ToList();            
            //if (taixe == "")
            //{
            //    data = data.Select(x => new booking()
            //    {
            //        id = x.id,
            //        car_from = x.car_from,
            //        car_to = x.car_to,
            //        car_type = x.car_type,
            //        car_hire_type = x.car_hire_type,
            //        name = x.name,
            //        date_from = x.date_from,
            //        date_to = x.date_to,
            //        phone = x.status == 1 ? "<span>Khách đã đặt thành công</span>" : "<a class='show_pn' href='#'>Số điện thoại</a>"
            //    }).ToList().OrderByDescending(s=>s.id).ToList();
            //}
            //else
            //{
            //    data = data.Select(x => new booking()
            //    {
            //        id = x.id,
            //        car_from = x.car_from,
            //        car_to = x.car_to,
            //        car_type = x.car_type,
            //        car_hire_type = x.car_hire_type,
            //        name = x.name,
            //        date_from = x.date_from,
            //        date_to = x.date_to,
            //        phone = x.status == 1 ? "<span>Khách đã đặt thành công</span>" : "<a class='phone' href='tel:" + x.phone + "'>" + x.phone + "</a>"
            //    }).ToList().OrderByDescending(s => s.id).ToList();    
            //}

            return PartialView("_LoadBooking", data);
        }

        public ActionResult Taixe()
        {
            if (Config.getCookie("taixeupdatepass") != "") return RedirectToRoute("taixedoipass");
            return View();
        }

        // Đăng nhập bằng mật khẩu
        public ActionResult LoginTaiXe()
        {
            if (Config.getCookie("taixelogged") != "") return RedirectToRoute("quanlybanggia");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginTaiXe(string phone, string pass_taixe)
        {
            try
            {
                MD5 md5Hash = MD5.Create();
                var epass = Config.GetMd5Hash(md5Hash, pass_taixe);
                var login = (from t in db.drivers where t.phone == phone && t.pass == epass select t).FirstOrDefault();
                if (login == null)
	            {
                    ModelState.AddModelError("", "Sai thông tin đăng nhập.");
		            return View();
	            }
                var name_login = login.phone+login.id+login.name;
                Config.setCookie("taixelogged", Config.GetMd5Hash(md5Hash, name_login) + "," + login.id);
                Config.setCookie("mvname", login.name);
                Config.setCookie("mvtype", login.driver_type == 0 ? "Tài xế" : "Nhà xe");
            }
            catch(Exception ex) 
            {
                Config.SaveTolog(ex.ToString());
                ModelState.AddModelError("", "Có lỗi xảy ra khi đăng nhập.");
                return View(); 
            }
            return RedirectToRoute("quanlybanggia");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Taixe(string phone, string car_number)
        {
            //if (Config.getCookie("taixeupdatepass") != "") return RedirectToRoute("taixedoipass");
            try
            {
                MD5 md5Hash = MD5.Create();
                var p = (from q in db.drivers where q.phone.Contains(phone) && q.car_number.Contains(car_number) select q).FirstOrDefault();
                if (p != null)
                {
                    var _tx = Config.GetMd5Hash(md5Hash, p.phone);
                    //Ghi ra cookie
                    Config.setCookie("taixeupdatepass", _tx + "-" + p.id);
                    ViewBag.idtaixe = p.id;
                }
                else
                {
                    ModelState.AddModelError("", "Sai thông tin đăng nhập");
                    return View();
                }

            }
            catch(Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                ModelState.AddModelError("", "Sai thông tin đăng nhập");
                return View();
            }
            return RedirectToRoute("taixedoipass");
        }

        public ActionResult UpdatePass()
        {
            if (Config.getCookie("taixeupdatepass") == "") return RedirectToAction("Taixe");
            if (Config.getCookie("taixelogged") != "") return RedirectToRoute("quanlybanggia");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePass(UpdatePassTaiXe model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại các trường.");
                return View(model);
            }
            var taixeupdate = db.drivers.Find(model.id_taixe);
            if (taixeupdate == null)
            {
                return View(taixeupdate);
            }
            try
            {
                db.Entry(taixeupdate).State = EntityState.Modified;
                string epass = model.NewPassword ?? "chanhniem";
                MD5 md5Hash = MD5.Create();
                var newpass = Config.GetMd5Hash(md5Hash, epass);
                taixeupdate.pass = newpass;
                db.SaveChanges();
                Config.RemoveCookie("taixeupdatepass");
            }
            catch(Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                return View(taixeupdate);
            }
            return RedirectToRoute("taixedangnhap");
        }

        public string getcarnumber(string keyword)
        {
            if (keyword == null) keyword = "";
            var p = (from q in db.drivers where q.car_number.Contains(keyword) orderby q.car_number ascending select q.car_number).ToList().Distinct();
            return JsonConvert.SerializeObject(p);
        }

        public ActionResult logouttaixe()
        {
            Config.RemoveCookie("taixelogged");
            Config.RemoveCookie("mvname");
            Config.RemoveCookie("mvtype");
            return RedirectToAction("Index");
        }

        public ActionResult InfoTaixelogin()
        {
            var d = Config.getCookie("taixelogged") != "" ? Config.getCookie("taixelogged").Split(',')[1] : "";
            var idtx = Convert.ToInt32(d);
            string strtx = "";
            var tx = (from s in db.drivers where s.id == idtx select s.name).FirstOrDefault();
            if (tx != null) strtx = tx;            
            return PartialView("_menuLoginTaiXe", strtx);
        }

        public ActionResult banggiaxedanang()
        {
            return View();
        }

        public ActionResult banggiaxedn1()
        {
            var data = (from s in db.car_price_da_nang select s).ToList();
            return PartialView("_banggiaxedn1", data);
        }

        public ActionResult banggiaxedn2()
        {
            var data = (from s in db.car_price_da_nang select s).ToList();
            return PartialView("_banggiaxedn2", data);
        }

        public ActionResult setcookieshowbg()
        {
            Config.setCookie("showgb", "showbgtxvn");
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public ActionResult banggiaxe(string lat1, string lng1, string lat2, string lng2, string from, string to, string loaixe, string tuyen, string kc)
        {
            if (loaixe == null && tuyen == null)
            {
                return View();
            }
            string banggiaxesql = "SELECT g8 as gialuudem," + tuyen + " as giaxe FROM pl_car_price where pl_car_type = " + loaixe;
            var banggiaxedata = db.Database.SqlQuery<getbanggia1>(banggiaxesql).FirstOrDefault();
            if (banggiaxedata == null)
            {
                return View(banggiaxedata);
            }
            var giaxe_1 = banggiaxedata.giaxe;
            if (kc != null && kc != "")
	        {
                int ikc = 1;
                try
                {
                   ikc = Convert.ToInt32(kc);
                }
                catch 
                {
                }
                if (tuyen == "g6" || tuyen == "g7")
                {
                    if (tuyen == "g6")
                    {
                        giaxe_1 *= 2 * ikc;
                    }
                    else
                    {
                        giaxe_1 *= ikc;
                    }
                }
	        }
            
            var data = new timbanggia()
            {
                from = from ?? "",
                to = to ?? "",
                gia = string.Format("{0:#,###} đồng", giaxe_1),
                gialuudem = string.Format("{0:#,###} đồng/đêm", banggiaxedata.gialuudem),
                lat1 = lat1 ?? "",
                lat2 = lat2 ?? "",
                lng1 = lng1 ?? "",
                lng2 = lng2 ?? "",
                loaixe = loaixe,
                tuyen = tuyen,
                kc = kc ?? ""
            };
            return View(data);
        }

        public ActionResult XeTaxiNoiBai()
        {
            ViewBag.menuleft = getProjectMenu();
            return View();
        }

        public string getProjectMenu()
        {
            string menuleft = "";
            try
            {
                //Lấy ra menu bên trái
                var mn = (from p in db.news
                          select new
                          {
                              image = p.image,
                              title = p.title,
                              des = p.des,
                              id = p.id,
                              datetime = p.datetime,
                          }).OrderByDescending(o => o.id).Take(10).ToList();


                string link = "";

                for (int i = 0; i < mn.Count; i++)
                {

                    link = "/tin/" + Config.unicodeToNoMark(mn[i].title) + "-" + mn[i].id;
                    string style = "style=\"display:block;\"";
                    menuleft += "<div " + style + ">&nbsp;&nbsp;-<a href=\"" + link + "\">" + mn[i].title + "</a></div>";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            return menuleft;
        }

        // Đăng ký cộng tác viên
        public ActionResult DangKyCongTacVien()
        {
            if (Config.getCookie("ctvlogged") != "") return RedirectToRoute("congtacvienquantri");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKyCongTacVien(string fullname, string ctv_email, string ctv_phone, string ctv_pass)
        {
            try
            {
                ctv_tiepthi _newCTV = new ctv_tiepthi();
                _newCTV.ctv_fullname = fullname ?? null;
                _newCTV.ctv_email = ctv_email ?? null;
                _newCTV.ctv_phone = ctv_phone ?? null;
                string epass = ctv_pass ?? "chanhniem";
                MD5 md5Hash = MD5.Create();
                var newpass = Config.GetMd5Hash(md5Hash, epass);
                _newCTV.ctv_pass = newpass;
                _newCTV.date_create = DateTime.Now;
                _newCTV.status = true;
                db.ctv_tiepthi.Add(_newCTV);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                ModelState.AddModelError("", "Máy chủ quá tải, vui lòng chờ 15 phút sau quay lại đăng ký.");
                return View();
            }
            return RedirectToRoute("congtacviendangnhap");
        }

        public ActionResult DangNhapCongTacVien()
        {
            if (Config.getCookie("ctvlogged") != "") return RedirectToRoute("congtacvienquantri");
            return View();
        }

        [HttpPost]
        public ActionResult DangNhapCongTacVien(string ctv_email, string ctv_pass)
        {
            try
            {
                string epass = ctv_pass ?? "chanhniem";
                MD5 md5Hash = MD5.Create();
                var newpass = Config.GetMd5Hash(md5Hash, epass);

                var ctvlogin = (from s in db.ctv_tiepthi where s.ctv_email == ctv_email && s.ctv_pass == newpass select s).FirstOrDefault();
                if (ctvlogin == null)
                {
                    ModelState.AddModelError("", "Sai thông tin đăng nhập.");
                    return View();
                }
                if (ctvlogin.status == false)
                {
                     ModelState.AddModelError("", "Tài khoản đang bị khóa.");
                     return View();
                }
                
                var logged = Config.GetMd5Hash(md5Hash, ctvlogin.ctv_phone);
                //Ghi ra cookie
                Config.setCookie("ctvlogged", logged+"-"+ctvlogin.ctv_id);
            }
            catch(Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                ModelState.AddModelError("", "Máy chủ đang quá tải. Vui lòng quay lại đăng nhập sau 10 phút.");
                return View();
            }
            return RedirectToRoute("congtacvienquantri");            
        }

        public ActionResult ctv_dang_xuat()
        {
            Config.RemoveCookie("ctvlogged");
            return RedirectToAction("Index");
        }

        public bool IsEmailExist(string Email)
        {
            using (var db = new thuexevnEntities())
            {
                var emodel = db.ctv_tiepthi.Any(x => x.ctv_email == Email);
                return emodel;
            }
        }

        public bool IsPhoneExist(string phone)
        {
            using (var db = new thuexevnEntities())
            {
                var emodel = db.ctv_tiepthi.Any(x => x.ctv_phone == phone);
                return emodel;
            }
        }

        public string checkemailctv(string key){
            if (key == null) { key = ""; }
            var x = "0";
            try
            {
                if (IsEmailExist(key))
                {
                    x = "1";
                }
                return x;
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                return "0";
            }
        }

        public string checkphonectv(string key)
        {
            if (key == null) { key = ""; }
            var x = "0";
            try
            {
                if (IsPhoneExist(key))
                {
                    x = "1";
                }
                return x;
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                return "0";
            }
        }

        [HttpPost]
        public string booking(string car_from, string car_to, string car_type, string car_hire_type, int? car_size, DateTime from_datetime, DateTime to_datetime, double? lon1, double? lat1, double? lon2, double? lat2, string name, string phone, string utm)
        {
            try
            {
                booking bo = new booking();
                bo.car_from = car_from;
                bo.car_to = car_to;
                bo.car_type = car_type;
                bo.car_hire_type = car_hire_type;
                bo.car_size = car_size;
                bo.date_from = from_datetime;
                bo.date_to = to_datetime;
                bo.lon1 = lon1;
                bo.lat1 = lat1;
                bo.lon2 = lon2;
                bo.lat2 = lat2;
                bo.name = name;
                bo.phone = phone;
                bo.date_time = DateTime.Now;
                bo.status = 0;
                bo.status2 = 0;
                db.bookings.Add(bo);
                db.SaveChanges();

                if (utm != null && utm != "" && utm != "null")
                {
                    long book_id = bo.id;
                    var _ctv = (from c in db.ctv_tiepthi where c.ctv_phone == utm select c).FirstOrDefault();
                    if (_ctv != null)
                    {
                        try
                        {
                            string sql = "INSERT INTO booking_ctv_tiepthi(booking_id,ctv_id,utm_code) VALUES(" + book_id + "," + _ctv.ctv_id + ",'" + utm + "')";
                            db.Database.ExecuteSqlCommand(sql);
                        }
                        catch (Exception ex)
                        {
                            Config.SaveTolog(ex.ToString());
                        }
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public ActionResult banggiaxe1()
        {
            var data = db.pl_car_price.ToList();
            return PartialView("_banggiaxe1", data);
        }

        public ActionResult LoadPromotionDriver(long? id)
        {
            string html = "";
            var data = db.driver_promotion.Where(x => x.driver_id == id && x.status == true).Select(x => x).FirstOrDefault();
            if (data != null)
	        {
                html += data.des;
	        }
            return PartialView("_LoadPromotionDriver", html);
        }

        public ActionResult LoadViewDriver(long? id)
        {
            int? count = 0;
            //throw new Exception("loi");
            try
            {
                var viewer = db.driver_view.Where(x => x.driver_id == id).FirstOrDefault();
                if (viewer != null)
                {
                    count = viewer.views;
                }
            }
            catch
            {                
            }
            
            return PartialView("_LoadViewDriver", count);
        }

        public ActionResult TimTaiXe(string lat1, string lng1, string lat2, string lng2, string from, string to, string loaixe, string kc, int? pg, string gia_select, string nhaxe, string date_go, string date_to, int? type_go)
        {
            if (lat1 == null || lng1 == null) return RedirectToAction("Index");

            int pageSize = 10;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;

            //kho dữ liệu giá xe đường dài việt nam
            if (lat1 == null) lat1 = "21.0277644"; if (lng1 == null) lng1 = "105.83415979999995";
            if (lat2 == null) lat2 = ""; if (lng2 == null) lng2 = "";
            if (from == null) from = ""; if (to == null) to = ""; if (loaixe == null) loaixe = "";
            if (gia_select == null) gia_select = "1"; if (nhaxe == null) nhaxe = "";
            string sql = "";

            if (!string.IsNullOrEmpty(date_go) && !string.IsNullOrEmpty(date_to))
            { 
              
                ViewBag.date_go = date_go;
                ViewBag.date_to = date_to;

                DateTime _dt_date_go = Config.ConvertToDatetime(date_go);
                DateTime _dt_date_to = Config.ConvertToDatetime(date_to);
                var songaydi = Config.dateDiff(_dt_date_go, _dt_date_to);
                ViewBag.songaydi = songaydi;
            }           

            if (type_go != null)
            {
                ViewBag.type_go = type_go;
            }

            sql = "SELECT t1.id as id, t1.name as name, t1.driver_type as driver_type, t1.car_years as car_years, t1.phone as phone, t1.email as email,t1.address as address, CAST( CASE WHEN t2.cp_car_type is null THEN t1.car_size ELSE t2.cp_car_type END AS int) as car_size, t1.car_model as car_model, t1.car_made as car_made, CAST( CASE WHEN t2.cp_price is null THEN t1.car_price ELSE t2.cp_price END AS int) as cp_price, CAST( CASE WHEN t2.cp_night is null THEN 0 ELSE t2.cp_night END AS int) as cp_night, t3.status as status, ACOS(SIN(PI()*" + lat1 + "/180.0)*SIN(PI()*t3.lat/180.0)+COS(PI()*" + lat1 + "/180.0)*COS(PI()*t3.lat/180.0)*COS(PI()*t3.lon/180.0-PI()*" + lng1 + "/180.0))*6371 as quangduong, DATEDIFF(day,t3.date_time,GETDATE()) AS DiffDate, t5.total_money as money FROM drivers t1 left JOIN driver_car_price t2 ON t1.id = t2.driver_id left JOIN list_online t3 on t1.phone = t3.phone and t1.car_number = t3.car_number and t3.lat <> 0 and t3.lon <> 0 full OUTER join drivers_money t5 on t1.id = t5.driver_id where t3.status = 0";

            // and t1.car_price >= 8000 
            List<timkiemDrivers> data = new List<timkiemDrivers>();
            try
            {
                data = db.Database.SqlQuery<timkiemDrivers>(sql).ToList();
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                return RedirectToAction("Index");
            }
            
            data = data.Where(x=>x.cp_price != -1).ToList();
            
            // Giới hạn khoảng cách nhà xe (<=300km)
            double? fq_duong = 300;
            data = data.Where(x => x.quangduong <= fq_duong).ToList();

            if (loaixe != null && loaixe != "")
            {
                ViewBag.loaixe = loaixe;
                int? iloaixe = int.Parse(loaixe);
                //sql += " and t1.car_size = " + loaixe + " or t2.cp_car_type = " + loaixe;
                data = data.Where(x => x.car_size.Equals(iloaixe)).ToList();
            }

            // tim kiem theo nhaxe
            try
            {
                if (nhaxe != null && nhaxe != "")
                {
                    int? inhaxe = int.Parse(nhaxe);
                    data = data.Where(x => x.driver_type == inhaxe).ToList();
                    ViewBag.nhaxe = nhaxe;
                }
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                return View();
            }    
        
                                    
            // Sắp xếp tăng dần theo giá xe               

            try
            {
                if (gia_select != null && gia_select != "1")
                {
                    data = data.OrderByDescending(x => x.cp_price).ToList();
                }
                else
                {
                    data = data.OrderBy(x => x.cp_price).ToList();
                }
            }

            catch
            {
                return View();
            }            



            //xuly loi khi nguoi dung co tinh xoa dia chi url de cho ket qua sai

            ViewBag.sotaixe = data.Count;

            if (kc != null && kc != "")
            {
                kc = kc.Trim();
                //ViewBag.kc_timkiem_view = kc;
                if (kc.Contains(','))
                {                    
                    kc = kc.Replace(',', '.');
                }
                //else if (kc.Contains('.'))
                //{
                //    kc = kc.Replace(".","");
                //}
                ViewBag.kc_timkiem = kc;

            }
            if (lat1 != null && lat1 != "")
	        {
		        ViewBag.lat1 = lat1;
	        }
            if(lng1 != null && lng1 != "")
	        {
		        ViewBag.lng1 = lng1;
	        }
             if(lat2 != null && lat2 != "")
	        {
		        ViewBag.lat2 = lat2;
	        }
            if(lng2 != null && lng2 != "")
	        {
		        ViewBag.lng2 = lng2;
	        }
            if(from != null && from != "") {
                ViewBag.from = from.Replace(", Việt Nam", "");
            }
            if(to != null && to != "") {
                ViewBag.to = to.Replace(", Việt Nam", "");
            }
            ViewBag.gia_select = gia_select;
            //if (search == null) search = ""; if (tt == null) tt = ""; if (car_hire_type == null) car_hire_type = "";
            data = data.OrderByDescending(x => x.money).OrderByDescending(x => x.driver_type).ToList();

            // taixe đã nộp tiền giảm dần

            return View(data.ToPagedList(pageNumber, pageSize)); 

        }

        public string getCarSize()
        {
            string carsize = "";
            try
            {
                //var p = (from q in db.drivers where q.car_size != null orderby q.car_size select q.car_size).Distinct().ToList();
                var p = (from q in db.car_size where q.name != null select q.name).Distinct().ToList();
                var p2 = p.Select(int.Parse).OrderBy(x=>x).ToList();
                for (int i = 0; i < p2.Count(); i++)
                {
                    string x1 = ""; string x2 = "";
                    if (p2[i] == 4)
                    {
                        x1 = "(giá siêu rẻ, có cốp)";
                    }
                    if (p2[i] == 5)
                    {
                        x2 = "(có cốp)";
                    }
                    carsize += "<option value=\"" + p2[i] + "\">Xe " + p2[i] + " chỗ " + x1 + x2 + "</option>";
                }
                
            }
            catch (Exception ex)
            {
            }
            return carsize;
        }

        public ActionResult Dangkyubergrab()
        {
            return View();
        }

        //public JsonResult GetLocationProperty()
        //{
        //    return Json(longlat, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult LoadCarImage(string made, string model)
        {
            string imgpath = "";
            string sql1 = "select top(1) image from car_made_model where made = N'" + made + "' and (model = '" + model + "' or model like N'%" + model + "%')";
            
            var url = db.Database.SqlQuery<string>(sql1).FirstOrDefault();
            if (url != null)
            {
                imgpath = url.ToString();
            }
            return PartialView("_LoadCarImage", imgpath);
        }

        //getModaldatxenhanh
        public string getModaldatxenhanh(long? driver_id, string diemdi, string diemden, string kcc)
        {
            string html = "";
            var _taxedx = db.drivers.Find(driver_id);
            if (_taxedx != null)
            {

                html += "<form class=\"form-horizontal\" method=\"post\" id=\"form_dat_thue_xe\" name=\"form_dat_thue_xe\" enctype=\"multipart/form-data\" style=\"color: #333;\">"
                   + "<div class=\"form-group\">"
                   + "<div class=\"col-md-12\">"
                   + "<h4>Tài xế: " + _taxedx.name + "</h4>"
                   + "<p><b>Đi từ: </b>" + diemdi + " <b>tới: </b>" + diemden + "</p>"
                   + "<p>Khoảng cách khoảng: " + kcc + " km (giá chưa bao gồm VAT).</p>"
                   +"</div>"
                   + "</div><hr/>"                 
                   + "<input type=\"hidden\" name=\"driver_id\" id=\"driver_id\" value=\"" + driver_id + "\" />"
                   + "<input type=\"hidden\" name=\"driver_name\" id=\"driver_name\" value=\"" + _taxedx.name + "\" />"
                   + "<input type=\"hidden\" name=\"lon_from\" id=\"lon_from\" value=\"\" />"
                   + "<input type=\"hidden\" name=\"lat_from\" id=\"lat_from\" value=\"\" />"
                   + "<input type=\"hidden\" name=\"lon_to\" id=\"lon_to\" value=\"\" />"
                   + "<input type=\"hidden\" name=\"lat_to\" id=\"lat_to\" value=\"\" />"
                   + "<input type=\"hidden\" name=\"from_place\" id=\"from_place\" value=\"\" />"
                   + "<input type=\"hidden\" name=\"to_place\" id=\"to_place\" value=\"\" />"
                   + "<input type=\"hidden\" name=\"car_type_made_model\" id=\"car_type_made_model\" value=\"\" />"
                   + "<input type=\"hidden\" name=\"car_hire_type\" id=\"car_hire_type\" value=\"Một chiều\" />"
                   + "<input type=\"hidden\" name=\"price_driver\" id=\"price_driver\" value=\"\" />"
                   + "<input type=\"hidden\" name=\"distance\" id=\"distance\" value=\"\" />"
                   + "<input type=\"hidden\" name=\"total_money\" id=\"total_money\" value=\"\" />"
                   + "<input type=\"hidden\" name=\"car_size_driver\" id=\"car_size_driver\" value=\"\" />"
                   + "<div class=\"form-group\">"
                   + "<div class=\"col-md-6\">"
                   + "<label class=\"control-label\">Họ tên khách: </label>"
                   + "<input name=\"customer_name\" id=\"customer_name\" class=\"form-control\" placeholder=\"Tên khách hàng\" />"
                   + "</div>"
                   + "<div class=\"col-md-6\">"
                   + "<label class=\"control-label\">Số điện thoại: </label>"
                   + "<input name=\"customer_phone\" id=\"customer_phone\" class=\"form-control\" placeholder=\"Số điện thoại khách hàng\" />"
                   + "</div>"
                   + "</div>"
                   + "<div class=\"form-group\">"
                   + "<div class=\"col-md-6\">"
                   + "<label class=\"control-label\">Từ ngày: </label>"
                   + "<input name=\"from_date\" id=\"from_date\" class=\"form-control\" placeholder=\"Từ ngày\" />"
                   + "</div>"
                   + "<div class=\"col-md-6\">"
                   + "<label class=\"control-label\">Đến ngày: </label>"
                   + "<input name=\"to_date\" id=\"to_date\" class=\"form-control\" placeholder=\"Đến ngày\" />"
                   + "</div>"
                   + "</div>"
                   + "<button type=\"button\" class=\"btn btn-primary\" id=\"btn_bookingtodriver\" onclick=\"saveBookingToDriver(" + _taxedx.id + ");\">Đặt xe</button>"
                   + "</form>";

                html += "<script>"
                + "$('#from_date').datetimepicker({"
                + "dayOfWeekStart: 1,"
                + "lang: 'en',"
                + "disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],"
                + "startDate: '@DateTime.Now.Year/@DateTime.Now.Month/@DateTime.Now.Date'"
                + "});"
                + "$('#to_date').datetimepicker({"
                + "dayOfWeekStart: 1,"
                + "lang: 'en',"
                + "disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],"
                + "startDate: '@DateTime.Now.Year/@DateTime.Now.Month/@DateTime.Now.Date'"
                + "});"
                + "var d = new Date();"
                + "var s = d.toLocaleString();"
                + "$('#to_date').datetimepicker({ value: s, step: 10 });"                
                + "</script>";
            }
            return html;
        }

        [HttpPost]
        public ActionResult saveBookingToDriver(booking_to_driver model)
        {
            int data = 0;
            try
            {
                booking_to_driver newBooking = new booking_to_driver();
                newBooking.lon_from = model.lon_from ?? null;
                newBooking.lat_from = model.lat_from ?? null;
                newBooking.lon_to = model.lon_to ?? null;
                newBooking.lat_to = model.lat_to ?? null;
                newBooking.from_place = model.from_place ?? null;
                newBooking.to_place = model.to_place ?? null;
                newBooking.car_type_made_model = model.car_type_made_model ?? null;
                newBooking.car_hire_type = model.car_hire_type ?? null;
                newBooking.driver_id = model.driver_id ?? null;
                newBooking.driver_name = model.driver_name ?? null;
                newBooking.date_booking = DateTime.Now;
                newBooking.price_driver = model.price_driver ?? null;
                newBooking.distance = model.distance ?? null;
                newBooking.total_money = model.total_money ?? null;
                newBooking.customer_name = model.customer_name ?? null;
                newBooking.customer_phone = model.customer_phone ?? null;
                newBooking.from_date = model.from_date ?? null;
                newBooking.to_date = model.to_date ?? null;
                newBooking.status = 0;
                newBooking.car_size_driver = model.car_size_driver ?? null;
                db.booking_to_driver.Add(newBooking);
                db.SaveChanges();
                data = 1;
                string result = "";
                //result += "Khách hàng: " + newBooking.customer_name
                //    + ".<br/> Số điện thoại: " + newBooking.customer_phone
                //    + ".<br/> Ngày đặt: " + newBooking.date_booking
                //    + ".<br/> Ngày đi: " + newBooking.from_date.ToString() + ".<br/> Ngày đến: " + newBooking.to_date.ToString()
                //    + ".<br/> Điểm đi: " + newBooking.from_place + ".<br/> Điểm đến: " + newBooking.to_place
                //    + ".<br/> Tài xế: " + newBooking.driver_name
                //    + ".<br/> Giá xe: " + newBooking.price_driver + " đồng"
                //    + ".<br/> Tổng tiền: " + newBooking.total_money + " đồng.";


                result += "<table style=\"margin: 0 auto;width: 700px;border: 1px solid #cbcbcb;background: rgba(193, 193, 193, 0.08);\">"
    + "<tr> <td colspan=\"2\"><img src=\"http://thuexevn.com/Images/bg_thx.jpg\" style=\"width: 100%; height: 150px\"/></td></tr>"
    + "<tr> <td colspan=\"2\" style=\"background: #337AB7;color: #fff;text-align: center;padding-top: 14px;\"> <h1>Khách đặt xe tài xế-<a style=\"color: #fff;\" href=\"http://thuexevn.com\">thuexevn.com</a></h1> </td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Khách hàng: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + newBooking.customer_name + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Số điện thoại: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + newBooking.customer_phone + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Ngày đặt: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + newBooking.date_booking + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Ngày đi: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + newBooking.from_date.ToString() + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Ngày đến: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + newBooking.to_date.ToString() + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Điểm đi: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + newBooking.from_place + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Điểm đến: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + newBooking.to_place + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Tài xế: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + newBooking.driver_name + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Giá xe: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + newBooking.price_driver + " đồng/km.</td> </tr>"
     + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Loại xe: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\"> Xe " + newBooking.car_size_driver + " chỗ, " + newBooking.car_type_made_model + " </td> </tr>"
     + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Khoảng cách: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + newBooking.distance + " km.</td> </tr>"
     + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Tổng tiền: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + newBooking.total_money + " đồng.</td> </tr>"
    + "<tr style=\"background: #F4FE01;color: #333;\"> <td colspan=\"2\" style=\"padding: 18px;\"> <p>Hotline: 096.410.8688 (HN)</p> <p>Hà Nội: Tòa Nhà Kim Ánh, số 1 Ngõ 78 Duy Tân, Dịch Vọng, Cầu giấy, Hà Nội.</p> </td> </tr>"
    + "</table>";


                // gui thông tin vao email
                try
                {
                    var sendmail = Config.Sendmail(WebConfigurationManager.AppSettings["emailroot"], WebConfigurationManager.AppSettings["passroot"], "thuexevn.com@gmail.com", DateTime.Now.ToString("dd-MM-yyyy HH:mm T") + "-Khách đặt xe tài xế", result);
                    if (sendmail == true)
	                {
		                data = 1;
	                }
                }
                catch (Exception ex)
                {
                    Config.SaveTolog(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LoadImageDriver(long? id)
        {
            var data = db.driver_images.Where(x => x.driver_id == id).Select(x => x).ToList();
            return PartialView("_LoadImageDriver", data);
        }

        public ActionResult profile(long? id, string taixe)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            ViewBag.driverId = driver.id;
            var p = db.list_online.Where(o => o.phone == driver.phone).FirstOrDefault();
            try
            {
                ViewBag.lon = p.lon;
                ViewBag.lat = p.lat;
            }
            catch (Exception ex)
            {
                ViewBag.lon = 105.3724793;
                ViewBag.lat = 20.9740874;
            }

            //get anh bia, anh dai dien
            ViewBag.fbImageLg = "/Content/fontend/img/bg_home_1.jpg";
            ViewBag.fbImageProfile = "/Content/fontend/img/logo.png";
            var driver_info = db.driver_info.Where(x => x.driver_id == id).FirstOrDefault();

            if (driver_info != null)
            {
                ViewBag.fbImageLg = driver_info.driver_img_cover ?? "/Content/fontend/img/bg_home_1.jpg";
                ViewBag.fbImageProfile = driver_info.driver_img_profile ?? "/Content/fontend/img/logo.png";
            }

            //Cập nhật driver_view
            var luotxem = db.driver_view.Where(x => x.driver_id == id).FirstOrDefault();
            if (luotxem == null)
            {
                driver_view addview = new driver_view();
                addview.driver_id = id;
                addview.views = 1;
                db.driver_view.Add(addview);
                db.SaveChanges();
            }
            else
            {
                luotxem.views += 1;
                db.Entry(luotxem).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(driver);
        }

        public ActionResult LoadAlbumDriver(long? driver_id)
        {
            var data = from s in db.driver_images where s.driver_id == driver_id orderby s.id descending select s;
            return PartialView("_LoadAlbumDriver", data.ToList());
        }

        public ActionResult LoadDesDriver(long? driver_id)
        {
            var data = from s in db.driver_info where s.driver_id == driver_id select s;
            return PartialView("_LoadDesDriver", data.FirstOrDefault());
        }

        public ActionResult LoadCommentDriver(long? driver_id, int? p)
        {
            var data = (from s in db.driver_rate_comment where s.cus_driver_id == driver_id orderby s.cus_cm_id descending select s).Take(5).ToList();

            //int pageSize = 1;
            //int pageNumber = (p ?? 1);
            //ViewBag.id = driver_id;
            return PartialView("_LoadCommentDriver", data.ToList());
        }

        [HttpPost]
        public ActionResult addCommentdriver(driver_rate_comment model)
        {
            string data = "";
            try
            {
                if (model.cus_cm_id == 0)
                {
                    driver_rate_comment newRate = new driver_rate_comment();
                    newRate.cm_date = DateTime.Now;
                    newRate.cus_rate = model.cus_rate ?? null;
                    newRate.cus_name = model.cus_name ?? null;
                    newRate.cus_phone = model.cus_phone ?? null;
                    newRate.cus_email = model.cus_email ?? null;
                    newRate.cus_driver_id = model.cus_driver_id ?? null;
                    newRate.cus_comment = model.cus_comment ?? null;
                    
                    db.driver_rate_comment.Add(newRate);
                    db.SaveChanges();
                    data = newRate.cus_cm_id.ToString();
                }
                else
                {
                    var newRate = db.driver_rate_comment.Find(model.cus_cm_id);
                    if (newRate != null)
                    {
                        newRate.cus_rate = model.cus_rate ?? null;
                        db.Entry(newRate).State = EntityState.Modified;
                        db.SaveChanges();
                        data = newRate.cus_cm_id.ToString();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TotalDanhGia(long? id)
        {
            var dt = db.driver_rate_comment.Where(x => x.cus_driver_id == id).Select(x => x).ToList();
            
            //var n = dt.Sum(x => x.cus_rate) / dt.Count;
            return PartialView("_TotalDanhGia", dt);
        }

        public ActionResult TotalDanhGiaRieng(long? id)
        {
            var dt = db.driver_rate_comment.Where(x => x.cus_driver_id == id).Select(x => x).ToList();
            var dg = new danhgiavm();
            var _rd = Guid.NewGuid().ToString();
            
            if (dt.Count > 0)
            {
                var n = dt.Sum(x => x.cus_rate) / dt.Count;
                dg = new danhgiavm()
                {
                    so1 = n,
                    so2 = id + _rd,
                    totalrate = dt.Count
                };
            }           
            
            return PartialView("_TotalDanhGiaRieng", dg);
        }

        [HttpPost]
        public ActionResult danh_gia_cm(long? id, int v)
        {
            string data = "";
            try
            {
                var dt = db.driver_rate_comment.Find(id);
                if (dt != null)
                {
                    if (v == 1)
                    {
                        
                        dt.cm_like = dt.cm_like != null ? dt.cm_like + 1 : 1;
                    }
                    else if (v == 0)
                    {
                        dt.cm_dislike = dt.cm_dislike != null ? dt.cm_dislike + 1 : 1;
                    }
                    db.Entry(dt).State = EntityState.Modified;
                    db.SaveChanges();
                    data = "1";
                }
                
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string bookingdatxe(string car_from, string car_to, string car_type, string car_hire_type, int? car_size, DateTime from_datetime, DateTime to_datetime, double? lon1, double? lat1, double? lon2, double? lat2, string name, string phone)
        {
            try
            {
                booking bo = new booking();
                bo.car_from = car_from;
                bo.car_to = car_to;
                bo.car_type = car_type;
                bo.car_hire_type = car_hire_type;
                bo.car_size = car_size;
                bo.date_from = from_datetime;
                bo.date_to = to_datetime;
                bo.lon1 = lon1;
                bo.lat1 = lat1;
                bo.lon2 = lon2;
                bo.lat2 = lat2;
                bo.name = name;
                bo.phone = phone;
                bo.date_time = DateTime.Now;
                bo.status = 0;
                db.bookings.Add(bo);
                db.SaveChanges();

                var result = "";
                //result += "Khách hàng: " + bo.name
                //    + ".<br/> Số điện thoại: " + bo.phone
                //    + ".<br/> Ngày đặt: " + bo.date_time
                //    + ".<br/> Ngày đi: " + bo.date_from.ToString() + ".<br/> Ngày đến: " + bo.date_to.ToString()
                //    + ".<br/> Điểm đi: " + bo.car_from + ".<br/> Điểm đến: " + bo.car_to
                //    + ".<br/> Kiểu xe: " + bo.car_type
                //    + ".<br/> Loại xe: " + bo.car_hire_type;

                result += "<table style=\"margin: 0 auto;width: 700px;border: 1px solid #cbcbcb;background: rgba(193, 193, 193, 0.08);\">"
    + "<tr> <td colspan=\"2\"><img src=\"http://thuexevn.com/Images/bg_thx.jpg\" style=\"width: 100%; height: 150px\"/></td></tr>"
    + "<tr> <td colspan=\"2\" style=\"background: #337AB7;color: #fff;text-align: center;padding-top: 14px;\"> <h1>Khách đặt xe hệ thống-<a style=\"color: #fff;\" href=\"http://thuexevn.com\">thuexevn.com</a></h1> </td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Khách hàng: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + bo.name + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Số điện thoại: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + bo.phone + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Ngày đi: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + bo.date_from.ToString() + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Ngày đến: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + bo.date_to.ToString() + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Điểm đi: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + bo.car_from + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Điểm đến: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + bo.car_to + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Kiểu xe: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + bo.car_type + "</td> </tr>"
    + "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Loại xe: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + bo.car_hire_type + "</td> </tr>"
    + "<tr style=\"background: #F4FE01;color: #333;\"> <td colspan=\"2\" style=\"padding: 18px;\"> <p>Hotline: 096.410.8688 (HN)</p> <p>Hà Nội: Tòa Nhà Kim Ánh, số 1 Ngõ 78 Duy Tân, Dịch Vọng, Cầu giấy, Hà Nội.</p> </td> </tr>"
    + "</table>";

                // send email
                try
                {
                    var sendmail = Config.Sendmail(WebConfigurationManager.AppSettings["emailroot"], WebConfigurationManager.AppSettings["passroot"], "thuexevn.com@gmail.com", DateTime.Now.ToString("dd-MM-yyyy HH:mm T") + "-Khách đặt xe hệ thống", result);
                }
                catch (Exception ex)
                {
                    Config.SaveTolog(ex.ToString());
                }

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        
        [HttpPost]
        public ActionResult SendEMail(string mv_ho_ten, string mv_phone, string mv_email, string mv_tieude, string mv_noidung, string mv_congty, string mv_diachi_congty, string mv_position)
        {
            int data = 0;
            try
            {
                string _body = "";
                //string _body = "Họ tên: " + mv_ho_ten + "<br />Số điện thoại: " + mv_phone + "<br /> Email: " + mv_email + "<br />Công ty: " + mv_congty + "<br />Địa chỉ: " + mv_diachi_congty + " <br />Nội dung: " + mv_noidung;

                _body += "<table style=\"margin: 0 auto;width: 700px;border: 1px solid #cbcbcb;background: rgba(193, 193, 193, 0.08);\">"
	+"<tr> <td colspan=\"2\"><img src=\"http://luatsu-vn.com/bg_1.jpg\" style=\"width: 100%; height: 150px\"/></td></tr>"
	+"<tr> <td colspan=\"2\" style=\"background: #991523;color: #fff;text-align: center;padding-top: 14px;\"> <h1>Luật sư Sblaw Việt Nam</h1> </td> </tr>"
	+"<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Họ tên: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + mv_ho_ten + "</td> </tr>"
	+"<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Số điện thoại: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + mv_phone + "</td> </tr>"
	+"<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Email: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + mv_email + "</td> </tr>"
	+"<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Công ty: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + mv_congty + "</td> </tr>"
	+"<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Địa chỉ: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + mv_diachi_congty + "</td> </tr>"
	+"<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Tiêu đề: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + mv_tieude + "</td> </tr>"
	+"<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Nội dung: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + mv_noidung + "</td> </tr>"
	+"<tr style=\"background: #f1b000;color: #fff;\"> <td colspan=\"2\" style=\"padding: 18px;\"> <p>Hotline: 0906.17.17.18 (HN) - 0904.340.664 (HCM)</p> <p>Hà Nội: Tầng 18, Center Building, Hapulico Complex, Số 85, Vũ Trọng Phụng, Quận Thanh Xuân</p> <p>Thành Phố Hồ Chí Minh.: Tầng 8, Toà nhà PDD, số 162, Đường Pasteur, Phường Bến Nghé, Quận 1.</p> </td> </tr>"
    +"</table>";


                if (Config.Sendmail(WebConfigurationManager.AppSettings["emailsblaw"], WebConfigurationManager.AppSettings["passsblaw"], "ha.nguyen@sblaw.vn", "Liên hệ ngày " + DateTime.Now + " " + mv_tieude, _body))
                {
                    data = 1;
                };

            }
            catch (Exception)
            {
                
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,driver_type,phone,email,car_model,car_made,car_years,car_size,car_number,car_type,car_price,total_moneys,province,date_time,code,address")] driver driver)
        {
            if (ModelState.IsValid)
            {
                db.drivers.Add(driver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(driver);
        }

        public ActionResult LoadBangGiaDriver(long? driver_id)
        {
            var data = db.driver_car_price.Where(x => x.driver_id == driver_id).OrderBy(x=>x.cp_car_type).ToList();
            return PartialView("_LoadBangGiaDriver", data);
        }

        public class car_model_made
        {
            public string name { get; set; }
        }

        public string getCarModelListFromMade2(string made)
        {
            string query = "SELECT  distinct model as name FROM [thuexevn].[dbo].[car_made_model] where made is not null and made like N'%" + made + "%' order by model";
            var p = db.Database.SqlQuery<car_model_made>(query);
            return JsonConvert.SerializeObject(p.ToList());
            //if (keyword == null) keyword = "";
            //var p = (from q in db.car_made_model where q.made.Contains(made) && q.model.Contains(keyword) orderby q.model ascending select new { name=q.model });
            //return JsonConvert.SerializeObject(p.ToList());
        }

        public ActionResult getcpdriver(long? id, int? socho)
        {
            int gia = -1;
            var data = db.driver_car_price.Where(x => x.driver_id == id && x.cp_car_type == socho).FirstOrDefault();
            if (data != null)
            {
                gia = (int)data.cp_price;
            }
            return Json(gia, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ContactUS()
        {
            return View();
        }

        [HttpPost]
        public ActionResult contactus(HttpPostedFileBase _file, string first_name, string add_ress, string email, string phone_number, string website, string[] loaixe, string banggia, string tour, string banggia_4, string banggia_5, string banggia_7, string banggia_16, string banggia_29, string banggia_35, string banggia_45)
        {
            if (Request.Files.Count > 0)
            {
                foreach (string file in Request.Files)
                {
                    _file = Request.Files[file];
                }
            }
            string fName = "";
            if (first_name == null) first_name = ""; if (add_ress == null) add_ress = ""; if (email == "") email = ""; if (phone_number == null) phone_number = ""; if (website == null) website = ""; if (loaixe == null) loaixe = new string[] { }; if (banggia == null) banggia = ""; if (tour == null) tour = "";
            if (banggia_4 == null) banggia_4 = "";
            if (banggia_5 == null) banggia_5 = "";
            if (banggia_7 == null) banggia_7 = "";
            if (banggia_16 == null) banggia_16 = "";
            if (banggia_29 == null) banggia_29 = "";
            if (banggia_35 == null) banggia_35 = "";
            if (banggia_45 == null) banggia_45 = ""; 
            //Save file content goes here
            if (_file != null && _file.ContentLength > 0)
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\imgnx", Server.MapPath(@"\")));
                string strDay = DateTime.Now.ToString("yyyyMM");
                string pathString = System.IO.Path.Combine(originalDirectory.ToString(), strDay);

                var _fileName = Guid.NewGuid().ToString("N") + ".jpg";

                bool isExists = System.IO.Directory.Exists(pathString);

                if (!isExists)
                    System.IO.Directory.CreateDirectory(pathString);

                var path = string.Format("{0}\\{1}", pathString, _fileName);

                _file.SaveAs(path);
                fName = "/Images/imgnx/" + strDay + "/" + _fileName;
            }

            var result = "";

            result += "<table style=\"margin: 0 auto;width: 700px;border: 1px solid #cbcbcb;background: rgba(193, 193, 193, 0.08);\">"
+ "<tr> <td colspan=\"2\"><img src=\"http://thuexevn.com/Images/bg_thx.jpg\" style=\"width: 100%; height: 150px\"/></td></tr>"
+ "<tr> <td colspan=\"2\" style=\"background: #337AB7;color: #fff;text-align: center;padding-top: 14px;\"> <h1>Form đăng ký thông tin nhà xe " + first_name + "</h1> </td> </tr>"
+ "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Tên nhà xe </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + first_name + "</td> </tr>"
+ "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Số điện thoại: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + phone_number + "</td> </tr>"
+ "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Địa chỉ: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + add_ress + "</td> </tr>"
+ "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Email: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + email + "</td> </tr>"
+ "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Website: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + website + "</td> </tr>"
+ "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Loại xe: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\"> " + string.Join(",", loaixe).ToString() + " chỗ.</td> </tr>";


            if (banggia_4 != null && banggia_4 != "") {
                result += "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Bảng giá xe 4 chỗ: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + banggia_4 + " đồng/1km.</td> </tr>";
            }
            if (banggia_5 != null && banggia_5 != "")
            {
                result += "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Bảng giá xe 5 chỗ: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + banggia_5 + " đồng/1km.</td> </tr>";
            }
            if (banggia_7 != null && banggia_7 != "")
            {
                result += "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Bảng giá xe 7 chỗ: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + banggia_7 + " đồng/1km.</td> </tr>";
            }
            if (banggia_16 != null && banggia_16 != "")
            {
                result += "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Bảng giá xe 16 chỗ: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + banggia_16 + " đồng/1km.</td> </tr>";
            }
            if (banggia_29 != null && banggia_29 != "")
            {
                result += "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Bảng giá xe 29 chỗ: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + banggia_29 + " đồng/1km.</td> </tr>";
            }
            if (banggia_35 != null && banggia_35 != "")
            {
                result += "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Bảng giá xe 35 chỗ: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + banggia_35 + " đồng/1km.</td> </tr>";
            }
            if (banggia_45 != null && banggia_45 != "")
            {
                result += "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Bảng giá xe 45 chỗ: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + banggia_45 + " đồng/1km.</td> </tr>";
            }

            result += "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Ghi chú bảng giá: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + banggia + "</td> </tr>"
+ "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Các tour hay đi: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">" + tour + "</td> </tr>"
+ "<tr> <td width=\"100\" style=\"border: 1px solid #cbcbcb; padding: 5px;\">Hình ảnh: </td> <td width=\"600\" style=\"border: 1px solid #cbcbcb; padding: 5px;\"><img style=\"width: 200px;\" src=\"http://thuexevn.com" + fName + "\" /></td> </tr>"
+ "<tr style=\"background: #F4FE01;color: #333;\"> <td colspan=\"2\" style=\"padding: 18px;\"> <p>Hotline: 096.410.8688 (HN)</p> <p>Hà Nội: Tòa Nhà Kim Ánh, số 1 Ngõ 78 Duy Tân, Dịch Vọng, Cầu giấy, Hà Nội. <br /> <a style=\"color: #fff;\" href=\"http://thuexevn.com\">thuexevn.com</a></p> </td> </tr>"
+ "</table>";

            // send email
            try
            {
                var sendmail = Config.Sendmail(WebConfigurationManager.AppSettings["emailroot"], WebConfigurationManager.AppSettings["passroot"], "thuexevn.com@gmail.com", DateTime.Now.ToString("dd-MM-yyyy HH:mm tt") + " - Form thông tin đăng ký nhà xe " + first_name, result);
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }


            return Json("1", JsonRequestBehavior.AllowGet);
        }


        public ActionResult Loadhotdeal()
        {
            var data = (from s in db.driver_view join d in db.drivers on s.driver_id equals d.id select new hotdeal(){
                driver_id = s.driver_id,
                driver_name = d.name,
                driver_view = s.views
            }).OrderByDescending(x=>x.driver_view).Take(5).ToList();
            return PartialView("_Loadhotdeal", data);
        }

        public ActionResult LoadDriverVertified(long? id)
        {
            int? tien = 0;
            var data = db.drivers_money.Where(x => x.driver_id == id).FirstOrDefault();
            if (data != null)
            {
                if (data.total_money > 0)
                {
                    tien = data.total_money;
                }
            }
            return PartialView("_LoadDriverVertified", tien);
        }

        public ActionResult getdriverview(long? id)
        {
            int? html = 0;
            var viewer = db.driver_view.Where(x => x.driver_id == id).SingleOrDefault();
            if (viewer != null)
            {
                html = viewer.views;
            }
            return Json(html, JsonRequestBehavior.AllowGet);
        }

    }
}