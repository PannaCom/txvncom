using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using ThueXeVn.Models;
namespace ThueXeVn
{
    public class Config
    {
        public static int PageSize = 25;
        public static string NewsImagePath = "/Images/News";
        public static string domain = "http://thuexevn.com/";
        public static thuexevnEntities db = new thuexevnEntities();
        public static bool mail(string from, string to, string topic, string pass, string content)
        {
            try
            {
                var fromAddress = from;
                var toAddress = to;
                //Password of your gmail address
                string fromPassword = pass;
                // Passing the values and make a email formate to display
                string subject = topic;
                string body = content;

                // smtp settings
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromAddress);
                message.To.Add(toAddress);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";//"smtp.gmail.com";
                    smtp.Port = 587;// 465;//587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Timeout = 20000;
                }
                // Passing values to smtp object
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static long randomcode()
        {
            Random random = new Random();
            return random.Next(1000000000, int.MaxValue) + 1;
        }
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            if (input == "" || input == null) input = "chanhniem";
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }
        public static string getTinhThanh()
        {
            var p = (from q in db.TinhThanhs orderby q.tinhthanhpho select q.tinhthanhpho).Distinct().ToList();
            string tinhthanh = "<select id=province name=province class=\"form-control\">";
            tinhthanh += "<option value=\"\">Chọn tỉnh thành</option>";
            for (int i = 0; i < p.Count; i++)
            {
                tinhthanh += "<option value=\"" + p[i] + "\">" + p[i] + "</option>";
            }
            tinhthanh += "</select>";
            return tinhthanh;
        }
        public static string getProjectMenu(int id)
        {
            string menuleft = "";
            try
            {
                //Lấy ra menu bên trái
                var mn = (from p in db.news where p.id!=id
                          select new
                          {
                              image = p.image,
                              title=p.title,
                              des=p.des,
                              id=p.id,
                              datetime=p.datetime,
                          }).OrderByDescending(o => o.id).Take(10).ToList();


                string link = "";
               
                for (int i = 0; i < mn.Count; i++)
                {
                   
                    link = "/tin/" + Config.unicodeToNoMark(mn[i].title) + "-" + mn[i].id;
                    string style = "style=\"display:block;\"";
                    menuleft += "<div " + style + ">&nbsp;&nbsp;-<a href=\"" + link + "\">" + mn[i].title.ToUpperInvariant() + "</a></div>";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            return menuleft;
        }
        //convert longitude latitude to geography
        public static DbGeography CreatePoint(double? latitude, double? longitude)
        {
            if (latitude == null || longitude == null) return null;
            latitude = (double)latitude;
            longitude = (double)longitude;
            return DbGeography.FromText(String.Format("POINT({1} {0})", latitude, longitude));
        }
        public static void setCookie(string field, string value)
        {
            HttpCookie MyCookie = new HttpCookie(field);
            MyCookie.Value = value;
            MyCookie.Expires = DateTime.Now.AddDays(365);
            HttpContext.Current.Response.Cookies.Add(MyCookie);
            //Response.Cookies.Add(MyCookie);   
            
        }
        public static string getCookie(string v)
        {
            try
            {
                return HttpContext.Current.Request.Cookies[v].Value.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static void RemoveCookie(string v)
        {
            if (HttpContext.Current.Request.Cookies[v] != null)
            {
                var c = new HttpCookie(v);
                c.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(c);
            }
        }

        //convert tieng viet thanh khong dau va them dau -
        public static string unicodeToNoMark(string input)
        {
            input = input.ToLowerInvariant().Trim();
            if (input == null) return "";
            string noMark = "a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,e,e,e,e,e,e,e,e,e,e,e,e,u,u,u,u,u,u,u,u,u,u,u,u,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,i,i,i,i,i,i,y,y,y,y,y,y,d,A,A,E,U,O,O,D";
            string unicode = "a,á,à,ả,ã,ạ,â,ấ,ầ,ẩ,ẫ,ậ,ă,ắ,ằ,ẳ,ẵ,ặ,e,é,è,ẻ,ẽ,ẹ,ê,ế,ề,ể,ễ,ệ,u,ú,ù,ủ,ũ,ụ,ư,ứ,ừ,ử,ữ,ự,o,ó,ò,ỏ,õ,ọ,ơ,ớ,ờ,ở,ỡ,ợ,ô,ố,ồ,ổ,ỗ,ộ,i,í,ì,ỉ,ĩ,ị,y,ý,ỳ,ỷ,ỹ,ỵ,đ,Â,Ă,Ê,Ư,Ơ,Ô,Đ";
            string[] a_n = noMark.Split(',');
            string[] a_u = unicode.Split(',');
            for (int i = 0; i < a_n.Length; i++)
            {
                input = input.Replace(a_u[i], a_n[i]);
            }
            input = input.Replace("  ", " ");
            input = Regex.Replace(input, "[^a-zA-Z0-9% ._]", string.Empty);
            input = removeSpecialChar(input);
            input = input.Replace(" ", "-");
            input = input.Replace("--", "-");
            return input;
        }

        public static string encodePhone(string input)
        {
            if (input == null)
            {
                input = "00000";
            }
            if (input.Length < 9)
            {
                input = "0000" + input;
            }
            string noMark = "a,b,c,d,e,f,g,h,i,j";
            string number = "0,1,2,3,4,5,6,7,8,9";
            string[] a_n = noMark.Split(',');
            string[] a_n2 = number.Split(',');
            for (int i = 0; i < a_n.Length; i++)
            {
                input = input.Replace(a_n2[i], a_n[i]);
            }
            input = input.Substring(0, 5);
            input = input.ToUpper();
            return input;
        }

        public static string removeSpecialChar(string input)
        {
            input = input.Replace("-", "").Replace(":", "").Replace(",", "").Replace("_", "").Replace("'", "").Replace("\"", "").Replace(";", "").Replace("”", "").Replace(".", "").Replace("%", "");
            return input;
        }
        public static void SaveTolog(string log)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath("~/" + "log.txt"), true))
            {
                sw.WriteLine(DateTime.Now.ToString() + ": " + log);
                sw.Close();
            }
        }

        public static void ToExcel(HttpResponseBase Response, object clientsList, string fileName)
        {
            try
            {
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = clientsList;
                grid.DataBind();
                Response.ClearContent();
                //var filename = "MatHang_" + DateTime.Now.ToString("yyyyMMdd")+".xls";
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                //Response.ContentType = "application/vnd.ms-excel";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);
                string headerTable = @"<Table><tr><td>Ngày tháng</td><td>Tên khách hàng</td><td>Giới tình</td><td>Số điện thoại</td><td>Thời gian đón</td><td>Thời gian trả</td><td>Điểm đón</td><td>Điểm trả</td><td>Loại xe</td><td>Hình thức</td><td>Tài xế</td><td>Giá</td><td>VAT</td><td>Tổng tiền</td><td>Ghi chú</td></tr></Table>";
                Response.Write(headerTable);
                Response.Write(sw.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                Config.SaveTolog(ex.ToString());
            }
        }

    }
}