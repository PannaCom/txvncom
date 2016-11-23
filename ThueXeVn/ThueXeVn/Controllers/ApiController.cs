using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;
using Newtonsoft.Json;
using System.Data.Entity;
using Twilio;
using System.Threading.Tasks;
namespace ThueXeVn.Controllers
{
    public class ApiController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();
        // GET: Api
        public ActionResult Index()
        {
            return View();
        }
        public class glol
        {
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
        public string getlistonline(double lon, double lat, string car_type, string car_made, string car_model, int? car_size,int? order)
        {
            string query = "select lon,lat,name,phone,car_model,car_size,car_type,car_made,car_price,GETDATE() as datetime,D from ";
            query += " (select name,phone,car_model,car_size,car_type,car_made,car_price from drivers where code=N'1') as A left join (select phone as phone2,lon,lat,status,ACOS(SIN(PI()*" + lat + "/180.0)*SIN(PI()*lat/180.0)+COS(PI()*" + lat + "/180.0)*COS(PI()*lat/180.0)*COS(PI()*lon/180.0-PI()*" + lon + "/180.0))*6371 As D from list_online where status=0) as B on A.phone=B.phone2 where D<1000 ";

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
            if (car_size != null && car_size>0)
            {
                query += " and (car_size=" + car_size + ") ";
            }
            if (order == null || order == 0) { 
                query += " order by d";
            }
            else
            {
                query += " order by car_price";
            }
            var p = db.Database.SqlQuery<glol>(query);
            return JsonConvert.SerializeObject(p.ToList());
        }
        public string sendSms(string to,string body)
        {
            try{
                var accountSid = "AC04c48ac42eca3ddebf01fae0aa2e91a1"; // Your Account SID from www.twilio.com/console
                var authToken = "ee3e7a2863fbafc2391541ee9e192b05";  // Your Auth Token from www.twilio.com/console

                TwilioRestClient twilio = new TwilioRestClient(accountSid, authToken);

                var message = twilio.SendSmsMessage(
                    "+12566702599", // From (Replace with your Twilio number)
                    "+84"+to, // To (Replace with your phone number)
                    body
                    );
                if (message.RestException != null)
                    return "0"; 
                else 
                    return "1";
            }catch(Exception ex){
                return "0";
            }
        }
        public int randomcode()
        {
            Random random = new Random();
            return random.Next(10000, 99999)+1;
        }
        public string acitive(int idtaixe,string code){
            try {
                var p = db.drivers.Where(o => o.id == idtaixe && o.code == code).FirstOrDefault();
                if (p != null)
                {
                    db.Database.ExecuteSqlCommand("update drivers set code=N'1' where id=" + idtaixe + " and code=N'" + code + "'");
                    return "1";
                }
                else return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public string resendactive(int idtaixe){
            try { 
                string code=randomcode().ToString();
                db.Database.ExecuteSqlCommand("update drivers set code=N'" + code + "' where id=" + idtaixe);
                return sendSms(db.drivers.Find(idtaixe).phone, "Ma kich hoat la " + code);
            }catch(Exception ex){
                return "0";
            }
        }
        [HttpPost]
        public string Register(int? id,string name, string phone, string car_made, string car_model, int car_size, int car_year, string car_type, int? car_price)
        {
            try
            {
                if (id == null) { 
                    driver r = new driver();
                    r.name = name;
                    r.phone = phone;
                    r.car_made = car_made;
                    r.car_model = car_model;
                    r.car_size = car_size;
                    r.car_years = car_year;
                    r.car_type = car_type;
                    r.car_price = car_price;
                    r.date_time = DateTime.Now;
                    db.drivers.Add(r);
                    db.SaveChanges();
                    string code=randomcode().ToString();
                    string ok = sendSms(phone, "Ma kich hoat la " + code);
                    db.Database.ExecuteSqlCommand("update drivers set code=N'" + code + "' where id=" + r.id);
                    return r.id.ToString();
                }
                else
                {
                    driver r = db.drivers.Find(id);
                    db.Entry(r).State = EntityState.Modified;
                    r.name = name;
                    r.phone = phone;
                    r.car_made = car_made;
                    r.car_model = car_model;
                    r.car_size = car_size;
                    r.car_years = car_year;
                    r.car_type = car_type;
                    r.car_price = car_price;
                    r.date_time = DateTime.Now;
                    db.SaveChanges();
                }
                //Config.mail("muabanraovat63@gmail.com", "vnnvh80@gmail.com", "Tài xế đăng ký " + phone, "Huynguyenviet1", "Họ tên: " + name + ", số điện thoại " + phone + ", tỉnh thành:" + province + ", Thông tin xe: " + car_made + "," + car_model + "," + car_size + "," + car_year);

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        [HttpPost]
        public string locate(string car_number, double lon, double lat, int status, string phone)
        {
            try
            {
                if (status == 0)
                {
                    db.Database.ExecuteSqlCommand("delete from list_online where phone=N'" + phone + "'");
                    //db.Database.ExecuteSqlCommand("update list_online set status=0 where phone=N'" + phone + "'");
                    //db.Database.ExecuteSqlCommand("update driver set last_online=getdate() where id=" + idtaixe);
                    list_online lo = new list_online();
                    lo.date_time = DateTime.Now;
                    lo.phone = phone;
                    lo.lon = lon;
                    lo.lat = lat;
                    lo.geo = Config.CreatePoint(lat, lon);
                    lo.car_number = car_number;
                    lo.status = 0;
                    db.list_online.Add(lo);
                    db.SaveChanges();
                }
                else
                {
                    db.Database.ExecuteSqlCommand("update list_online set status=1 where phone=N'" + phone + "'");
                }
                return "1";

            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public class glol2
        {
            public double lon { get; set; }
            public double lat { get; set; }
            public DateTime? date_time { get; set; }
            public double D { get; set; }
        }
        public string getaround(double lon, double lat)
        {
            string query = "";
            query="select lon,lat,date_time,D from (select lon,lat,date_time,ACOS(SIN(PI()*" + lat + "/180.0)*SIN(PI()*lat/180.0)+COS(PI()*" + lat + "/180.0)*COS(PI()*lat/180.0)*COS(PI()*lon/180.0-PI()*" + lon + "/180.0))*6371 As D from list_online where status=0 and DATEDIFF(minute,date_time,getdate())<=60) as A where D<=100 order by D";

            var p = db.Database.SqlQuery<glol2>(query);
            return JsonConvert.SerializeObject(p.ToList());
        }
        public class car_model_made
        {
            public string name { get; set; }
        }
        public string getCarModelList(string keyword)
        {
            if (keyword == null) keyword = "";
            string query = "SELECT  distinct name FROM [thuexevn].[dbo].[list_car_model] where name is not null and name like N'%" + keyword + "%' order by name";
            var p = db.Database.SqlQuery<car_model_made>(query);
            return JsonConvert.SerializeObject(p.ToList()); 
        }
        public string getCarModelList2(string keyword)
        {
            if (keyword == null) keyword = "";
            var p = (from q in db.list_car_model where q.name.Contains(keyword) orderby q.name ascending select q.name);
            return JsonConvert.SerializeObject(p.ToList());
        }
        public string getCarModelListFromMade(string keyword)
        {
            if (keyword == null) keyword = "";
            string query = "SELECT  distinct model as name FROM [thuexevn].[dbo].[car_made_model] where made is not null and made like N'%" + keyword + "%' order by model";
            var p = db.Database.SqlQuery<car_model_made>(query);
            return JsonConvert.SerializeObject(p.ToList());
        }
        public string activecode(string phone, string code)
        {
            try
            {
                var p=db.activecodes.Where(o=>o.code==code && (o.phone==null || o.phone=="")).FirstOrDefault();
                db.Database.ExecuteSqlCommand("update activecode set phone=N'" + phone + "' where code=N'" + code + "' and id=" + p.id);
                return p.type_code.ToString();
            }catch(Exception ex){
                return "-1";
            }
        }
        public string call(string phone)
        {
            try
            {
                var p = db.call_log.Any(o => o.phone == phone);
                if (p) {
                    db.Database.ExecuteSqlCommand("update call_log set calls=calls+1 where phone=N'" + phone + "'");
                }
                else
                {
                    call_log clg = new call_log();
                    clg.calls = 1;
                    clg.phone = phone;
                    db.call_log.Add(clg);
                    db.SaveChanges();
                }
                return "1";
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }
        public string getCarMadeList()
        {
            string query = "SELECT  distinct car_made as name FROM [thuexevn].[dbo].[drivers] where car_made is not null order by car_made";
            var p = db.Database.SqlQuery<car_model_made>(query);
            return JsonConvert.SerializeObject(p.ToList());
        }
        public string getAllCarMadeList()
        {
            string query = "SELECT  name FROM [thuexevn].[dbo].[list_car] where name is not null order by no";
            var p = db.Database.SqlQuery<car_model_made>(query);
            return JsonConvert.SerializeObject(p.ToList());
        }
        public string getAllCarTypeList()
        {
            string query = "SELECT  name FROM [thuexevn].[dbo].[list_car_type] where name is not null order by id";
            var p = db.Database.SqlQuery<car_model_made>(query);
            return JsonConvert.SerializeObject(p.ToList());
        }
        [HttpPost]
        public string DelRegister(string id)
        {
            try { 
                if (Config.getCookie("logged") == "") return "0";
                db.Database.ExecuteSqlCommand("delete from drivers where id in " + id);
                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }

        }
        [HttpPost]
        public string activeDriver(int id)
        {
            try
            {
                if (Config.getCookie("logged") == "") return "0";
                db.Database.ExecuteSqlCommand("update drivers set code=N'1' where id=" + id);
                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }

        }
        #region api/PostRegId
        public class dnotifies {
            public int? tobject { get; set; }
            public string regid { get; set; }
            public int? os { get; set; }
        }

        // nguyenvannam - date: 14/11/2016
        [HttpPost]
        public int PostRegId(dnotifies model)
        {
            int sended = 0;
            if (!ModelState.IsValid)
            {
                sended = 0;
            }
            try
            {
                var _notifi = db.notifies.Where(x => x.reg_id == model.regid).FirstOrDefault();
                if (_notifi == null)
                {
                    notify newtnotify = new notify();
                    newtnotify.tobject = model.tobject ?? null;
                    newtnotify.reg_id = model.regid ?? null;
                    newtnotify.os = model.os ?? null;
                    db.notifies.Add(newtnotify);
                    db.SaveChanges();
                    sended = 1;
                }
                else
                {
                    sended = 1;
                }
                return sended;
            }
            catch (Exception ex)
            {
               return sended;
            }
            
        }

        #endregion

    }
}