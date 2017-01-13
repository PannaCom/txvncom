using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;
using PagedList;
using PagedList.Mvc;
using System.Threading.Tasks;

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
        public ActionResult Addbanggia(string cp_car_type, string cp_price, string cp_multiple, string cp_multiple2)
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
                if (cp_car_type == "") cp_car_type = "null"; if (cp_price == "") cp_price = "null"; if (cp_multiple == "") { cp_multiple = "null"; } if (cp_multiple2 == "") { cp_multiple2 = "null"; }
                try
                {
                    var sql = "INSERT INTO driver_car_price(cp_car_type,cp_price,cp_multiple,cp_multiple2,driver_id) VALUES(" + cp_car_type + "," + cp_price + "," + cp_multiple + "," + cp_multiple2 + "," + id+")";
                    var addbanggiaxe = db.Database.ExecuteSqlCommand(sql);
                    TempData["Updated"] = "Đã thêm mới bảng giá.";
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
        public ActionResult Edit(long? cp_id, string cp_car_type, string cp_price, string cp_multiple, string cp_multiple2)
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
                if (cp_car_type == "") cp_car_type = "null"; if (cp_price == "") cp_price = "null"; if (cp_multiple == "") { cp_multiple = "null"; } if (cp_multiple2 == "") { cp_multiple2 = "null"; }
                try
                {
                    var sql = "update driver_car_price set cp_car_type = " + cp_car_type + ", cp_price = " + cp_price + ", cp_multiple = " + cp_multiple + ", cp_multiple2 = " + cp_multiple2 + " where id = " + cp_id;
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

    }
}