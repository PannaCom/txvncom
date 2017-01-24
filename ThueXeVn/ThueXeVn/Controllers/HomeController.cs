﻿using Newtonsoft.Json;
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
        public string Register(int? id, string name, string phone, string car_number, string car_made, string car_model, int? car_size, int car_year, string car_type, int? car_price, string address, double lon, double lat, string password_tx)
        {
            try
            {
                if (id == null || id==-1)
                { 
                    driver r = new driver();
                    r.name = name;
                    r.phone = phone;
                    r.car_number = car_number;
                    r.car_made = car_made;
                    r.car_model = car_model;
                    r.car_size = car_size;
                    r.car_years = car_year;
                    r.car_type = car_type;
                    r.car_price = car_price;
                    r.address = address;
                    r.code = "1";
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
                    lo.car_number = car_number;
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
                    r.name = name;
                    r.phone = phone;
                    r.car_number = car_number;
                    r.car_made = car_made;
                    r.car_model = car_model;
                    r.car_size = car_size;
                    r.car_years = car_year;
                    r.car_type = car_type;
                    r.car_price = car_price;
                    r.address = address;
                    r.date_time = DateTime.Now;
                    
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
                    db.Database.ExecuteSqlCommand("update list_online set lon=" + lon + ",lat=" + lat + " where phone=N'" + phone + "' and car_number=N'" + car_number + "'");
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

        public ActionResult banggiaxe(string lat1, string lng1, string lat2, string lng2, string from, string to, string loaixe, string tuyen, string kc)
        {
            if (loaixe == null && tuyen == null)
            {
                return View();
            }
            string banggiaxesql = "SELECT g8 as 'gialuudem'," + tuyen + " as 'giaxe' FROM pl_car_price where pl_car_type = " + loaixe;
            var banggiaxedata = db.Database.SqlQuery<getbanggia1>(banggiaxesql).FirstOrDefault();
            if (banggiaxedata == null)
            {
                return View(banggiaxedata);
            }
            if (kc == null)
	        {
		        kc = "10";
	        }
            
            var giaxe_1 = banggiaxedata.giaxe;
            int ikc = Convert.ToInt32(kc);
            if (tuyen == "g6" || tuyen == "g7")
            {
                if (tuyen == "g6")
                {
                    giaxe_1 *= 2*ikc;
                }
                else
                {
                    giaxe_1 *= ikc;
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

        public ActionResult TimTaiXe(string lat1, string lng1, string lat2, string lng2, string from, string to, string loaixe, string kc, int? pg, string gia_select)
        {
            int pageSize = 10;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;

            //kho dữ liệu giá xe đường dài việt nam
            if (lat1 == null) lat1 = "21.0277644"; if (lng1 == null) lng1 = "105.83415979999995"; if (gia_select == null) gia_select = "1";

            var sql = "SELECT t1.id as id, t1.name as name, t1.phone as phone, t1.email as email,t1.address as address, t1.car_size as car_size, t2.cp_car_type as car_size2, t1.car_model as car_model, t1.car_made as car_made, CAST( CASE WHEN t2.cp_price is null THEN t1.car_price ELSE t2.cp_price END AS int) as cp_price, t3.status as status, ACOS(SIN(PI()*" + lat1 + "/180.0)*SIN(PI()*t3.lat/180.0)+COS(PI()*" + lat1 + "/180.0)*COS(PI()*t3.lat/180.0)*COS(PI()*t3.lon/180.0-PI()*" + lng1 + "/180.0))*6371 as quangduong, DATEDIFF(day,t3.date_time,GETDATE()) AS DiffDate FROM drivers t1 left JOIN driver_car_price t2 ON t1.id = t2.driver_id left JOIN list_online t3 on t1.phone = t3.phone and t1.car_number = t3.car_number and t3.lat <> 0 and t3.lon <> 0 where t1.car_price <> -1 and status = 0";

            if (loaixe != null && loaixe != "")
            {
                ViewBag.loaixe = loaixe;
                sql += " and t1.car_size = " + loaixe + " or t2.cp_car_type = " + loaixe;
            }
            

            double? fq_duong = 300;
            List<timkiemDrivers> data = new List<timkiemDrivers>();
            try
            {
                if (gia_select == "1")
                {
                    data = db.Database.SqlQuery<timkiemDrivers>(sql).Where(x => x.quangduong <= fq_duong).OrderBy(x => x.cp_price).ToList();
                    //)
                }
                else
                {
                    data = db.Database.SqlQuery<timkiemDrivers>(sql).Where(x => x.quangduong <= fq_duong).OrderByDescending(x => x.cp_price).ToList();
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
                ViewBag.kc_timkiem_view = kc;
                if (kc.Contains(','))
                {                    
                    kc = kc.Replace(',', '.');
                }
                else if (kc.Contains('.'))
                {
                    kc = kc.Replace(".","");
                }
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
            return View(data.ToPagedList(pageNumber, pageSize)); 

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

      

    }
}