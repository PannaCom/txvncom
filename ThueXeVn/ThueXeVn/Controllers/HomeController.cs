using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ThueXeVn.Models;
namespace ThueXeVn.Controllers
{
    public class HomeController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();
        public ActionResult Index()
        {
            var p = (from q in db.news select q).OrderByDescending(o => o.id).Take(3);
            ViewBag.news = p.ToList();
            return View();
        }

        public ActionResult About()
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
            return "0";
        }
        public ActionResult Login()
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
        public string Register(string name, string phone, string province, string car_made, string car_model, int? car_size, int car_year, string car_type,int? car_price)
        {
            try
            {
                driver r = new driver();
                r.name = name;
                r.phone = phone;
                r.province = province;
                r.car_made = car_made;
                r.car_model = car_model;
                r.car_size = car_size;
                r.car_years = car_year;
                r.car_type = car_type;
                r.car_price = car_price;
                db.drivers.Add(r);
                db.SaveChanges();
                Config.mail("muabanraovat63@gmail.com", "vnnvh80@gmail.com", "Tài xế đăng ký " + phone, "Huynguyenviet1", "Họ tên: " + name + ", số điện thoại " + phone + ", tỉnh thành:" + province + ", Thông tin xe: " + car_made + "," + car_model + "," + car_size + "," + car_year);

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
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://thuexevn.com/Home/Price");
                    writer.WriteElementString("changefreq", "always");
                    writer.WriteElementString("priority", "0.99");
                    writer.WriteEndElement();
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://thuexevn.com/tin/page=1");
                    writer.WriteElementString("changefreq", "always");
                    writer.WriteElementString("priority", "0.98");
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
    }
}