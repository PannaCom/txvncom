using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;
using PagedList;
using PagedList.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;

namespace ThueXeVn.Controllers
{
    public class TaiXeBangGiaController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();
        // GET: TaiXeBangGia
        public ActionResult Index(int? pg)
        {
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");

            var id_taixe = Config.getCookie("taixelogged").Split(',').Last();
            long id = Convert.ToInt64(id_taixe);

            var taxidn = db.drivers.Find(id);
            if (taxidn == null)
            {
                Config.RemoveCookie("taixelogged"); return RedirectToRoute("taixedangnhap");
            }

            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;

            //var sql = "SELECT id,cp_car_type,cp_price,cp_multiple,cp_multiple2,driver_id FROM driver_car_price where driver_id = " + id;

            //var data = db.Database.SqlQuery<driver_car_price>(sql).ToList().OrderBy(s => s.cp_car_type);
            var data = (from s in db.driver_car_price where s.driver_id == id orderby s.cp_car_type ascending select s);

            return View(data.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Addbanggia()
        {
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");
            var id_taixe = Config.getCookie("taixelogged").Split(',').Last();
            long id = Convert.ToInt64(id_taixe);
            var taxidn = db.drivers.Find(id);
            if (taxidn == null)
            {
                Config.RemoveCookie("taixelogged"); return RedirectToRoute("taixedangnhap");
            }
            ViewBag.driver_id = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addbanggia(driver_car_price model)
        {
            try
            {
                driver_car_price newprice = new driver_car_price();
                newprice.driver_id = model.driver_id ?? null;
                newprice.cp_price = model.cp_price ?? null;
                newprice.cp_night = model.cp_night ?? 0;
                db.driver_car_price.Add(newprice);
                db.SaveChanges();
                //var sql = "INSERT INTO driver_car_price(cp_car_type,cp_price,driver_id) VALUES(" + cp_car_type + "," + cp_price + "," + id + ")";
                //var addbanggiaxe = db.Database.ExecuteSqlCommand(sql);
                TempData["Updated"] = "Đã thêm mới bảng giá.";
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
                TempData["Error"] = "Vui lòng kiểm tra lại các trường.";
            }
            return RedirectToRoute("quanlybanggia");
        }

        public ActionResult Edit(long? id)
        {
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");
            if (id == null || id == 0)
            {
                return RedirectToAction("Addbanggia");
            }
            var banggia = db.driver_car_price.Find(id);
            return View(banggia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(long? cp_id, driver_car_price model)
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
            return RedirectToRoute("quanlybanggia");
        }

        //DeleteBuild
        public ActionResult Delete(long? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToRoute("quanlybanggia");
            }
            driver_car_price model = db.driver_car_price.Find(id);
            if (model == null)
            {
                return View();
            }
            return View(model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long? id)
        {
            driver_car_price model = await db.driver_car_price.FindAsync(id);
            if (model == null)
            {
                return View();
            }

            try
            {
                db.driver_car_price.Remove(model);
                await db.SaveChangesAsync();
                TempData["Updated"] = "Đã xóa bảng giá thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa " + ex.ToString();
            }
            return RedirectToRoute("quanlybanggia");
        }

        public ActionResult driverPromotion(int? pg)
        {
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");

            var id_taixe = Config.getCookie("taixelogged").Split(',').Last();
            long id = Convert.ToInt64(id_taixe);

            var taxidn = db.drivers.Find(id);
            if (taxidn == null)
            {
                Config.RemoveCookie("taixelogged"); return RedirectToRoute("taixedangnhap");
            }
            ViewBag.driverId = id;
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;

            var data = db.driver_promotion.Where(x => x.driver_id == id).Select(x => x).ToList();

            return View(data.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult themanhnhaxe(int? pg)
        {
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");

            var id_taixe = Config.getCookie("taixelogged").Split(',').Last();
            long id = Convert.ToInt64(id_taixe);
            var taxidn = db.drivers.Find(id);
            if (taxidn == null)
            {
                Config.RemoveCookie("taixelogged"); return RedirectToRoute("taixedangnhap");
            }
            ViewBag.driverId = id;
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;

            var data = db.driver_images.Where(x => x.driver_id == id).Select(x => x).ToList();

            return View(data.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult uploadimg(long? driver_id)
        {
            var fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\nhaxe", Server.MapPath(@"\")));
                        string strDay = DateTime.Now.ToString("yyyyMM");
                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), strDay);

                        var _fileName = Guid.NewGuid().ToString("N") + ".jpg";

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, _fileName);
                        //System.Drawing.Image bm = System.Drawing.Image.FromStream(file.InputStream);
                        // Thay đổi kích thước ảnh
                        //bm = ResizeBitmap((Bitmap)bm, 100, 100); /// new width, height
                        //// Giảm dung lượng ảnh trước khi lưu
                        //ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                        //ImageCodecInfo ici = null;
                        //foreach (ImageCodecInfo codec in codecs)
                        //{
                        //    if (codec.MimeType == "image/jpeg")
                        //        ici = codec;
                        //}
                        //EncoderParameters ep = new EncoderParameters();
                        //ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)80);
                        //bm.Save(path, ici, ep);
                        //bm.Save(path);
                        file.SaveAs(path);
                        fName = "/Images/nhaxe/" + strDay + "/" + _fileName;

                        var update_img = db.Database.ExecuteSqlCommand("INSERT INTO driver_images(img_url,driver_id) VALUES('" + fName + "'," + driver_id + ")");
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }
            return Json(new { Message = fName }, JsonRequestBehavior.AllowGet);
        }

        

        public string getModaladdPromotion(long? driver_id)
        {
            string html = "";
            html += "<form class=\"form-horizontal\" method=\"post\" id=\"form_add_driver_promotion\" name=\"form_add_driver_promotion\" enctype=\"multipart/form-data\">"
                   + "<input type=\"hidden\" name=\"driver_id\" id=\"driver_id\" value=\"" + driver_id + "\" />"
                   + "<div class=\"form-group\">"
                   + "<div class=\"col-md-12\">"
                   + "<label class=\"control-label\">Thông tin khuyến mại: </label>"
                   + "<textarea rows=\"10\" name=\"promotion_des\" id=\"promotion_des\" class=\"form-control\" placeholder=\"Thông tin khuyến mại\" />"
                   + "</div>"
                   + "</div>"
                   + "<div class=\"form-group\">"
                   + "<div class=\"col-md-12\">"
                   + "<label class=\"control-label\">Trạng thái khuyến mại: </label>"
                   + "<select name=\"promo_status\" id=\"promo_status\" class=\"form-control\">"
                   + "<option value=\"True\">Kích hoạt</option>"
                   + "<option value=\"False\">Kết thúc khuyến mại</option>"
                   + "</select>"
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
                   + "<button type=\"button\" class=\"btn btn-primary\" id=\"btn_saveDriverPromotion\" onclick=\"saveDriverPromotion();\">Cập nhật khuyến mại</button>"
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
               + "$('#to_date').datetimepicker({ format:'Y/m/d' });"
               + "$('#from_date').datetimepicker({ value: s, step: 10, format:'Y/m/d' });"
               + "</script>";
            return html;
        }

        //getModalEditPromotion
        public string getModalEditPromotion(int id)
        {
            string html = "";
            var _promotion = db.driver_promotion.Find(id);
            if (_promotion != null)
            {
                html += "<form class=\"form-horizontal\" method=\"post\" id=\"form_add_driver_promotion\" name=\"form_add_driver_promotion\" enctype=\"multipart/form-data\">"
                   + "<input type=\"hidden\" name=\"driver_id\" id=\"driver_id\" value=\"" + _promotion.driver_id + "\" />"
                   + "<div class=\"form-group\">"
                   + "<div class=\"col-md-12\">"
                   + "<label class=\"control-label\">Thông tin khuyến mại: </label>"
                   + "<textarea rows=\"10\" name=\"promotion_des\" id=\"promotion_des\" class=\"form-control\" placeholder=\"Thông tin khuyến mại\" />"
                   + "</div>"
                   + "</div>"
                   + "<div class=\"form-group\">"
                   + "<div class=\"col-md-12\">"
                   + "<label class=\"control-label\">Trạng thái khuyến mại: </label>"
                   + "<select name=\"promo_status\" id=\"promo_status\" class=\"form-control\">"
                    +"<option value=\"True\" selected>Kích hoạt</option>"
                    + "<option value=\"False\">Kết thúc khuyến mại</option>"
                    +"</select>"
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
                   + "<button type=\"button\" class=\"btn btn-primary\" id=\"btn_saveDriverPromotion\" onclick=\"saveDriverPromotion();\">Cập nhật khuyến mại</button>"
                   + "</form>"
                   + "<script>document.getElementById(\"promotion_des\").value = \"" + _promotion.des + "\";document.getElementById(\"promo_status\").value = \""+ _promotion.status +"\";</script>";

                html += "<script>"
               + "$('#from_date').datetimepicker({"
               + "dayOfWeekStart: 1,"
               + "lang: 'en',"
               + "disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],"
               + "});"
               + "$('#to_date').datetimepicker({"
               + "dayOfWeekStart: 1,"
               + "lang: 'en',"
               + "disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],"               
               + "});"
               + "$('#to_date').datetimepicker({value:'" + _promotion.to_date.Value.ToString("yyyy/MM/dd") + "', format:'Y/m/d'});"
               + "$('#from_date').datetimepicker({value:'" + _promotion.from_date.Value.ToString("yyyy/MM/dd") + "', format:'Y/m/d'});"
               + "</script>";
            }
           
            
            return html;
        }

        [HttpPost]
        public ActionResult deleteDriverPromotion(int? id)
        {
            string deleted = "";
            try
            {
                var km = db.driver_promotion.Find(id);
                db.driver_promotion.Remove(km);
                db.SaveChanges();
                deleted = "1";
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }
            return Json(deleted, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult saveDriverPromotion(long? driver_id, string promotion_des, bool? promo_status, DateTime? from_date, DateTime? to_date)
        {
            string data = ""; string tt_km = ""; string result = "";
            var _promotion = db.driver_promotion.Where(x => x.driver_id == driver_id).FirstOrDefault();
            // chưa có thì thêm mới
            try
            {
                if (_promotion == null)
                {
                    driver_promotion addpromotion = new driver_promotion();
                    addpromotion.driver_id = driver_id ?? null;
                    addpromotion.des = promotion_des ?? null;
                    addpromotion.status = promo_status ?? null;
                    addpromotion.from_date = from_date ?? null;
                    addpromotion.to_date = to_date ?? null;
                    db.driver_promotion.Add(addpromotion);
                    tt_km = addpromotion.status == true ? "Đang khuyến mại" : "Kết thúc khuyến mại";
                    result = "<tr><td>" + addpromotion.des + "</td><td>" + tt_km + "</td><td>" + "<button class=\"btn btn-info\" onclick=\"editDriverPromotion(" + addpromotion.id + ");\"" + "id=\"edit_driver_promotion_" + addpromotion.id + "\">Sửa thông tin khuyến mại</button>" + "</td></tr>";
                }
                else
                {
                    // đã nộp thì cập nhật
                    _promotion.des = promotion_des ?? null;
                    _promotion.status = promo_status ?? null;
                    _promotion.from_date = from_date ?? null;
                    _promotion.to_date = to_date ?? null;
                    db.Entry(_promotion).State = EntityState.Modified;
                    tt_km = _promotion.status == true ? "Đang khuyến mại" : "Kết thúc khuyến mại";
                    //result = "<tr><td>" + _promotion.des + "</td><td>" + tt_km + "</td><td>" + "<button class=\"btn btn-info\" onclick=\"editDriverPromotion(" + _promotion.id + ");\"" + "id=\"edit_driver_promotion_" + _promotion.id + "\">Sửa thông tin khuyến mại</button>" + "</td></tr>";
                    result = "1";
                }
                db.SaveChanges();

                data += result;
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult khachdatxe(string k, int? pg)
        {
            if (k == null) k = "";
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");

            var id_taixe = Config.getCookie("taixelogged").Split(',').Last();
            long id = Convert.ToInt64(id_taixe);

            var taxidn = db.drivers.Find(id);
            if (taxidn == null)
            {
                Config.RemoveCookie("taixelogged"); return RedirectToRoute("taixedangnhap");
            }
            var p = db.booking_to_driver.Where(x => x.driver_id == id).Select(x=>x);
            if (k != null && k != "")
            {
                p = p.Where(x => x.customer_name.Contains(k));
            }
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            return View(p.ToList().ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult deleteKhachDatXe(long? id)
        {
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");
            var id_taixe = Config.getCookie("taixelogged").Split(',').Last();
            long id_driver = Convert.ToInt64(id_taixe);
            string deleted = "";
            var khach = db.booking_to_driver.Where(x=>x.id == id && x.driver_id == id_driver).Select(x=>x).FirstOrDefault();
            if (khach != null)
            {
                db.booking_to_driver.Remove(khach);
                db.SaveChanges();
                deleted = "1";
            }

            return Json(deleted, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult deleteDriverImage(long? id)
        {
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");
            var id_taixe = Config.getCookie("taixelogged").Split(',').Last();
            long id_driver = Convert.ToInt64(id_taixe);
            string deleted = "";
            var image = db.driver_images.Where(x => x.id == id && x.driver_id == id_driver).Select(x => x).FirstOrDefault();
            if (image != null)
            {
                db.driver_images.Remove(image);
                db.SaveChanges();
                deleted = "1";
            }

            return Json(deleted, JsonRequestBehavior.AllowGet);
        }

        //public string getNoteTaiXe(long? id)
        //{
        //    var driver_info = (from s in db.driver_info where s.driver_id == id select s.driver_note).FirstOrDefault();
        //    return driver_info;
        //}

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult update_driver_info(HttpPostedFileBase _filecover, HttpPostedFileBase _fileprofile, driver_info model)
        {
            var data = new object() { };
            string fCover = null;
            string fProfile = null;
            if (Request.Files.Count > 0)
            {
                _filecover = Request.Files["filecover-0"];
                _fileprofile = Request.Files["fileprofile-0"];
            }
            //Save file content goes here
            if (_filecover != null && _filecover.ContentLength > 0)
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Image\\nhaxe", Server.MapPath(@"\")));
                string pathString = System.IO.Path.Combine(originalDirectory.ToString());
                string basicUID = Guid.NewGuid().ToString("N");
                var _fileName = basicUID + ".jpg";
                bool isExists = System.IO.Directory.Exists(pathString);
                if (!isExists)
                    System.IO.Directory.CreateDirectory(pathString);

                var path = string.Format("{0}\\{1}", pathString, _fileName);

                _filecover.SaveAs(path);
                fCover = "/Image/nhaxe/" + _fileName;
            }

            if (_fileprofile != null && _fileprofile.ContentLength > 0)
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Image\\nhaxe", Server.MapPath(@"\")));
                string pathString = System.IO.Path.Combine(originalDirectory.ToString());
                string basicUID = Guid.NewGuid().ToString("N");
                var _fileName = basicUID + ".jpg";
                bool isExists = System.IO.Directory.Exists(pathString);
                if (!isExists)
                    System.IO.Directory.CreateDirectory(pathString);

                var path = string.Format("{0}\\{1}", pathString, _fileName);

                _fileprofile.SaveAs(path);
                fProfile = "/Image/nhaxe/" + _fileName;
            }

            if (model.id == 0)
            {
                driver_info newdi = new driver_info();
                newdi.driver_id = model.driver_id ?? null;
                newdi.driver_img_cover = fCover ?? null;
                newdi.driver_img_profile = fProfile ?? null;
                newdi.driver_des = model.driver_des ?? null;
                newdi.driver_note = model.driver_note ?? null;
                db.driver_info.Add(newdi);
                db.SaveChanges();
                data = newdi;
            }
            else
            {
                var editdi = db.driver_info.Find(model.id);
                if (editdi != null)
                {
                    editdi.driver_id = model.driver_id ?? null;
                    if (fCover != null)
                    {
                        editdi.driver_img_cover = fCover;
                    }
                    if (fProfile != null)
                    {
                        editdi.driver_img_profile = fProfile;
                    }
                    
                    editdi.driver_des = model.driver_des ?? null;
                    editdi.driver_note = model.driver_note ?? null;
                    db.Entry(editdi).State = EntityState.Modified;
                    db.SaveChanges();
                    data = editdi;
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getInfoTaiXe(long? id)
        {
            var data = new object() { };
            try
            {
                var driver_info = (from s in db.driver_info where s.driver_id == id select s).FirstOrDefault();
                data = driver_info;
            }
            catch
            {                
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult updateNote(long? id, string note_driver)
        //{
        //    string updated = "";
        //    try
        //    {
        //        var update = (from s in db.driver_info where s.driver_id == id select s).FirstOrDefault();
        //        if (update != null)
        //        {
        //            update.driver_note = note_driver ?? null;
        //            db.Entry(update).State = EntityState.Modified;
        //            db.SaveChanges();
        //            updated = "1";
        //        }
        //        else
        //        {
        //            driver_info ghichu = new driver_info();
        //            ghichu.driver_id = id;
        //            ghichu.driver_note = note_driver ?? null;
        //            db.driver_info.Add(ghichu);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Config.SaveTolog(ex.ToString());
        //    }
        //    return Json(updated, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult updateInfo(long? id, string des)
        //{
        //    string updated = "";
        //    try
        //    {
        //        var update = (from s in db.driver_info where s.driver_id == id select s).FirstOrDefault();
        //        if (update != null)
        //        {
        //            update.driver_des = des ?? null;
        //            db.Entry(update).State = EntityState.Modified;
        //            db.SaveChanges();
        //            updated = "1";
        //        }
        //        else
        //        {
        //            driver_info ghichu = new driver_info();
        //            ghichu.driver_id = id;
        //            ghichu.driver_des = des ?? null;
        //            db.driver_info.Add(ghichu);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Config.SaveTolog(ex.ToString());
        //    }
        //    return Json(updated, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult driverEdit()
        {
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");
            var id_taixe = Config.getCookie("taixelogged").Split(',').Last();
            long id_driver = Convert.ToInt64(id_taixe);

            driver driver = db.drivers.Find(id_driver);
            string phone = driver.phone;
            //string car_number=driver.car_number;
            var p = db.list_online.Where(o => o.phone == phone).FirstOrDefault();
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
        public ActionResult driverEdit([Bind(Include = "id,name,driver_type,phone,email,car_model,car_made,car_years,car_size,car_number,car_type,car_price,total_moneys,province,date_time,code,address, lon, lat")] driver driver)
        {
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");
            if (ModelState.IsValid)
            {
                db.Entry(driver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(driver);
        }


    }
}