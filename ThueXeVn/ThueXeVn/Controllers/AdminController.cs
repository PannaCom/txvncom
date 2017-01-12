using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using ThueXeVn.Models;

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

    }
}