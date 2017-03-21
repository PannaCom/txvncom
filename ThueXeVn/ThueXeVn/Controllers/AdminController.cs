using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using ThueXeVn.Models;
using Twilio;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace ThueXeVn.Controllers
{
    public class AdminController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();
        // GET: Admin
        public ActionResult danhsachctv(int? pg, string search)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");

            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            if (search == null) {search = "";}

            string sql = "select ctv_id, ctv_fullname, ctv_phone, ctv_email, point_share, date_create, status from ctv_tiepthi";

            if (search != null && search != "")
            {
                sql += " where ctv_fullname like N'%" + search + "%' or ctv_phone like N'%" + search + "%'";
                ViewBag.search = search;
            }

            var data = db.Database.SqlQuery<ctvVM>(sql).ToList().OrderByDescending(s => s.date_create);


            return View(data.ToList().ToPagedList(pageNumber, pageSize));
        }


        public ActionResult CountNumberShares(long? id)
        {
            var sql = "select COUNT(t1.ctv_id) from ctv_tiepthi t1 join booking_ctv_tiepthi t2 on t1.ctv_id = t2.ctv_id where t1.ctv_id = " + id;
            var x = 0;
            try
            {
                x = db.Database.SqlQuery<int>(sql).FirstOrDefault();
            }
            catch 
            {
            }            
            return PartialView("_CountNumberShares", x);
        }

        //updatestatus
        public string updatestatus(int id)
        {
            try
            {
                var _ctv = db.ctv_tiepthi.Find(id);
                if (_ctv != null)
                {
                    string sql = "";
                    if (_ctv.status == false)
                    {
                        sql = "update ctv_tiepthi set status = 1 where ctv_id =" + id;
                    }
                    else
                    {
                        sql = "update ctv_tiepthi set status = 0 where ctv_id =" + id;
                    }
                    db.Database.ExecuteSqlCommand(sql);
                }

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public ActionResult SendSMS()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");



            return View();
        }

        [HttpPost]
        public ActionResult SendSMS(string phone_number, int sendmulti1, string content_sms)
        {
            if (phone_number != null && phone_number.Length < 10 && phone_number.Length > 11)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại số điện thoại.");
                return View();
            }

            var acountsms = (from s in db.value_config select s).FirstOrDefault();
            string error = ""; string success = "";
            try
            {
                var accountSid = acountsms.value_id; // Your Account SID from www.twilio.com/console
                var authToken = acountsms.value_token;  // Your Auth Token from www.twilio.com/console

                TwilioRestClient twilio = new TwilioRestClient(accountSid, authToken);

                if (sendmulti1 == 1)
                {
                    var message = twilio.SendSmsMessage(
                    "+12566702599", // From (Replace with your Twilio number)
                    "+84" + phone_number, // To (Replace with your phone number)
                    content_sms
                    );

                    if (message.RestException != null)
                    {
                        error = "Có lỗi không gửi được tin nhắn.";                        
                    }
                    else
                    {
                        success = "Tin nhắn gửi thành công tới số điện thoại ." + phone_number;
                    }
                    string tt = "";
                    if (error != "")
	                {
		                tt = "chua gui";
	                }
                    if (success != "")
	                {
		                 tt = "da gui";
	                }
                    Config.SavePhoneSended(phone_number, tt);
                }
                else if(sendmulti1 == 2)
                {
                    var dstaixe = db.drivers.Where(x => x.phone != null && x.phone.Length >= 10 && x.phone.Length <= 11).Select(x=>x.phone).ToList();
                    if (dstaixe.Count > 0)
                    {
                        foreach (var item in dstaixe)
                        {
                            var message = twilio.SendSmsMessage(
                            "+12566702599", // From (Replace with your Twilio number)
                            "+84" + item, // To (Replace with your phone number)
                                content_sms
                            );
                            string tt = "da gui";
                            if (message.RestException != null)
                            {
                                tt = "chua gui";
                                error += "Có lỗi không gửi được tin nhắn tới số " + item + " <br />.";
                                //continue;
                            }
                            Config.SavePhoneSended(item, tt);
                        }
                        success += "Tin nhắn gửi thành công tới tất cả người nhận.";     
                    }
                }

                if (error != "")
                {
                    TempData["Error"] = error;
                }
                TempData["Update"] = success;
                
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                TempData["Error"] = error;
            }

            return RedirectToAction("SendSMS");
        }

        public ActionResult khachdatxe(string k, int? pg)
        {
            if (k == null) k = "";
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            var p = (from q in db.booking_to_driver where q.customer_name.Contains(k) || q.customer_phone.Contains(k) || q.driver_name.Contains(k) select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            return View(p.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult deleteKhachDatXe(long? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            string deleted = "";
            var khach = db.booking_to_driver.Find(id);
            if (khach != null)
            {
                db.booking_to_driver.Remove(khach);
                db.SaveChanges();
                deleted = "1";
            }

            return Json(deleted, JsonRequestBehavior.AllowGet);
        }

        public ActionResult taixetoancau()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");

            return View();
        }

        public class txtoancau {
            public string name {get; set;}
            public string phone {get; set;}
        }

        public ActionResult loadtaixe()
        {
            var data = db.tai_xe_toan_cau.Where(x => x.F4 != null && x.F4 != "0").Select(x => new txtoancau()
            {
                name = x.F3,
                phone = x.F4
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult sendsmstoancau(string content, string phone) 
        {
            // load ds cuoi cung
            int txend = -1;

            var originalDirectory = new DirectoryInfo(Server.MapPath(@"\"));
            string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "log_sptc_end.txt");

            bool isExists = System.IO.File.Exists(pathString);

            if (isExists){
                string contentfile = System.IO.File.ReadAllText(pathString);
                txend = int.Parse(contentfile);
            }          

            if (phone == null) phone = "all";
            var dstx = db.tai_xe_toan_cau.Where(x => x.F4 != null && x.F4 != "0").OrderBy(x => x.F1).ToList();

            if (txend > -1)
            {
                dstx = dstx.Where(x => x.F1 > txend).ToList();
            }
            StringBuilder sb = new StringBuilder();
            string strstatus = "chưa gửi được";
            int countTaixe = 0;
            var acountsms = (from s in db.value_config select s).FirstOrDefault();
            try
            {
                var accountSid = acountsms.value_id; // Your Account SID from www.twilio.com/console
                var authToken = acountsms.value_token;  // Your Auth Token from www.twilio.com/console

                TwilioRestClient twilio = new TwilioRestClient(accountSid, authToken);
                if (dstx.Count > 0)
                {
                    foreach (var item in dstx)
                    {
                        //var i = 1;
                        var _sophone = getPhoneNumber(item.F4);
                        string jsonCustomer = JsonConvert.
                                  SerializeObject(_sophone);
                        sb.AppendFormat("data: {0}\n\n", jsonCustomer);

                        var message = twilio.SendSmsMessage(
                        "+12566702599", // From (Replace with your Twilio number)
                        "+84" + _sophone, // To (Replace with your phone number)
                            content
                        );

                        if (message.RestException != null)
                        {
                            Config.SaveLogSendedEnd(item.F1);
                            break;
                        }
                        else
                        {
                            strstatus = "Đã gửi thành công.";
                            Config.SavePhoneToanCau(_sophone, strstatus);
                            countTaixe += 1;
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }
            //return Content(sb.ToString(), "text/event-stream");
            return Json(new { message = strstatus, count = countTaixe }, JsonRequestBehavior.AllowGet);
        }

        //public void Process()
        //{
        //    Response.ContentType = "text/event-stream";
        //    using (thuexevnEntities db = new thuexevnEntities())
        //    {
        //        foreach (var obj in db.tai_xe_toan_cau)
        //        {
        //            string jsonCustomer = JsonConvert.
        //                         SerializeObject(obj);
        //            string data = string.Format("data: {jsonCustomer}\n\n", jsonCustomer);
        //            System.Threading.Thread.Sleep(5000);
        //            Response.Write(data);
        //            Response.Flush();
        //        }
        //        Response.Close();
        //    }
        //}

        //public ActionResult Process()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    using (thuexevnEntities db = new thuexevnEntities())
        //    {
        //        foreach (var obj in db.tai_xe_toan_cau)
        //        {
        //            string jsonCustomer = JsonConvert.
        //                                  SerializeObject(obj);
        //            sb.AppendFormat("data: {0}\n\n", jsonCustomer);
        //        }
        //    }
        //    return Content(sb.ToString(), "text/event-stream");
        //}


        public string getPhoneNumber(string subjectString)
        {
            //System.Text.RegularExpressions.Regex
            var resultString = subjectString;
            if (resultString.Contains("'"))
            {
                resultString = resultString.Replace("'", "");
            }
            if (resultString.Contains("."))
            {
                resultString = resultString.Replace(".", "");
            }
            if (resultString.Contains(":"))
            {
                resultString = resultString.Replace(":", "");
            }
            if (resultString.Contains("/"))
            {
                resultString = resultString.Split('/')[0];
            }
            resultString = Regex.Match(resultString, @"\d+").Value;
            resultString = resultString.Trim();
            return resultString;
        }

    }
}