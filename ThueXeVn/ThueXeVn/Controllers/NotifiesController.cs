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
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;

namespace ThueXeVn.Controllers
{
    public class NotifiesController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        public const String certificateFile = System.Web.Hosting.HostingEnvironment.MapPath("/APNsNew.p12");
        public const String certificatePass = "txvn";
        public const String certificateHostName = "gateway.sandbox.push.apple.com";
        public const string fcmAppId = "AIzaSyAIGls7p_pw8titXZyIvECI3Vyj1NsL5TQ";
        public const string fcmSenderId = "193430184784";

        // GET: Notifies
        public ActionResult Index()
        {
            return View();
        }

        public int PushMessageForIOS(string deviceId, string title, string body)
        {
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
                return false;
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