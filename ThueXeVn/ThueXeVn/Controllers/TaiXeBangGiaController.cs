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

            var sql = "SELECT id,cp_car_type,cp_price,cp_multiple,cp_multiple2,driver_id FROM driver_car_price where driver_id = " + id;

            var data = db.Database.SqlQuery<driver_car_price>(sql).ToList().OrderBy(s => s.cp_car_type);

            return View(data.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Addbanggia()
        {
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");           

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addbanggia(string cp_car_type, string cp_price)
        {
            
            if (Config.getCookie("taixelogged") != null)
            {
                var id_taixe = Config.getCookie("taixelogged").Split(',').Last();
                long id = Convert.ToInt64(id_taixe);
                var taxidn = db.drivers.Find(id);
                if (taxidn == null)
                {
                    Config.RemoveCookie("taixelogged"); return RedirectToRoute("taixedangnhap");
                }
                if (cp_car_type == "") cp_car_type = "null"; if (cp_price == "") cp_price = "null"; 
                //if (cp_multiple == "") { cp_multiple = "null"; } if (cp_multiple2 == "") { cp_multiple2 = "null"; }
                try
                {
                    var sql = "INSERT INTO driver_car_price(cp_car_type,cp_price,driver_id) VALUES(" + cp_car_type + "," + cp_price + "," + id+")";
                    var addbanggiaxe = db.Database.ExecuteSqlCommand(sql);
                    TempData["Updated"] = "Đã thêm mới bảng giá.";
                    return RedirectToRoute("quanlybanggia");
                }
                catch(Exception ex)
                {
                    Config.SaveTolog(ex.ToString());
                    ModelState.AddModelError("", "Vui lòng kiểm tra lại các trường.");
                    return View();
                }
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
        public ActionResult Edit(long? cp_id, string cp_car_type, string cp_price)
        {

            if (Config.getCookie("taixelogged") != null)
            {
                var id_taixe = Config.getCookie("taixelogged").Split(',').Last();
                long id = Convert.ToInt64(id_taixe);
                var taxidn = db.drivers.Find(id);
                if (taxidn == null)
                {
                    Config.RemoveCookie("taixelogged"); return RedirectToRoute("taixedangnhap");
                }
                if (cp_car_type == "") cp_car_type = "null"; if (cp_price == "") cp_price = "null"; 
                //if (cp_multiple == "") { cp_multiple = "null"; } if (cp_multiple2 == "") { cp_multiple2 = "null"; }
                try
                {
                    var sql = "update driver_car_price set cp_car_type = " + cp_car_type + ", cp_price = " + cp_price + " where id = " + cp_id;
                    var updatebanggiaxe = db.Database.ExecuteSqlCommand(sql);
                    TempData["Updated"] = "Đã Cập nhật mới bảng giá.";
                }
                catch (Exception ex)
                {
                    Config.SaveTolog(ex.ToString());
                    ModelState.AddModelError("", "Vui lòng kiểm tra lại các trường.");
                    return View();
                }
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
                   + "<button type=\"button\" class=\"btn btn-primary\" id=\"btn_saveDriverPromotion\" onclick=\"saveDriverPromotion();\">Cập nhật khuyến mại</button>"
                   + "</form>";
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
                   + "<button type=\"button\" class=\"btn btn-primary\" id=\"btn_saveDriverPromotion\" onclick=\"saveDriverPromotion();\">Cập nhật khuyến mại</button>"
                   + "</form>"
                   + "<script>document.getElementById(\"promotion_des\").value = \"" + _promotion.des + "\";document.getElementById(\"promo_status\").value = \""+ _promotion.status +"\";</script>";
            }
           
            
            return html;
        }

        [HttpPost]
        public ActionResult saveDriverPromotion(long? driver_id, string promotion_des, bool? promo_status)
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
                    db.driver_promotion.Add(addpromotion);
                    tt_km = addpromotion.status == true ? "Đang khuyến mại" : "Kết thúc khuyến mại";
                    result = "<tr><td>" + addpromotion.des + "</td><td>" + tt_km + "</td><td>" + "<button class=\"btn btn-info\" onclick=\"editDriverPromotion(" + addpromotion.id + ");\"" + "id=\"edit_driver_promotion_" + addpromotion.id + "\">Sửa thông tin khuyến mại</button>" + "</td></tr>";
                }
                else
                {
                    // đã nộp thì cập nhật
                    _promotion.des = promotion_des ?? null;
                    _promotion.status = promo_status ?? null;
                    db.Entry(_promotion).State = EntityState.Modified;
                    tt_km = _promotion.status == true ? "Đang khuyến mại" : "Kết thúc khuyến mại";
                    result = "<tr><td>" + _promotion.des + "</td><td>" + tt_km + "</td><td>" + "<button class=\"btn btn-info\" onclick=\"editDriverPromotion(" + _promotion.id + ");\"" + "id=\"edit_driver_promotion_" + _promotion.id + "\">Sửa thông tin khuyến mại</button>" + "</td></tr>";
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
            return View(p.ToPagedList(pageNumber, pageSize));
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

        public string getNoteTaiXe(long? id)
        {
            var driver_info = (from s in db.driver_info where s.driver_id == id select s.driver_note).FirstOrDefault();
            return driver_info;
        }

        public string getInfoTaiXe(long? id)
        {
            var driver_info = (from s in db.driver_info where s.driver_id == id select s.driver_des).FirstOrDefault();
            return driver_info;
        }

        [HttpPost]
        public ActionResult updateNote(long? id, string notes)
        {
            string updated = "";
            try
            {
                var update = (from s in db.driver_info where s.driver_id == id select s).FirstOrDefault();
                if (update != null)
                {
                    update.driver_note = notes ?? null;
                    db.Entry(update).State = EntityState.Modified;
                    db.SaveChanges();
                    updated = "1";
                }
                else
                {
                    driver_info ghichu = new driver_info();
                    ghichu.driver_id = id;
                    ghichu.driver_note = notes ?? null;
                    db.driver_info.Add(ghichu);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }
            return Json(updated, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult updateInfo(long? id, string des)
        {
            string updated = "";
            try
            {
                var update = (from s in db.driver_info where s.driver_id == id select s).FirstOrDefault();
                if (update != null)
                {
                    update.driver_des = des ?? null;
                    db.Entry(update).State = EntityState.Modified;
                    db.SaveChanges();
                    updated = "1";
                }
                else
                {
                    driver_info ghichu = new driver_info();
                    ghichu.driver_id = id;
                    ghichu.driver_des = des ?? null;
                    db.driver_info.Add(ghichu);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }
            return Json(updated, JsonRequestBehavior.AllowGet);
        }

    }
}