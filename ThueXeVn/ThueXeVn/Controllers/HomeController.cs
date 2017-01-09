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

        //public ActionResult Login()
        //{
        //    ViewBag.Message = "Your application description page.";
        //    return View();
        //}
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
                   

                    //tin tức
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://thuexevn.com/tin/page=1");
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
        public ActionResult Login()
        {
            if (Config.getCookie("taixelogged") != "") return RedirectToRoute("quanlybanggia");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string phone, string pass)
        {
            try
            {
                MD5 md5Hash = MD5.Create();
                var epass = Config.GetMd5Hash(md5Hash, pass);
                var login = (from t in db.drivers where t.phone == phone && t.pass == epass select t).FirstOrDefault();
                if (login == null)
	            {
                    ModelState.AddModelError("", "Sai thông tin đăng nhập.");
		            return View();
	            }
                var name_login = login.phone+login.id+login.name;
                Config.setCookie("taixelogged", Config.GetMd5Hash(md5Hash, name_login));
            }
            catch 
            {
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
                    Config.setCookie("taixeupdatepass", _tx + "," + p.id);
                    ViewBag.idtaixe = p.id;
                }
                else
                {
                    ModelState.AddModelError("", "Sai thông tin đăng nhập");
                    return View();
                }

            }
            catch
            {
                ModelState.AddModelError("", "Sai thông tin đăng nhập");
                return View();
            }
            return RedirectToRoute("taixedoipass");
        }

        public ActionResult UpdatePass()
        {
            if (Config.getCookie("taixeupdatepass") != "") return RedirectToRoute("taixedoipass");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePass(UpdatePassTaiXe model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi đổi mật khẩu.");
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
            }
            catch 
            {
                return View(taixeupdate);
            }
            return RedirectToAction("Login");
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

        public ActionResult banggiaxe()
        {
            return View();
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

    }
}