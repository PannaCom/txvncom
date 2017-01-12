using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;
using PagedList;
using PagedList.Mvc;

namespace ThueXeVn.Controllers
{
    public class CongTacVienController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: CongTacVien
        public ActionResult Index()
        {
            if (Config.getCookie("ctvlogged") == "") return RedirectToRoute("congtacviendangnhap");

            var id_ctv = Config.getCookie("ctvlogged").Split('-').Last();
            int id = Convert.ToInt32(id_ctv);
            var ctv = db.ctv_tiepthi.Find(id);
            if (ctv == null)
            {
                return RedirectToRoute("congtacviendangnhap");
            }

            string url = Request.Url.Authority;            
            if (Request.ServerVariables["HTTPS"] == "on")
            {
                url = "https://" + url;
            }
            else
            {
                url = "http://" + url;
            }

            string link = url + "/dat-xe?utm=" + ctv.ctv_phone;
            ViewBag.linkshare = link;
            return View();
        }

        //public ActionResult getlink()
        //{
        //    if (Config.getCookie("ctvlogged") == "") return RedirectToRoute("congtacviendangnhap");

        //    var id_ctv = Config.getCookie("ctvlogged").Split('-').Last();
        //    int id = Convert.ToInt32(id_ctv);
        //    var ctv = db.ctv_tiepthi.Find(id);
        //    if (ctv == null)
        //    {
        //        return RedirectToRoute("congtacviendangnhap");
        //    }

        //    string url = Request.Url.Authority;
        //    if (Request.ServerVariables["HTTPS"] == "on")
        //    {
        //        url = "https://" + url;
        //    }
        //    else
        //    {
        //        url = "http://" + url;
        //    }

        //    string link = url + "/dat-xe?utm=" + ctv.ctv_phone;
        //    return Json(link, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult danhsachdatxe(int? pg, DateTime? tg_dx, string tt, string search, string car_hire_type)
        {
            if (Config.getCookie("ctvlogged") == "") return RedirectToRoute("congtacviendangnhap");

            var id_ctv = Config.getCookie("ctvlogged").Split('-').Last();
            int id = Convert.ToInt32(id_ctv);
            var _ctv = db.ctv_tiepthi.Find(id);
            if (_ctv != null)
            {
                ViewBag.soluottiepthi = _ctv.point_share;
            }
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;

            var sql = "SELECT t1.id, t1.name, t1.phone, t1.date_time, t1.date_from, t1.date_to, t1.car_from, t1.car_to, t1.car_hire_type, t1.status, t1.status2 FROM booking t1 left JOIN booking_ctv_tiepthi t2 ON t1.id = t2.booking_id left JOIN ctv_tiepthi t3 ON t2.ctv_id = t3.ctv_id where t3.ctv_id =" + id;
            if (search == null) search = ""; if (tt == null) tt = ""; if (car_hire_type == null) car_hire_type = "";
            if (tg_dx != null)
            {
                sql += "and t1.date_time >= '" + tg_dx + "'";
                ViewBag.tg_dx = tg_dx;
            }
            
            if (tt != null && tt != "")
            {
                sql += "and t1.status2 = " + tt;
                ViewBag.tt = tt;
            }

            if (search != null && search != "")
            {
                sql += "and t1.name like N'%" + search + "%'";
                ViewBag.search = search;
            }

            if (car_hire_type != null && car_hire_type != "")
            {
                sql += "and t1.car_hire_type like N'%" + car_hire_type + "%'";
                ViewBag.car_hire_type = car_hire_type;
            }

            var data = db.Database.SqlQuery<bookingVM>(sql).ToList().OrderByDescending(s=>s.date_time);
            
            return View(data.ToList().ToPagedList(pageNumber, pageSize));
        }

        

    }
}