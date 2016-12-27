using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;

namespace ThueXeVn.Controllers
{
    public class ListController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: List
        public ActionResult bangke()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");

            return View();
        }
    }
}