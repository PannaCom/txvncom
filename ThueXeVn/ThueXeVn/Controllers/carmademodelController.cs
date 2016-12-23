using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;

namespace ThueXeVn.Controllers
{
    public class carmademodelController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: carmademodel
        public ActionResult Index()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            return View(db.car_made_model.ToList());
        }

        // GET: carmademodel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            car_made_model car_made_model = db.car_made_model.Find(id);
            if (car_made_model == null)
            {
                return HttpNotFound();
            }
            return View(car_made_model);
        }

        // GET: carmademodel/Create
        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            ViewBag.list_car = db.list_car.Select(x => new SelectListItem() { Value = x.name, Text = x.name });
            return View();
        }

        // POST: carmademodel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,made,model,image")] car_made_model car_made_model)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (car_made_model.made == null && car_made_model.model == null)
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin các trường.");
                ViewBag.list_car = db.list_car.Select(x => new SelectListItem() { Value = x.name, Text = x.name });
                return View();
            }
            if (ModelState.IsValid)
            {
                db.car_made_model.Add(car_made_model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(car_made_model);
        }

        // GET: carmademodel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            car_made_model car_made_model = db.car_made_model.Find(id);
            ViewBag.list_car = db.list_car.Select(x => new SelectListItem() { Value = x.name, Text = x.name });
            if (car_made_model == null)
            {
                return HttpNotFound();
            }
            return View(car_made_model);
        }

        // POST: carmademodel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,made,model,image")] car_made_model car_made_model)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (car_made_model.made == null && car_made_model.model == null)
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin các trường.");
                ViewBag.list_car = db.list_car.Select(x => new SelectListItem() { Value = x.name, Text = x.name });
                return View();
            }
            if (ModelState.IsValid)
            {
                db.Entry(car_made_model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car_made_model);
        }

        // GET: carmademodel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            car_made_model car_made_model = db.car_made_model.Find(id);
            if (car_made_model == null)
            {
                return HttpNotFound();
            }
            return View(car_made_model);
        }

        // POST: carmademodel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            car_made_model car_made_model = db.car_made_model.Find(id);
            db.car_made_model.Remove(car_made_model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult upanh()
        {
            bool isSaved = true;
            var fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(string.Format("{0}images\\carmodel", Server.MapPath(@"\")));
                        string strDay = DateTime.Now.ToString("yyyyMM");
                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), strDay);

                        var _fileName = Guid.NewGuid().ToString("N") + ".jpg";

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, _fileName);
                        System.Drawing.Image bm = System.Drawing.Image.FromStream(file.InputStream);
                        // Thay đổi kích thước ảnh
                        bm = ResizeBitmap((Bitmap)bm, 100, 100); /// new width, height
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
                        bm.Save(path);
                        //file.SaveAs(path);
                        fName = "/images/carmodel/" + strDay + "/" + _fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                isSaved = false;
                Config.SaveTolog(ex.ToString());
            }
            if (isSaved)
            {
                return Json(new { Message = fName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = "Có lỗi khi lưu tệp tin" }, JsonRequestBehavior.AllowGet);
            }
        }

        private Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight)
        {
            Bitmap result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage((System.Drawing.Image)result))
                g.DrawImage(b, 0, 0, nWidth, nHeight);
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
