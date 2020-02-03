using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using prikol.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace prikol.Controllers
{
    public class OpenweatherMapController : Controller
    {

        // GET: OpenweatherMap
        public ActionResult Index()
        {
            return View();
        }

        public string getWeatherForCurrCity()
        {
            string str = GetCity();
            return str;

        }

        [HttpGet]
        public ActionResult GetWeather()
        {
            return View();
        }
        [HandleError(ExceptionType = typeof(System.Net.WebException))]
        public static string GetLocalIPAddress()
        {
            string pubIp = new System.Net.WebClient().DownloadString("https://api.ipify.org");
            return pubIp;
        }

        public static string GetCity()
        {

            string currIP = GetLocalIPAddress();
            HttpWebRequest apiRequest =
                        WebRequest.Create("http://ip-api.com/json/" +
                        currIP) as HttpWebRequest;
            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            JObject o = JObject.Parse(apiResponse);
            string temp = o["city"].ToString();
            return temp;
        }

        //Поправить сохранение в файл
        [HttpPost]
        public FileResult saveToFile()
        {
            string path = @"C:\Univer\for1day.txt";
            string file_type = "application/txt";

            ResponseWeather rootObject = TempStore.response;

            //string json = JsonSerializer.Serialize(rootObject);

            string json = JsonConvert.SerializeObject(rootObject);

            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine(json);
            }

            return File(path, file_type);

        }

        public FileResult saveToFile4days(int IdElement)
        {
            string path = @"C:\Univer\for4day.txt";
            string file_type = "application/txt";
            tempModel tempModel = TempStore.temps.Find(item=>item.Id == IdElement);
            string json = JsonConvert.SerializeObject(tempModel);
            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine(json);
            }

            return File(path, file_type);

        }




        [HandleError(ExceptionType = typeof(System.Net.WebException))]
        [HttpPost]
        public ActionResult GetWeather(string city)
        {
            //https://samples.openweathermap.org/data/2.5/weather?q=Minsk&appid=3ddbdbaa8509490ac43f89297c044421   погода
            string apiKey = "3ddbdbaa8509490ac43f89297c044421";
            HttpWebRequest apiRequest =
                WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?q=" +
                city + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;
            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }
            ResponseWeather rootObject = JsonConvert.DeserializeObject<ResponseWeather>(apiResponse);
            ViewBag.responceWeather = rootObject;
            TempStore.response = rootObject;

            TimeSpan sunrise = TimeSpan.FromMilliseconds(rootObject.sys.sunrise);
            TimeSpan sunset = TimeSpan.FromMilliseconds(rootObject.sys.sunset);

            ViewBag.sunrise = sunrise;
            ViewBag.sunset= sunset;


            return PartialView("GetWeatherPartial");
        }


        //для 5 дневной погоды, бертся через каждые 3 часа
        //Добавить краткую сводку за день(делал раньше)
        [HttpPost]
        public ActionResult getWeather5days(string city5days)
        {
            string cc = city5days.Split(new char[] { ',' })[0];
            string apiKey = "3ddbdbaa8509490ac43f89297c044421";

            HttpWebRequest apiRequest =
                WebRequest.Create("http://api.openweathermap.org/data/2.5/forecast?q=" +
                cc + "&appid=" + apiKey) as HttpWebRequest;
            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }
            int cnt = Convert.ToInt32(JObject.Parse(apiResponse)["cnt"]);

            var o = JObject.Parse(apiResponse)["list"];

            List<tempModel> allInfo = new List<tempModel>();

            

            for (int i = 0; i < cnt; i++)
            {
                string w = Convert.ToString(o[i]);
                tempModel rootObject = JsonConvert.DeserializeObject<tempModel>(w);
                allInfo.Add(rootObject);

            }
            ViewBag.AllInfos = allInfo;

            int? time = null;
            List<tempModel> days = new List<tempModel>();
            tempModel first_day = new tempModel();
            first_day.weather.Add(new Weather());
            int counterDays = 0;
            time = allInfo[0].dt_txt.Day;

            for (int i = 0; i < allInfo.Count; i++)
            {

                if (allInfo[i].dt_txt.Day == time)
                {
                    counterDays++;
                    first_day.clouds.all += allInfo[i].clouds.all;
                    first_day.main.humidity += allInfo[i].main.humidity;
                    first_day.main.pressure += allInfo[i].main.pressure;
                    first_day.main.temp += allInfo[i].main.temp;
                    first_day.weather[0].description = allInfo[i].weather[0].description;
                    first_day.dt_txt = allInfo[i].dt_txt;
                    first_day.wind.deg += allInfo[i].wind.deg;
                    first_day.wind.speed += allInfo[i].wind.speed;
                }
                else
                {
                    first_day.Id = i;
                    first_day.clouds.all /= counterDays;
                    first_day.main.humidity /= counterDays;
                    first_day.main.pressure /= counterDays;
                    first_day.main.temp /= counterDays;
                    first_day.main.temp -= 273;
                    first_day.wind.deg /= counterDays;
                    first_day.wind.speed /= counterDays;

                    days.Add(first_day);
                    time = allInfo[i].dt_txt.Day;
                    counterDays = 0;
                    first_day = new tempModel();
                    first_day.weather.Add(new Weather());

                }
            }
            TempStore.temps = days;
            ViewBag.averageWeather = days;


            return PartialView("getWeather5days");
            
        }

    }
}