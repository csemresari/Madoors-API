using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Madoors.Controllers
{
    public class BaseAPIController : ApiController
    {

        protected PersistencyManager m_Persistency_Manager = new PersistencyManager();

        protected bool PushNotification(string branchName, string entranceName, string pushMessage, Guid id)
        {
            bool isPushMessageSend = false;

            try
            {
                String parseApplicationKey = "XXXXXXX";
                String parseRestApiKey = "XXXXXXX";

                string postString = "";
                string urlpath = "https://api.parse.com/1/push";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(urlpath);

                string channel = branchName;
                postString = "{\"channels\": [\"" + channel + "\"" + "] ," +
                                  "\"data\" : { \"alert\":\"" + pushMessage + "\" ," +
                                               "\"sound\" : \"" + "alert.wav" + "\" ," +
                                               "\"id\" : \"" + id.ToString() + "\" " +
                                    "}" + "}";

                var tampon = Encoding.UTF8.GetBytes(postString);

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.ContentLength = tampon.Length;
                httpWebRequest.Headers.Add("X-Parse-Application-Id", parseApplicationKey);
                httpWebRequest.Headers.Add("X-Parse-REST-API-KEY", parseRestApiKey);
                httpWebRequest.Method = "POST";

                using (var requestWriter = httpWebRequest.GetRequestStream())
                {
                    requestWriter.Write(tampon, 0, tampon.Length);

                    requestWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JObject jObjRes = JObject.Parse(responseText);
                    if (Convert.ToString(jObjRes).IndexOf("true") != -1)
                    {
                        isPushMessageSend = true;
                    }
                }

                m_Logger.Debug("isPushMessageSend: " + isPushMessageSend);
            }
            catch (Exception exc)
            {
                m_Logger.Debug("Domain: " + domain);
                m_Logger.Debug("Identifier: " + identifier);
                m_Logger.Debug("Push Message: " + pushMessage);
                m_Logger.Debug("AppConfig: " + appConfig.ToString());
                m_Logger.Debug("ID: " + id);
                m_Logger.Debug("PushNotification:\n\n" + exc.StackTrace);
            }

            return isPushMessageSend;
        }

        // GET api/baseapi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/baseapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/baseapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/baseapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/baseapi/5
        public void Delete(int id)
        {
        }
    }
}
