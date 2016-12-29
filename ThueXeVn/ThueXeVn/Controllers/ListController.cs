using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;
using Excel;

namespace ThueXeVn.Controllers
{
    public class ListController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        public ActionResult Index()
        {
            return View();
        }

        // GET: List
        public ActionResult bangke()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");


            return View();
        }

        //[HttpPost]
        //public ActionResult ImportToExcel()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        if (Request.Files.Count > 0)
        //        {
        //            var file = Request.Files[0];
        //            if (file != null && file.ContentLength > 0)
        //            {
        //                string fileExtension = System.IO.Path.GetExtension(file.FileName);

        //                if (fileExtension == ".xls" || fileExtension == ".xlsx")
        //                {
        //                    string fileLocation = Server.MapPath("~/Content/") + file.FileName;
        //                    if (System.IO.File.Exists(fileLocation))
        //                    {
        //                        System.IO.File.Delete(fileLocation);
        //                    }
        //                    file.SaveAs(fileLocation);

        //                    // đọc file excel tại đây
        //                    FileStream stream = System.IO.File.Open(fileLocation, FileMode.Open, FileAccess.Read);

        //                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
        //                    //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

        //                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
        //                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        //                    //3. DataSet - The result of each spreadsheet will be created in the result.Tables
        //                    ds = excelReader.AsDataSet();
        //                    if (ds == null)
        //                    {
        //                        return null;
        //                    }

        //                    //5. Data Reader methods
        //                    excelReader.Read();
        //                    //while (excelReader.Read())
        //                    //{
        //                    //}
        //                    for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
        //                    {
        //                        //var strngathang = ds.Tables[0].Rows[i][2].ToString();
        //                        //if (ds.Tables[0].Rows[i][2].ToString().Length == 4)
        //                        //{
        //                        //    strngathang = "01.01." + ds.Tables[0].Rows[i][2].ToString().Substring(2, 2);
        //                        //}
        //                        //var ngaythang = strngathang == "" ? (DateTime?)null : DateTime.FromOADate(double.Parse(strngathang));


        //                        string sql = string.Format("insert into Publications(f1, f2, f3, f4, f5, f6, f7, f8) values('{0}', '{1}','{2}','{3}','{4}','{5}','{6}','{7}')", ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][1].ToString().Replace("'", "''"), ds.Tables[0].Rows[i][2].ToString(), ds.Tables[0].Rows[i][3].ToString().Replace("'", "''"), ds.Tables[0].Rows[i][4].ToString(), ds.Tables[0].Rows[i][5].ToString(), ds.Tables[0].Rows[i][6].ToString(), ds.Tables[0].Rows[i][7].ToString());

        //                        int noOfRowInserted = db.Database.ExecuteSqlCommand(sql);

        //                        // đóng vòng lặp
        //                        if (i == ds.Tables[0].Rows.Count - 1)
        //                        {
        //                            excelReader.Close();
        //                            break;
        //                        }
        //                    }

        //                    //6. Free resources (IExcelDataReader is IDisposable)


        //                }


        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return RedirectToAction("Index");

        //}


    }
}