using CourierService.Services.StreamVitals;
using CourierService.ViewModels.StreamVitalsIntegration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace CourierService.Controllers
{
    public class StreamVitalsController : Controller
    {
        StreamVitalService SV_service = new StreamVitalService();

        // GET: StreamVitals
        //public void DeviceAssign()
        //{

        //    var data = SV_service.AssignDevice().ToList();


        //    foreach (var order in data.ToList())
        //    {
        //        List<int> NewOrderId = new List<int>();
        //        List<int> VerifiedOrderId = new List<int>();

        //        if (order.DeviceTypeId == 1 && order.Customer_Reference == null)
        //        {
        //            data.Remove(order);
        //        }
        //        if (order.DeviceTypeId == 8)
        //        {
        //            var res = SV_service.GetPairList(order.ReceiverId.Value, order.CreatedOn.Value);

        //            foreach (var Rep in res)
        //            {
        //                if (Rep.SenderId != null && Rep.Customer_Reference != null && Rep.DeliveredDate != null)
        //                {
        //                    NewOrderId.Add(Rep.Id);
        //                }
        //            }
        //            foreach (var Rep1 in res)
        //            {
        //                int con = Rep1.Id - 1;
        //                if (Rep1.SenderId == null && Rep1.DeliveredDate != null)
        //                {
        //                    var test = NewOrderId.FirstOrDefault(y => y == con);
        //                    if (test > 0)
        //                    {
        //                        VerifiedOrderId.Add(test);
        //                    }
        //                }
        //            }
        //            var neworder = VerifiedOrderId.FirstOrDefault(z => z == order.Id);
        //            if (neworder <= 0)
        //            {
        //                data.Remove(order);
        //            }
        //        }
        //    }




        //    if (data.Count() != 0)
        //    {
        //        foreach (var item in data)
        //        {
        //            var model = new List<AssignDeviceViewModel>
        //            {
        //                new AssignDeviceViewModel
        //                {
        //                    PatientId = item.Receiver.SVPatientId,
        //                    EHR = item.Receiver.EHR,
        //                    IMEI = item.Customer_Reference,
        //                    RefrenceId = item.Id,
        //                    RequestType = item.DeviceTypeId == 1 ? "New Device" : (item.DeviceTypeId == 6 ? "Return" : (item.DeviceTypeId == 8 ? "Replacement" : "")),
        //                    Notes = "",
        //                    DeliveredOn = item.DeliveredDate
        //                }
        //            };
        //            try
        //            {
        //                using (HttpClient client = new HttpClient())
        //                {
        //                    // For Serialize
        //                    var myContent = JsonConvert.SerializeObject(model);
        //                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //                    var byteContent = new ByteArrayContent(buffer);
        //                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");

        //                    client.DefaultRequestHeaders.Add("CourierKey", "c8c6742e-5487-45df-9acf-1e723b1386d1");

        //                    var responseTask = client.PostAsync("http://svrpdapi.vu360solutions.org/courierservice/PaitentDeviceAssignRequest", byteContent);
        //                    responseTask.Wait();
        //                    var result = responseTask.Result;
        //                    if (result.IsSuccessStatusCode)
        //                    {
        //                        SV_service.UpdateIsCompleted(model[0].RefrenceId);
        //                        SV_service.UpdateIntegrationStatus(model[0].RefrenceId);

        //                        if (item.DeviceTypeId==8)
        //                        {
        //                            var ret =SV_service.GetReplacementReturn(model[0].RefrenceId);

        //                            SV_service.UpdateIsCompleted(ret.Id);
        //                            SV_service.UpdateIntegrationStatus(ret.Id);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Add("CourierKey", "c8c6742e-5487-45df-9acf-1e723b1386d1");
        //            var responseTask = client.GetAsync("http://svrpdapi.vu360solutions.org/courierservice/SendDailyEmail");
        //            responseTask.Wait();
        //            var result = responseTask.Result;
        //            if (result.IsSuccessStatusCode)
        //            {

        //            }
        //        }
        //    }
        //}

        public void DeviceAssignWithSingleObject()
        {
            var data = SV_service.AssignDevice().ToList();


            foreach (var order in data.ToList())
            {
                List<int> NewOrderId = new List<int>();
                List<int> VerifiedOrderId = new List<int>();


                if (order.DeviceTypeId == 8)
                {
                    var res = SV_service.GetPairList(order.ReceiverId.Value, order.CreatedOn.Value);

                    foreach (var Rep in res)
                    {
                        if (Rep.SenderId != null && Rep.Customer_Reference != null && Rep.DeliveredDate != null)
                        {
                            NewOrderId.Add(Rep.Id);
                        }
                    }
                    foreach (var Rep1 in res)
                    {
                        int con = Rep1.Id - 1;
                        if (Rep1.SenderId == null && Rep1.DeliveredDate != null)
                        {
                            var test = NewOrderId.FirstOrDefault(y => y == con);
                            if (test > 0)
                            {
                                VerifiedOrderId.Add(test);
                            }
                        }
                    }
                    var neworder = VerifiedOrderId.FirstOrDefault(z => z == order.Id);
                    if (neworder <= 0)
                    {
                        data.Remove(order);
                    }
                }
            }




            if (data.Count() != 0)
            {
                foreach (var item in data)
                {
                    var model = new AssignDeviceViewModel
                    {
                        PatientId = item.Receiver.SVPatientId,
                        EHR = item.Receiver.EHR,
                        IMEI = item.Customer_Reference,
                        RefrenceId = item.Id,
                        RequestType = item.DeviceTypeId == 1 ? "New Device" : (item.DeviceTypeId == 6 ? "Return" : (item.DeviceTypeId == 8 ? "Replacement" : "")),
                        Notes = "",
                        DeliveredOn = item.DeliveredDate
                    };
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            // For Serialize
                            var myContent = JsonConvert.SerializeObject(model);
                            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                            var byteContent = new ByteArrayContent(buffer);
                            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");

                            client.DefaultRequestHeaders.Add("CourierKey", "c8c6742e-5487-45df-9acf-1e723b1386d1");

                            var responseTask = client.PostAsync("http://svrpdapi.vu360solutions.org/courierservice/PaitentDeviceAssign", byteContent);
                            responseTask.Wait();
                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                var readTask = result.Content.ReadAsAsync<ResponceViewModel>();
                                readTask.Wait();
                                var res = readTask.Result;

                                // Update IsCompleted and IntegrationRemarks
                                SV_service.UpdateInegrationFields(res);
                                // Update IntegrationStatus
                                SV_service.UpdateIntegrationStatus(res.RefrenceId);

                                if (item.DeviceTypeId == 8)
                                {
                                    var ret = SV_service.GetReplacementReturn(res.RefrenceId);
                                    SV_service.UpdateIntegrationStatus(ret.Id);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Server error try after some time.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("CourierKey", "c8c6742e-5487-45df-9acf-1e723b1386d1");
                    var responseTask = client.GetAsync("http://svrpdapi.vu360solutions.org/courierservice/SendDailyEmail");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                    }
                }
            }
        }
    }
}