using CourierService.ViewModels.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace CourierService
{
    public class SendleAPI
    {
        public async Task<String> Ping()
        {
           return await Task.Run(() =>
            {
                using (var client = Utility.GetHttpClient())
                {
                    var responseT = client.GetAsync("api/ping");
                    responseT.Wait();
                    var dr = responseT.Result.Content.ReadAsStringAsync();
                    return dr.Result;
                }
            });
            
        }

        public async Task<String> CreateOrder(OrderViewModel model)
        {
            return await Task.Run(() =>
            {
                using (var client = Utility.GetHttpClient())
                {

                    var myContent = JsonConvert.SerializeObject(model);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");
                    var responseTask = client.PostAsync("api/orders/", byteContent);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // if (result.IsSuccessStatusCode)
                    // {
                    return result.Content.ReadAsStringAsync();
                    //  }

                }
            });
        
        }

        public async Task<OrderTrackingViewModel> TrackOrder(string refrenceId)
        {
            return await Task.Run(() =>
            {
                using (var client = Utility.GetHttpClient())
                {
                    var response = client.GetAsync("api/tracking/" + refrenceId);
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<OrderTrackingViewModel>();
                        readTask.Wait();
                        return readTask.Result;
                       
                    }

                    return null;
                }
            });

        }
    }
}