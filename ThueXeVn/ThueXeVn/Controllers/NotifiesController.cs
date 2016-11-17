using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;

namespace ThueXeVn.Controllers
{
    public class NotifiesController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        public const String certificatePass = "txvn";
        public const String certificateHostName = "gateway.sandbox.push.apple.com";
        public const string fcmAppId = "AIzaSyAIGls7p_pw8titXZyIvECI3Vyj1NsL5TQ";
        public const string fcmSenderId = "193430184784";

        // GET: Notifies
        public ActionResult Index()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            ViewBag.ListObject = ListObject();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SendNotifies(Notification model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại các trường.");
                return View();
            }

            List<notify> nguoinhan = new List<notify>();
            string NameObject = "";
            int typeObject = model.tobject;
            string strTitle = model.title ?? "";
            string strBody = model.body ?? "";
            
            if (typeObject == 1)
            {
                nguoinhan = db.notifies.Where(x => x.tobject == 1).ToList();
                NameObject = "Tài xế.";
            }
            else if (typeObject == 2)
            {
                nguoinhan = db.notifies.Where(x => x.tobject == 2).ToList();
                NameObject = "Khách thuê xe.";
            }
            else
            {
                nguoinhan = db.notifies.Where(x => x.tobject != null).ToList();
                NameObject = "Tài xế và khách thuê xe.";
            }

            if (nguoinhan.Count == 0)
            {
                TempData["Error"] = "Không có người nhận nào";
                return RedirectToAction("Index");
            }

            try 
	        {
                // xử lý gửi thông báo
                foreach (var item in nguoinhan)
                {
                    // Thêm thông báo vào bảng notices
                    notice _log = new notice();
                    _log.tobject = typeObject;
                    _log.regid = item.reg_id;
                    _log.os = item.os;
                    _log.title = strTitle;
                    _log.body = strBody;
                    db.notices.Add(_log);
                    if (item.os == 1)
                    {
                        if (PushMessageForAndroid(item.reg_id, strTitle, strBody) == 1)
                        {
                            TempData["Updated"] = "Đã gửi thông báo tới " + NameObject;
                            db.SaveChanges(); // Gửi thành công thì lưu lại vào bảng notices
                        }
                    }
                    else if(item.os == 2) 
                    {
                        if (PushMessageForIOS(item.reg_id, strTitle, strBody) == 1)
                        {
                            TempData["Updated"] = "Đã gửi thông báo tới " + NameObject;
                            db.SaveChanges(); // Gửi thành công thì lưu lại vào bảng notices
                        }
                    }                   

                }
                
                return RedirectToAction("Index");
	        }
	        catch (Exception ex)
	        {
		        TempData["Error"] = "Có lỗi xảy ra khi gửi thông báo";
                return RedirectToAction("Index");
	        }

        }

        public List<SelectListItem> ListObject()
        {
            List<SelectListItem> newList = new List<SelectListItem>();
            newList.Add(new SelectListItem() { Value = "1", Text = "Gửi cho tài xế" });
            newList.Add(new SelectListItem() { Value = "2", Text = "Gửi cho khách thuê xe" });
            newList.Add(new SelectListItem() { Value = "3", Text = "Gửi cho cả tài xế và khách thuê xe" });
            return newList;
        }

       

        public int PushMessageForIOS(string deviceId, string title, string body)
        {
            String certificateFile = System.Web.Hosting.HostingEnvironment.MapPath("/APNsNew.p12");
            int sended = 0;
            int port = 2195;
            
            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificateFile), certificatePass);

            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);


            TcpClient client = new TcpClient(certificateHostName, port);
            SslStream sslStream = new SslStream(client.GetStream(), false);

            try
            {
                sslStream.AuthenticateAsClient(certificateHostName, certificatesCollection, SslProtocols.Tls, false);
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memoryStream);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)32);

                writer.Write(HexStringToByteArray(deviceId.ToUpper()));
                //String payload = "{\"aps\":{\"alert\":\"" + messager + "\",\"badge\":1,\"sound\":\"default\"}}";
                string payload = "{\"aps\" : {\"alert\" : {\"title\" :\"" + title + "\",\"body\" :\"" + body + "\", \"action-loc-key\" : \"PLAY\"}, \"badge\" : 1, \"sound\":\"default\"}}";
                writer.Write((byte)0);
                writer.Write((byte)payload.Length);
                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                writer.Write(b1);
                writer.Flush();
                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();
                client.Close();
                sended = 1;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                client.Close();
            }
            catch (Exception e)
            {
                client.Close();
            }
            return sended;
        }

        public int PushMessageForAndroid(string regId, string title, string body)
        {
            int sended = 0;
            try
            {
                if (regId != null && regId != "")
                {
                    //thiết lập GCM send
                    WebRequest tRequest;
                    tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "POST";
                    tRequest.UseDefaultCredentials = true;
                    tRequest.PreAuthenticate = true;
                    tRequest.Credentials = CredentialCache.DefaultNetworkCredentials;
                    //định dạng JSON
                    tRequest.ContentType = "application/json";
                    //tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", fcmAppId));

                    tRequest.Headers.Add(string.Format("Sender: id={0}", fcmSenderId));

                    //string postData = "{ \"registration_ids\": [ \"" + RegArr + "\" ],\"data\": {\"message\": \"" + title + ";" + hinhanh + "\",\"id\":\"" + strhethongst + "\"}}"; //"\",\"dsanh\":\"" + dsanh +
                    string postData = "{ \"registration_ids\": [ \"" + regId + "\" ],\"data\": {\"message\": \"" + title + "\",\"body\": \"" + body + "\",\"id\": \"" + "\",\"collapse_key\":\"" + "" + "\"}}";

                    //string postData = Convert.ToBase64String(fileBytes);

                    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    tRequest.ContentLength = byteArray.Length;

                    Stream dataStream = tRequest.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    WebResponse tResponse = tRequest.GetResponse();

                    dataStream = tResponse.GetResponseStream();

                    StreamReader tReader = new StreamReader(dataStream);


                    string sResponseFromServer = tReader.ReadToEnd();

                    tReader.Close();
                    dataStream.Close();
                    tResponse.Close();

                    var json = JObject.Parse(sResponseFromServer);  //Turns your raw string into a key value lookup
                    var xyz = json["success"].ToString();

                    if (xyz != "0")
                    {
                        sended = 1;
                    }
                }
                return sended;
            }
            catch (Exception ex)
            {
                return sended;
            }
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            //check for null
            if (hexString == null) return null;
            //get length
            int len = hexString.Length;
            if (len % 2 == 1) return null;
            int len_half = len / 2;
            //create a byte array
            byte[] bs = new byte[len_half];
            try
            {
                //convert the hexstring to bytes
                for (int i = 0; i != len_half; i++)
                {
                    bs[i] = (byte)Int32.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception : " + ex.Message);
            }
            //return the byte array
            return bs;
        }

    }
}