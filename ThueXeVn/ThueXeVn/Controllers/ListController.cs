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
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

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
        [ValidateAntiForgeryToken]
        public ActionResult Edit(long? id, DateTime date, string customer_name, string sex, string phone, DateTime time_to_pick, DateTime time_to_pay, string pickup, string paypoints, int so_cho, string form, string driver, float? price, float? vat, float? sum, string note)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("bangke");
            }
            var model = (from s in db.invoices where s.id == id select s).FirstOrDefault();
            if (model != null)
            {
                try
                {
                    model.date = date != null ? (DateTime?)Convert.ToDateTime(date) : null;
                    model.customer_name = customer_name ?? null;
                    model.sex = sex ?? null;
                    model.phone = phone ?? null;
                    model.time_to_pick = time_to_pick != null ? (DateTime?)Convert.ToDateTime(time_to_pick) : null;
                    model.time_to_pay = time_to_pay != null ? (DateTime?)Convert.ToDateTime(time_to_pay) : null;
                    model.pickup = pickup ?? null;
                    model.paypoints = paypoints ?? null;
                    model.so_cho = (int?)so_cho ?? null;
                    model.form = form ?? null;
                    model.driver = driver ?? null;
                    model.price = (Double?)price ?? null;
                    model.vat = (Double?)vat ?? null;
                    model.sum = (Double?)sum ?? null;
                    model.note = note ?? null;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Đã xảy ra lỗi khi thêm mới. " + ex.ToString();
                }
            }
            TempData["Updated"] = "Cập nhật dữ liệu thành công";
            return RedirectToAction("bangke");
        }


        public ActionResult Edit(long? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("bangke");
            }
            var model = (from s in db.invoices where s.id == id select s).FirstOrDefault();

            return View(model);
        }

        //DeleteBuild
        public ActionResult Delete(long? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToRoute("bangke");
            }
            invoice model = db.invoices.Find(id);
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
            invoice model = await db.invoices.FindAsync(id);
            if (model == null)
            {
                return View();
            }

            try
            {
                db.invoices.Remove(model);
                await db.SaveChangesAsync();
                TempData["Deleted"] = "Dữ liệu đã được xóa.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa " + ex.ToString();
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
                            string fileLocation = Server.MapPath("~/App_Data/");

                            //if (System.IO.File.Exists(fileLocation))
                            //{
                            //    System.IO.File.Delete(fileLocation);
                            //}

                            string strDay = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), file.FileName);
                            string pathString = System.IO.Path.Combine(fileLocation, strDay);

                            file.SaveAs(pathString);

                            // đọc file excel tại đây
                            FileStream stream = System.IO.File.Open(pathString, FileMode.Open, FileAccess.Read);

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
                            int countc = ds.Tables[0].Rows.Count;
                            for (int i = 2; i < countc; i++)
                            {
                                var c1 = ""; string c2 = null; string c3 = ""; string c4 = ""; string c5 = ""; string c6 = ""; string c7 = ""; string c8 = ""; string c9 = ""; string c10 = ""; string c11 = ""; string c12 = ""; string c13 = ""; string c14 = ""; string c15 = "";

                                var _date = ds.Tables[0].Rows[i][0] != null ? ds.Tables[0].Rows[i][0].ToString() : "";
                                try
                                {
                                    c1 = _date != "" ? DateTime.ParseExact(_date, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm") : "";
                                }
                                catch
                                {
                                    c1 = "";
                                }
                                
                                c2 = ds.Tables[0].Rows[i][1] != null ? ds.Tables[0].Rows[i][1].ToString() : "";
                                c3 = ds.Tables[0].Rows[i][2] != null ? ds.Tables[0].Rows[i][2].ToString() : "";
                                c4 = ds.Tables[0].Rows[i][3] != null ? ds.Tables[0].Rows[i][3].ToString() : "";
                                var _date2 = ds.Tables[0].Rows[i][4] != null ? ds.Tables[0].Rows[i][4] : null;
                                if (_date2 != null)
                                {
                                    try
                                    {
                                        c5 = DateTime.ParseExact(_date2.ToString(), "dd/MM/yyyy hh:mm tt", null).ToString("yyyy-MM-dd hh:mm");
                                   
                                    }
                                    catch 
                                    {
                                        c5 = "";
                                    }
                                }

                                var _date3 = ds.Tables[0].Rows[i][5] != null ? ds.Tables[0].Rows[i][5] : null;
                                if (_date3 != null)
                                {
                                    try
                                    {
                                        c6 = DateTime.ParseExact(_date3.ToString(), "dd/MM/yyyy hh:mm tt", null).ToString("yyyy-MM-dd hh:mm");  
                                    }
                                    catch
                                    {
                                        c6 = "";
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
                               
                                var bcheckempty = c1 == "" && c2 == "" && c3 == "" && c4 == "";
                                if (!bcheckempty)
                                {
                                    try
                                    {
                                        string sql2 = @"INSERT INTO invoices(date, customer_name, sex, phone, time_to_pick, time_to_pay, pickup, paypoints, so_cho, form, driver, price, vat, sum, note) SELECT '" + c1 + "',N'" + c2 + "',N'" + c3 + "','" + c4 + "','" + c5 + "','" + c6 + "',N'" + c7 + "',N'" + c8 + "','" + c9 + "',N'" + c10 + "',N'" + c11 + "','" + c12 + "','" + c13 + "','" + c14 + "',N'" + c15 + "' WHERE NOT EXISTS (SELECT date, customer_name, sex, phone FROM invoices WHERE date = '" + c1 + "' AND customer_name = N'" + c2 + "' AND sex = N'" + c3 + "' AND phone = '" + c4 + "')";

                                        int noOfRowInserted = db.Database.ExecuteSqlCommand(sql2);
                                    }
                                    catch
                                    {
                                        continue;
                                    }

                                }
                                else
                                {
                                    continue;
                                }

                                if (i == countc - 1)
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


        public void ExportToExcel()
        {
            try
            {
                var model = (from s in db.invoices select s).Select(x => new
                {
                    Ngaythang = x.date ?? null,
                    Khachhang = x.customer_name,
                    Gioitinh = x.sex,
                    Dienthoai = x.phone,
                    Thoigiandon = x.time_to_pick,
                    Thoigiantra = x.time_to_pay,
                    DiemDon = x.pickup,
                    DiemTra = x.paypoints,
                    SoCho = x.so_cho,
                    HinhThuc = x.form,
                    TaiXe = x.driver,
                    Gia = x.price,
                    VAT = x.vat,
                    TongTien = x.sum,
                    GhiChu = x.note
                });
                if (model != null)
                {
                    DataTable dt = new DataTable();

                    dt.Columns.Add("Ngày tháng", typeof(DateTime));
                    dt.Columns.Add("Khách hàng", typeof(string));
                    dt.Columns.Add("Giới tính", typeof(string));
                    dt.Columns.Add("Số điện thoại", typeof(string));
                    dt.Columns.Add("Thời gian đón", typeof(DateTime));
                    dt.Columns.Add("Thời gian trả", typeof(DateTime));
                    dt.Columns.Add("Điểm đón", typeof(string));
                    dt.Columns.Add("Điểm trả", typeof(string));
                    dt.Columns.Add("Số chỗ", typeof(string));
                    dt.Columns.Add("Hình thức", typeof(string));
                    dt.Columns.Add("Tài xế", typeof(string));
                    dt.Columns.Add("Giá", typeof(string));
                    dt.Columns.Add("VAT", typeof(string));
                    dt.Columns.Add("Tổng tiền", typeof(string));
                    dt.Columns.Add("Ghi chú", typeof(string));
                    foreach (var item in model)
                    {
                        dt.Rows.Add(item.Ngaythang, item.Khachhang, item.Gioitinh, item.Dienthoai, item.Thoigiandon, item.Thoigiantra, item.DiemDon, item.DiemTra, item.SoCho, item.HinhThuc, item.TaiXe, item.Gia, item.VAT, item.TongTien, item.GhiChu);
                    }
                    string fileName1 = string.Format("Danh_Sach_bang_ke_{0:yyyyMMdd}.xlsx", DateTime.Now);
                    WriteExcelWithNPOI(dt, "xlsx", fileName1);
                }                
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }
        }


        public void WriteExcelWithNPOI(DataTable dt, String extension, String fileName)
        {

            IWorkbook workbook;

            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt.Columns.Count; j++)
            {

                ICell cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }

            using (var exportData = new MemoryStream())
            {
                Response.Clear();
                workbook.Write(exportData);
                if (extension == "xlsx") //xlsx file format
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                    Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")  //xls file format
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                    Response.BinaryWrite(exportData.GetBuffer());
                }
                Response.End();
            }
        }

    }
}