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
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;

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
            if (search == null) { search = ""; }

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
                else if (sendmulti1 == 2)
                {
                    var dstaixe = db.drivers.Where(x => x.phone != null && x.phone.Length >= 10 && x.phone.Length <= 11).Select(x => x.phone).ToList();
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

        public class txtoancau
        {
            public string name { get; set; }
            public string phone { get; set; }
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

            if (isExists)
            {
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

        public ActionResult adminbanggia(int? pg, string search)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");

            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            if (search == null) { search = ""; }

            var data = (from s in db.driver_car_price
                        join d in db.drivers on s.driver_id equals d.id
                        select
                            new driver_car_price_vm()
                            {
                                id = s.id,
                                driver_name = d.name,
                                driver_id = s.driver_id,
                                cp_price = s.cp_price,
                                cp_night = s.cp_night,
                                cp_multiple2 = s.cp_multiple2,
                                cp_multiple = s.cp_multiple,
                                cp_car_type = s.cp_car_type
                            }).ToList();

            if (search != null && search != "")
            {
                search = search.Trim();
                data = data.Where(x => x.driver_name.Contains(search)).ToList();
                ViewBag.search = search;
            }

            return View(data.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult admineditbanggia(long? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null || id == 0)
            {
                return RedirectToAction("adminbanggia");
            }
            var banggia = (from s in db.driver_car_price
                           where s.id == id
                           join d in db.drivers on s.driver_id equals d.id
                           select
                           new driver_car_price_vm()
                           {
                               id = s.id,
                               driver_name = d.name,
                               driver_id = s.driver_id,
                               cp_price = s.cp_price,
                               cp_night = s.cp_night,
                               cp_multiple2 = s.cp_multiple2,
                               cp_multiple = s.cp_multiple,
                               cp_car_type = s.cp_car_type
                           }).FirstOrDefault();
            return View(banggia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult admineditbanggia(long? cp_id, driver_car_price_vm model)
        {
            try
            {
                var editprice = db.driver_car_price.Find(cp_id);
                if (editprice != null)
                {
                    editprice.cp_price = model.cp_price ?? null;
                    editprice.cp_night = model.cp_night ?? 0;
                    db.Entry(editprice).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //var sql = "update driver_car_price set cp_car_type = " + cp_car_type + ", cp_price = " + cp_price + " where id = " + cp_id;
                //var updatebanggiaxe = db.Database.ExecuteSqlCommand(sql);
                TempData["Updated"] = "Đã Cập nhật mới bảng giá.";
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                TempData["Error"] = "Vui lòng kiểm tra lại các trường.";
            }
            return RedirectToAction("adminbanggia");
        }

        #region ### module get place nearby

        public class mapdata
        {
            public results[] results { get; set; }
        }

        public class nearby
        {
            public results2[] results { get; set; }
        }

        public class photos
        {
            public int? height { get; set; }
            public string[] html_attributions { get; set; }
            public int? width { get; set; }

        }


        public class results
        {
            public geometry geometry { get; set; }
            public string formatted_address { get; set; }
        }

        public class results2
        {
            public geometry geometry { get; set; }
            public string icon { get; set; }
            public string name { get; set; }
            public photos[] photos { get; set; }
            public double? rating { get; set; }
            public string scope { get; set; }
            public string[] types { get; set; }
            public string vicinity { get; set; }
        }

        public class geometry
        {
            public location location { get; set; }
        }

        public class location
        {
            public double? lat { get; set; }
            public double? lng { get; set; }
        }

        public async Task<ActionResult> getnearbyrestaurent(string type)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (type == null)
            {
                return View();
            }

            List<country> data = new List<country>();

            // get config 
            int? stepid = 0;
            var config = (from c in db.country_config where c.type == type select c).FirstOrDefault();
            if (config != null)
            {
                stepid = config.country_end;
            }

            data = (from s in db.countries orderby s.id where s.lon != null && s.lat != null && s.id > stepid select s).ToList();

            var total = 0;
            var countend = 0;
            var totalplace = 0;
            if (data.Count == 0)
            {
                TempData["Updated"] = "Đã cập nhật danh sách " + type + " cho các tỉnh thành.";
                return View();
            }

            data = data.Take(50).ToList();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    foreach (var item in data)
                    {
                        var urlreq = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + item.lat + "," + item.lon + "&radius=15000&type=" + type + "&key=AIzaSyBqxfIx32ftFpmqW4i2bZ5bckYM_LBl870";
                        HttpResponseMessage response = await httpClient.GetAsync(urlreq);
                        response.EnsureSuccessStatusCode();
                        if (response.IsSuccessStatusCode)
                        {
                            var stateInfo = response.Content.ReadAsStringAsync().Result;
                            //var geodata = JsonConvert.DeserializeObject<mapdata>(stateInfo);
                            var nearby = await JsonConvert.DeserializeObjectAsync<nearby>(stateInfo);

                            if (nearby != null)
                            {
                                foreach (var itemnearby in nearby.results)
                                {
                                    //check nếu đã có địa điểm đã tồn tại thì bỏ qua, nhảy tới cái tiếp theo
                                    if (db.country_place_nearby.Any(o => o.lat == itemnearby.geometry.location.lat && o.lng == itemnearby.geometry.location.lng)) continue;

                                    country_place_nearby addnearby = new country_place_nearby();
                                    addnearby.country_id = item.id;
                                    addnearby.icon = itemnearby.icon ?? null;
                                    addnearby.lat = itemnearby.geometry.location.lat ?? null;
                                    addnearby.lng = itemnearby.geometry.location.lng ?? null;
                                    addnearby.name = itemnearby.name ?? null;
                                    if (itemnearby.photos != null)
                                    {
                                        addnearby.photo_height = itemnearby.photos[0].height ?? null;
                                        addnearby.photo_width = itemnearby.photos[0].width ?? null;
                                        addnearby.photo_html = itemnearby.photos[0].html_attributions != null ? string.Join(";", itemnearby.photos[0].html_attributions).ToString() : null;
                                    }
                                    addnearby.rating = itemnearby.rating ?? null;
                                    addnearby.scope = itemnearby.scope ?? null;
                                    addnearby.type = type ?? null;
                                    addnearby.types = itemnearby.types != null ? string.Join(";", itemnearby.types) : null;
                                    addnearby.vicinity = itemnearby.vicinity ?? null;
                                    db.country_place_nearby.Add(addnearby);
                                    await db.SaveChangesAsync();
                                    totalplace += 1;
                                }

                                total += 1;
                                countend = item.id;
                            }

                        }
                    }

                }

                //Đánh dấu quan huyen đã quét vào country_config
                if (config != null)
                {
                    config.country_end = countend;
                    db.Entry(config).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                else
                {
                    country_config newconfig = new country_config();
                    newconfig.type = type ?? null;
                    newconfig.country_end = countend;
                    db.country_config.Add(newconfig);
                    await db.SaveChangesAsync();
                }
                ViewBag.next = "ok";
                ViewBag.type = type;

            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                return View();
            }

            TempData["Updated"] = "Đã lấy được danh sách  <b>" + totalplace + "</b> " + type + "/<b>" + total + "</b> quận huyện, quận huyện id cuối cùng: " + countend;
            return View();
        }

        public async Task<ActionResult> getlonglatquanhuyen(string type)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (type == null)
            {
                return View();
            }

            // get danh sach quan huyen
            var data = (from s in db.countries where s.lon == null && s.lat == null select s).ToList();
            var total = 0;
            if (data.Count == 0)
            {
                TempData["Updated"] = "Đã lấy hết vị trí lonlat cho các quận huyện.";
                return View();
            }
            try
            {

                using (HttpClient httpClient = new HttpClient())
                {
                    foreach (var item in data)
                    {
                        var urlreq = "https://maps.googleapis.com/maps/api/geocode/json?address=" + item.quanhuyen + "," + item.tinhthanh + "&sensor=false";
                        HttpResponseMessage response = await httpClient.GetAsync(urlreq);
                        response.EnsureSuccessStatusCode();
                        if (response.IsSuccessStatusCode)
                        {
                            var stateInfo = response.Content.ReadAsStringAsync().Result;
                            //var geodata = JsonConvert.DeserializeObject<mapdata>(stateInfo);
                            var geodata = await JsonConvert.DeserializeObjectAsync<mapdata>(stateInfo);

                            if (geodata != null)
                            {
                                //update lonlat quan huyen
                                var _location = geodata.results[0].geometry.location;
                                var _fulladdress = geodata.results[0].formatted_address;

                                item.lat = _location.lat ?? null;
                                item.lon = _location.lng ?? null;
                                item.formatted_address = _fulladdress ?? null;
                                db.Entry(item).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                                total += 1;
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                return View();
            }


            TempData["Updated"] = "Đã lấy được vị trí lonlat cho: " + total + " quận huyện.";
            return View();
        }

        #endregion
    }
}