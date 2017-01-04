using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;
using Excel;
using System.Data;
using System.IO;
using System.Globalization;
using PagedList;
using PagedList.Mvc;

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
        public ActionResult bangke(int? pg, string search)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.invoices select q);
            if (data == null)
            {
                return View(data);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(x => x.customer_name.Contains(search));
                ViewBag.search = search;
            }

            data = data.OrderBy(x => x.id);

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult bangke(DateTime date, string customer_name, string sex, string phone, DateTime time_to_pick, DateTime time_to_pay, string pickup, string paypoints, int so_cho, string form, string driver, float price, float vat, float sum, string note)
        {
            try
            {
                invoice _new = new invoice();
                _new.date = date != null ? (DateTime?)Convert.ToDateTime(date) : null;
                _new.customer_name = customer_name ?? null;
                _new.sex = sex ?? null;
                _new.phone = phone ?? null;
                _new.time_to_pick = time_to_pick != null ? (DateTime?)Convert.ToDateTime(time_to_pick) : null;
                _new.time_to_pay = time_to_pay != null ? (DateTime?)Convert.ToDateTime(time_to_pay) : null;
                _new.pickup = pickup ?? null;
                _new.paypoints = paypoints ?? null;
                _new.so_cho = (int?)so_cho ?? null;
                _new.form = form ?? null;
                _new.driver = driver ?? null;
                _new.price = (Double?)price ?? null;
                _new.vat = (Double?)vat ?? null;
                _new.sum = (Double?)sum ?? null;
                _new.note = note ?? null;
                db.invoices.Add(_new);
                db.SaveChanges();
                TempData["Updated"] = "Thêm dữ liệu thành công";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi thêm mới. " + ex.ToString();
            }
            return RedirectToAction("bangke");
        }

        [HttpPost]
        public ActionResult ImportToExcel()
        {
            DataSet ds = new DataSet();
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileExtension = System.IO.Path.GetExtension(file.FileName);

                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            string fileLocation = Server.MapPath("~/Content/") + file.FileName;
                            if (System.IO.File.Exists(fileLocation))
                            {
                                System.IO.File.Delete(fileLocation);
                            }
                            file.SaveAs(fileLocation);

                            // đọc file excel tại đây
                            FileStream stream = System.IO.File.Open(fileLocation, FileMode.Open, FileAccess.Read);

                            //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                            //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

                            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                            ds = excelReader.AsDataSet();
                            if (ds == null)
                            {
                                return null;
                            }

                            //5. Data Reader methods
                            excelReader.Read();
                            //while (excelReader.Read())
                            //{
                            //}

                            for (int i = 2; i < ds.Tables[0].Rows.Count; i++)
                            {
                                //var strngathang = ds.Tables[0].Rows[i][2].ToString();
                                //if (ds.Tables[0].Rows[i][2].ToString().Length == 4)
                                //{
                                //    strngathang = "01.01." + ds.Tables[0].Rows[i][2].ToString().Substring(2, 2);
                                //}
                                //var ngaythang = strngathang == "" ? (DateTime?)null : DateTime.FromOADate(double.Parse(strngathang));
                                var c1 = ""; string c2 = null; string c3 = ""; string c4 = ""; string c5 = ""; string c6 = ""; string c7 = ""; string c8 = ""; string c9 = ""; string c10 = ""; string c11 = ""; string c12 = ""; string c13 = ""; string c14 = ""; string c15 = "";

                                var _date = ds.Tables[0].Rows[i][0].ToString();
                                var dt = DateTime.ParseExact(_date, "dd/MM/yyyy", null);
                                c1 = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                c2 = ds.Tables[0].Rows[i][1] != null ? ds.Tables[0].Rows[i][1].ToString() : null;
                                c3 = ds.Tables[0].Rows[i][2] != null ? ds.Tables[0].Rows[i][2].ToString() : null;
                                c4 = ds.Tables[0].Rows[i][3] != null ? ds.Tables[0].Rows[i][3].ToString() : null;
                                var _date2 = ds.Tables[0].Rows[i][4] != null ? ds.Tables[0].Rows[i][4] : null;
                                if (_date2 != null)
                                {
                                    
                                    try
                                    {
                                        c5 = DateTime.ParseExact(_date2.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            c5 = DateTime.ParseExact(_date2.ToString(), "dd/MM/yyyy HH:mm", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                c5 = DateTime.ParseExact(_date2.ToString(), "dd/MM/yyyy h:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    c5 = DateTime.ParseExact(_date2.ToString(), "dd/MM/yyyy h:mm", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                }
                                                catch
                                                {
                                                    try
                                                    {
                                                        c5 = DateTime.ParseExact(_date2.ToString(), "dd/MM/yyyy HH:mm:ss T", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                    }
                                                    catch
                                                    {
                                                        try
                                                        {
                                                            c5 = DateTime.ParseExact(_date2.ToString(), "dd/MM/yyyy h:mm:ss T", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                        }
                                                        catch
                                                        {
                                                            c5 = "";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (Convert.ToDateTime(c5).Month < 10 || Convert.ToDateTime(c5).Day < 10)
                                {
                                    c5 = DateTime.ParseExact(c5, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                }

                                var _date3 = ds.Tables[0].Rows[i][5] != null ? ds.Tables[0].Rows[i][5] : null;
                                if (_date3 != null)
                                {
                                    if (DateTime.ParseExact(_date3.ToString(), "dd/MM/yyyy", null).Month < 10 || DateTime.ParseExact(_date3.ToString(), "dd/MM/yyyy", null).Day < 10)
                                    {
                                        _date3 = DateTime.ParseExact(_date3.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                                    }
                                    try
                                    {
                                        c6 = DateTime.ParseExact(_date3.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            c6 = DateTime.ParseExact(_date3.ToString(), "dd/MM/yyyy HH:mm", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                c6 = DateTime.ParseExact(_date3.ToString(), "dd/MM/yyyy h:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    c6 = DateTime.ParseExact(_date3.ToString(), "dd/MM/yyyy h:mm", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                }
                                                catch
                                                {
                                                    try
                                                    {
                                                        c6 = DateTime.ParseExact(_date3.ToString(), "dd/MM/yyyy HH:mm:ss T", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                    }
                                                    catch
                                                    {
                                                        try
                                                        {
                                                            c6 = DateTime.ParseExact(_date3.ToString(), "dd/MM/yyyy h:mm:ss T", null).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                        }
                                                        catch
                                                        {
                                                            c6 = "";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }

                                c7 = ds.Tables[0].Rows[i][6] != null ? ds.Tables[0].Rows[i][6].ToString() : null;
                                c8 = ds.Tables[0].Rows[i][7] != null ? ds.Tables[0].Rows[i][7].ToString() : null;
                                c9 = ds.Tables[0].Rows[i][8] != null ? ds.Tables[0].Rows[i][8].ToString() : null;
                                c10 = ds.Tables[0].Rows[i][9] != null ? ds.Tables[0].Rows[i][9].ToString() : null;
                                c11 = ds.Tables[0].Rows[i][10] != null ? ds.Tables[0].Rows[i][10].ToString() : null;
                                c12 = ds.Tables[0].Rows[i][11] != null ? ds.Tables[0].Rows[i][11].ToString() : null;
                                c13 = ds.Tables[0].Rows[i][12] != null ? ds.Tables[0].Rows[i][12].ToString() : null;
                                c14 = ds.Tables[0].Rows[i][13] != null ? ds.Tables[0].Rows[i][13].ToString() : null;
                                c15 = ds.Tables[0].Rows[i][14] != null ? ds.Tables[0].Rows[i][14].ToString() : null;

                                //string sql = string.Format("insert into invoices(date, customer_name, sex, phone, time_to_pick, time_to_pay, pickup, paypoints, so_cho, form, driver, price, vat, sum, note) values('{0}', '{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}', '{13}', '{14}')", c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12, c13, c14, c15);

                                var bcheckempty = c1 == null && c2 == null && c3 == null && c4 == null && c5 == null && c6 == null && c7 == null && c8 == null && c9 == null && c10 == null && c11 == null && c12 == null && c13 == null && c14 == null && c15 == null;
                                if (!bcheckempty)
                                {
                                    string sql2 = @"INSERT INTO invoices(date, customer_name, sex, phone, time_to_pick, time_to_pay, pickup, paypoints, so_cho, form, driver, price, vat, sum, note) SELECT '" + c1 + "',N'" + c2 + "','" + c3 + "','" + c4 + "','" + c5 + "','" + c6 + "','" + c7 + "','" + c8 + "','" + c9 + "','" + c10 + "','" + c11 + "'," + c12 + "," + c13 + "," + c14 + ",'" + c15 + "' WHERE NOT EXISTS (SELECT date, customer_name, sex, phone FROM invoices WHERE date = '" + c1 + "' AND customer_name = N'" + c2 + "' AND sex = '" + c3 + "' AND phone = '" + c4 + "')";

                                    int noOfRowInserted = db.Database.ExecuteSqlCommand(sql2);
                                }                              
                                
                                // đóng vòng lặp
                                if (i == ds.Tables[0].Rows.Count - 1)
                                {
                                    excelReader.Close();
                                    break;
                                }
                            }

                            //6. Free resources (IExcelDataReader is IDisposable)


                        }


                    }
                }
                TempData["Updated"] = "Thêm dữ liệu thành công";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi thêm mới. " + ex.ToString();
            }

            return RedirectToAction("bangke");

        }


    }
}