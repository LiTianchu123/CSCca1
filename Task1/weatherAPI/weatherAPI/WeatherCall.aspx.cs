using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace weatherAPI
{
    public partial class WeatherCall : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          

        }

        [WebMethod]
        [ScriptMethod]
        public static string GetWeather(string country,string numOfDays) {

            string weatherJson = "";

            //http://api.worldweatheronline.com/premium/v1/weather.ashx?key=****&q=London&format=xml&num_of_days=5
            //id=jipx(spacetime0)
            UriBuilder url = new UriBuilder();
            url.Scheme = "http";// Same as "http://"

            url.Host = "api.worldweatheronline.com";
            url.Path = "premium/v1/weather.ashx";// change to v2
            url.Query = "q="+country+"&format=json&num_of_days="+numOfDays+"&key=0c7b9fe2e0474c95829141431202612&includeLocation=yes";

            //Make a HTTP request to the global weather web service
            weatherJson = MakeRequest(url.ToString());
            JObject jsonResponse = JObject.Parse(weatherJson);
            JToken data = jsonResponse["data"];
            if (data != null)
            {
                return data.ToString();
            }
            else
            {
                return null;
            }
        }

        public static string MakeRequest(string requestUrl) {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.ContentType = "json";
                request.Timeout = 15 * 1000;
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(response.GetResponseStream());
                string jsonString = "";
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    jsonString = reader.ReadToEnd();
                    Console.WriteLine(reader.ReadToEnd());
                }

                return (jsonString);

            }
            catch (Exception e){
                return null;
            }
        }
    }
}