using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using ThueXeVn.Models;
using Twilio;

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

    }
}