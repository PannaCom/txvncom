using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThueXeVn.Controllers
{
    public class TaiXeBangGiaController : Controller
    {

        // GET: TaiXeBangGia
        public ActionResult Index()
        {
            if (Config.getCookie("taixelogged") == "") return RedirectToRoute("taixedangnhap");
            return View();
        }
    }
}