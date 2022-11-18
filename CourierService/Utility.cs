using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace CourierService
{
    public class Utility
    {
        internal const string APIBaseAddress = "https://api.sendle.com";
        internal const string UserName = "rizwan_vu360solution";
        internal const string Password = "Mn9hSRM2Pk8St6S5g9cvFxGJ";

        public static HttpClient GetHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(APIBaseAddress) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{UserName}:{Password}")));
            client.DefaultRequestHeaders.Authorization = authValue;
            return client;
        }
        public static DateTime getDefaultTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
        }


    }
}