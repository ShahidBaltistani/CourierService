using CourierService.Services;
using CourierService.Services.NewPracticeF;
using CourierService.Services.OrderLogs;
using CourierService.Services.Orders;
using CourierService.Services.TaskManagements;
using CourierService.ViewModels.Orders;
using CourierService.ViewModels.ReturnORReplacament;
using CourierService.ViewModels.TaskManagements;
using CourierService.ViewModels.TaskOrder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourierService.Controllers
{
    [Authorize]
    public class CreatingOrdersController : Controller
    {
        APIResponseService _APIResponseService = new APIResponseService();
        OrderService orderService = new OrderService();
        OrderLabelService orderLabelService = new OrderLabelService();
        OrderLogService _orderLogService = new OrderLogService();
        TaskService _service = new TaskService();
        NewPracticeService newPractice = new NewPracticeService();
        int ReceiverIdForBulkReplacement = 0;
        int orderIdForLog = 0;
        public async Task<ActionResult> ReplacementOrder(int Id, DateTime PickUpDate, string ReplacementRemarks)
        {
            var item = orderService.GetById(Id);
            string date = PickUpDate.ToString("yyyy-MM-dd");

            var newOrder = new
            {
                pickup_date = date,
                first_mile_option = "pickup",
                description = item.Description,
                weight = new
                {
                    value = "2",
                    units = "lb"
                },
                customer_reference = item.Customer_Reference,
                sender = new
                {
                    instructions = "Please Call Support Number - Replacement/New",
                    contact = new
                    {
                        name = item.Sender.Name,
                        phone = item.Sender.Phone,
                        company = item.Sender.Company
                    },
                    address = new
                    {
                        address_line1 = item.Sender.Address_Line1,
                        suburb = item.Sender.Suburb,
                        postcode = item.Sender.Postcode,
                        state_name = item.Sender.State_Name,
                        country = item.Sender.Country
                    }
                },
                receiver = new
                {
                    instructions = "Please Call Support Number - Replacement/New",
                    contact = new
                    {
                        name = item.Receiver.Name,
                        email = item.Receiver.Email,
                        company = item.Receiver.Company
                    },
                    address = new
                    {
                        address_line1 = item.Receiver.Address_Line1,
                        suburb = item.Receiver.Suburb,
                        postcode = item.Receiver.Postcode,
                        state_name = item.Receiver.State_Name,
                        country = item.Receiver.Country
                    }
                }
            };

            var returnOrder = new
            {

                pickup_date = date,
                first_mile_option = "pickup",
                description = item.Description,
                weight = new
                {
                    value = "2",
                    units = "lb"
                },
                //customer_reference = item.Customer_Reference,
                sender = new
                {
                    instructions = "Please Call Support Number - Replacement/Return",
                    contact = new
                    {
                        name = item.Receiver.Name,
                        phone = item.Receiver.Phone,
                        company = "-"
                    },
                    address = new
                    {
                        address_line1 = item.Receiver.Address_Line1,
                        suburb = item.Receiver.Suburb,
                        postcode = item.Receiver.Postcode,
                        state_name = item.Receiver.State_Name,
                        country = item.Receiver.Country
                    }
                },
                receiver = new
                {
                    instructions = "Please Call Support Number - Replacement/Return",
                    contact = new
                    {
                        name = "Rizwan Khan",
                        phone = item.Sender.Phone,
                        email = item.Sender.Email,
                        company = item.Sender.Company
                    },
                    address = new
                    {
                        address_line1 = item.Sender.Address_Line1,
                        suburb = item.Sender.Suburb,
                        postcode = item.Sender.Postcode,
                        state_name = item.Sender.State_Name,
                        country = item.Sender.Country
                    }
                }
            };

            using (var client = Utility.GetHttpClient())
            {
                var myContent = JsonConvert.SerializeObject(newOrder);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");
                var responseTask = client.PostAsync("https://api.sendle.com/api/orders", byteContent);//
                responseTask.Wait();
                var result = responseTask.Result;
                var responsetext = await result.Content.ReadAsStringAsync();



                if (result.IsSuccessStatusCode)
                {

                    dynamic data = JObject.Parse(responsetext);
                    JArray array = data.labels;
                    var newModel = new OrderViewModel()
                    {
                        ReceiverId = item.ReceiverId,
                        SourceId = item.SourceId,
                        DeviceTypeId = 8,
                        PracticeId = item.PracticeId,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        PatientId = item.ReceiverId.Value,
                        NewPracticeId = item.NewPracticeId
                    };

                    var replacement = orderService.AddSingleOrder(newModel);
                    var model = new APIResponseViewModel
                    {
                        OrderId = replacement.Id,
                        ResponseText = responsetext,
                        SendleOrderId = data.order_id,
                        SendleOrderUrl = data.order_url,
                        SendleRefrence = data.sendle_reference,
                        SendleTrackingUrl = data.tracking_url,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };
                    _APIResponseService.Add(model);
                    var model1 = new OrderLabelViewModel
                    {
                        OrderId = replacement.Id,
                        Formate = array[0]["format"].ToString(),
                        Size = array[0]["size"].ToString(),
                        Url = array[0]["url"].ToString()
                    };

                    orderLabelService.Add(model1);

                    orderService.UpdateOrderStatus(replacement.Id, 2);

                    // Add Remarks
                    _orderLogService.UpdateCancelRemarks(replacement.Id, ReplacementRemarks);


                    //Order Log

                    var orderLog = new OrderLogViewModel
                    {
                        DeviceTypeId = 1,
                        OrderId = replacement.Id,
                        ReceiverId = replacement.ReceiverId,
                        ReferenceOrderId = Id,
                        SendleRefrence = data.sendle_reference
                    };
                    _orderLogService.Add(orderLog);

                }
                else
                {

                    TempData["responseText"] = responsetext;
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);

                }

                var myContent1 = JsonConvert.SerializeObject(returnOrder);
                var buffer1 = System.Text.Encoding.UTF8.GetBytes(myContent1);
                var byteContent1 = new ByteArrayContent(buffer1);
                byteContent1.Headers.ContentType = new MediaTypeHeaderValue("application/Json");//
                var responseTask1 = client.PostAsync("https://api.sendle.com/api/orders", byteContent1);
                responseTask1.Wait();
                var result1 = responseTask1.Result;
                var responsetext1 = await result1.Content.ReadAsStringAsync();
                if (result1.IsSuccessStatusCode)
                {
                    dynamic data = JObject.Parse(responsetext1);
                    JArray array = data.labels;
                    var newModel = new OrderViewModel()
                    {
                        ReceiverId = item.ReceiverId,
                        SourceId = item.SourceId,
                        DeviceTypeId = 8,
                        PracticeId = item.PracticeId,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        SenderId = null,
                        PatientId = item.ReceiverId.Value,

                        NewPracticeId = item.NewPracticeId
                    };

                    var replacement = orderService.AddSingleReturnOrder(newModel);
                    var model = new APIResponseViewModel
                    {
                        OrderId = replacement.Id,
                        ResponseText = responsetext1,
                        SendleOrderId = data.order_id,
                        SendleOrderUrl = data.order_url,
                        SendleRefrence = data.sendle_reference,
                        SendleTrackingUrl = data.tracking_url,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };
                    _APIResponseService.Add(model);
                    var model1 = new OrderLabelViewModel
                    {
                        OrderId = replacement.Id,
                        Formate = array[0]["format"].ToString(),
                        Size = array[0]["size"].ToString(),
                        Url = array[0]["url"].ToString()
                    };

                    orderLabelService.Add(model1);

                    orderService.UpdateOrderStatus(replacement.Id, 2);


                    // Add Remarks
                    _orderLogService.UpdateCancelRemarks(replacement.Id, ReplacementRemarks);

                    var orderLog = new OrderLogViewModel
                    {
                        DeviceTypeId = 6,
                        OrderId = replacement.Id,
                        ReceiverId = replacement.ReceiverId,
                        ReferenceOrderId = Id,
                        SendleRefrence = data.sendle_reference
                    };
                    _orderLogService.Add(orderLog);
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["responseText"] = responsetext;
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);

                }
            }
        }

        public ActionResult SingleOrder(string Tab)
        {
            ViewBag.Tab = Tab;
            return View();
        }
        [HttpPost]
        public ActionResult CreateSingleOrder(OrderViewModel model, OrderwithTaskViewModel orderwithTask)
        {
            var newmodel = new ReturnReplacementViewModel
            {
                SourceId = model.SourceId,
                DeviceTypeId = model.DeviceTypeId,
                NewPracticeId = model.NewPracticeId,
                Instructions = model.Receiver.Instructions,
                Name = model.Receiver.Name,
                Email = model.Receiver.Email,
                Phone = model.Receiver.Phone,
                Address_Line1 = model.Receiver.Address_Line1,
                Address_Line2 = model.Receiver.Address_Line2,
                Suburb = model.Receiver.Suburb,
                Postcode = model.Receiver.Postcode,
                State_Name = model.Receiver.State_Name,
                Country = model.Receiver.Country,
            };

            if (Request["MakeReturnorReplacement"] != null)
            {
                return RedirectToAction("MakeReturnorReplacement", "CreatingOrders", newmodel);
            }


            var instructions = "";
            switch (model.DeviceTypeId)
            {
                case 1:
                    instructions = "Please Call Support Number";
                    break;
                case 2:
                    instructions = "Please Call Support Number -B";
                    break;
                case 3:
                    instructions = "Please Call Support Number -HP";
                    break;
                case 4:
                    instructions = "Please Call Support Number -Adp";
                    break;
                case 9:
                    instructions = "Please Call Support Number -Pouch";
                    break;
                case 10:
                    instructions = "Please Call Support Number Cuff(s)";
                    break;
                case 11:
                    instructions = "Please Call Support Number Cuff(b)";
                    break;
            }
            model.Receiver.Instructions = instructions;


            model.CreatedBy = Convert.ToInt32(Session["UserId"]);
            model.CreatedOn = DateTime.Now;
            model.Receiver.CreatedBy = Convert.ToInt32(Session["UserId"]);
            model.Receiver.CreatedOn = DateTime.Now;

            model.PracticeId = newPractice.GetPracticeNameById(model.NewPracticeId.Value).Name;


            var orders = orderService.AddSingleOrder(model);
            if (orders == null)
            {
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }




            // New Code
            if (orderwithTask.AddToTaskManagement == true)
            {
                TaskMailViewModel TaskMailmodel = new TaskMailViewModel();

                TaskMailmodel.From = orderwithTask.From;
                TaskMailmodel.Subject = orderwithTask.Subject;
                TaskMailmodel.ReceivedDate = orderwithTask.ReceivedDate;
                TaskMailmodel.Description = orderwithTask.Description;

                TaskMailmodel.CreatedOn = DateTime.Now;
                TaskMailmodel.IsDeleted = false;
                TaskMailmodel.IsActive = true;
                var TMReturn = _service.AddTaskMail(TaskMailmodel);

                TaskDetailViewModel TaskDetailmodel = new TaskDetailViewModel();

                TaskDetailmodel.TaskMailId = TMReturn.Id;
                TaskDetailmodel.PatientClinic = model.PracticeId;
                TaskDetailmodel.PatientName = model.Receiver.Name;
                TaskDetailmodel.PatientAddress = model.Receiver.Address_Line2;
                TaskDetailmodel.Remarks = "-";

                TaskDetailmodel.CreatedOn = DateTime.Now;
                TaskDetailmodel.IsDeleted = false;
                TaskDetailmodel.IsActive = true;
                _service.AddTaskDetail(TaskDetailmodel);

            }




            return RedirectToAction("InProcessOrders", "InProcess");
        }

        public async Task<ActionResult> CreatingOrder(List<int> Id, DateTime PickUpDate)
        {

            string date = PickUpDate.ToString("yyyy-MM-dd");
            var orders = new List<OrderViewModel>();
            foreach (var item in Id)
            {
                var orderViewModel = orderService.GetById(item);
                orders.Add(orderViewModel);
            }

            orders = orders.Where(x => x.OrderStatusId == 1 || x.OrderStatusId == 3).ToList();

            foreach (var item in orders)
            {



                var order = new
                {

                    pickup_date = date,
                    first_mile_option = "pickup",
                    description = item.Description,
                    weight = new
                    {
                        value = "2",
                        units = "lb"
                    },
                    customer_reference = item.Customer_Reference,
                    sender = new
                    {
                        instructions = item.Sender.Instructions,
                        contact = new
                        {
                            name = item.Sender.Name,
                            phone = item.Sender.Phone,
                            company = item.Sender.Company
                        },
                        address = new
                        {
                            address_line1 = item.Sender.Address_Line1,
                            suburb = item.Sender.Suburb,
                            postcode = item.Sender.Postcode,
                            state_name = item.Sender.State_Name,
                            country = item.Sender.Country
                        }
                    },
                    receiver = new
                    {
                        instructions = item.Receiver.Instructions,
                        contact = new
                        {
                            name = item.Receiver.Name,
                            email = item.Receiver.Email,
                            company = item.Receiver.Company
                        },
                        address = new
                        {
                            address_line1 = item.Receiver.Address_Line1,
                            suburb = item.Receiver.Suburb,
                            postcode = item.Receiver.Postcode,
                            state_name = item.Receiver.State_Name,
                            country = item.Receiver.Country
                        }
                    }
                };



                using (var client = Utility.GetHttpClient())
                {
                    var myContent = JsonConvert.SerializeObject(order);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");
                    var responseTask = client.PostAsync("https://api.sendle.com/api/orders", byteContent);//
                    responseTask.Wait();
                    var result = responseTask.Result;
                    var responsetext = await result.Content.ReadAsStringAsync();
                    if (result.IsSuccessStatusCode)
                    {

                        dynamic data = JObject.Parse(responsetext);
                        JArray array = data.labels;


                        var model = new APIResponseViewModel
                        {
                            OrderId = item.Id,
                            ResponseText = responsetext,
                            SendleOrderId = data.order_id,
                            SendleOrderUrl = data.order_url,
                            SendleRefrence = data.sendle_reference,
                            SendleTrackingUrl = data.tracking_url,
                            CreatedBy = Convert.ToInt32(Session["UserId"]),
                            CreatedOn = DateTime.Now,
                            IsDeleted = false,
                            IsActive = true
                        };
                        _APIResponseService.Add(model);
                        var model1 = new OrderLabelViewModel
                        {
                            OrderId = item.Id,
                            Formate = array[0]["format"].ToString(),
                            Size = array[0]["size"].ToString(),
                            Url = array[0]["url"].ToString()
                        };

                        orderLabelService.Add(model1);



                        orderService.UpdateOrderStatus(item.Id, 2);

                        //Order Log
                        var orderLog = new OrderLogViewModel
                        {
                            DeviceTypeId = item.DeviceTypeId,
                            OrderId = item.Id,
                            ReceiverId = item.ReceiverId,
                            SendleRefrence = data.sendle_reference
                        };

                        _orderLogService.Add(orderLog);
                    }
                    else
                    {
                        var model = new APIResponseViewModel
                        {
                            OrderId = item.Id,
                            ResponseText = responsetext,
                            CreatedBy = Convert.ToInt32(Session["UserId"]),
                            CreatedOn = DateTime.Now,
                            IsDeleted = false,
                            IsActive = true
                        };
                        _APIResponseService.Add(model);



                        orderService.UpdateOrderStatus(item.Id, 3);

                    }
                }
            }
            return Json(true);
        }

        public ActionResult CreateSingleOrder(int Id)
        {
            var data = orderService.GetById(Id);
            return View(data);
        }


        [HttpGet]
        public ActionResult ErrorPage()
        {
            var res = TempData["responseText"];
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SingleCreateOrder(OrderViewModel model, DateTime Pickup_Date)
        {
            var instructions = "";
            if (model.DeviceTypeId == 1)
            {
                instructions = "Please Call Support Number";
            }
            else if (model.DeviceTypeId == 2)
            {
                instructions = "Please Call Support Number -B";
            }
            else if (model.DeviceTypeId == 3)
            {
                instructions = "Please Call Support Number -HP";
            }
            else if (model.DeviceTypeId == 4)
            {
                instructions = "Please Call Support Number -Adp";
            }
            else if (model.DeviceTypeId == 7)
            {
                instructions = "Please Call Support Number -Duplicate";
            }
            else if (model.DeviceTypeId == 10)
            {
                instructions = "Please Call Support Number -Cuff(s)";
            }
            else
            {
                instructions = "Please Call Support Number";
            }
            var item = orderService.GetById(model.Id);
            string date = Pickup_Date.ToString("yyyy-MM-dd");
            var order = new
            {

                pickup_date = date,
                first_mile_option = "pickup",
                description = item.Description,
                weight = new
                {
                    value = "2",
                    units = "lb"
                },
                //customer_reference = item.Customer_Reference,
                sender = new
                {
                    instructions = instructions,
                    contact = new
                    {
                        name = item.Sender.Name,
                        phone = item.Sender.Phone,
                        company = item.Sender.Company
                    },
                    address = new
                    {
                        address_line1 = item.Sender.Address_Line1,
                        suburb = item.Sender.Suburb,
                        postcode = item.Sender.Postcode,
                        state_name = item.Sender.State_Name,
                        country = item.Sender.Country
                    }
                },
                receiver = new
                {
                    instructions = instructions,
                    contact = new
                    {
                        name = item.Receiver.Name,
                        phone = item.Receiver.Phone,
                        email = item.Receiver.Email,
                        company = item.Receiver.Company
                    },
                    address = new
                    {
                        address_line1 = item.Receiver.Address_Line1,
                        suburb = item.Receiver.Suburb,
                        postcode = item.Receiver.Postcode,
                        state_name = item.Receiver.State_Name,
                        country = item.Receiver.Country
                    }
                }
            };
            using (var client = Utility.GetHttpClient())
            {
                var myContent = JsonConvert.SerializeObject(order);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");
                var responseTask = client.PostAsync("https://api.sendle.com/api/orders", byteContent);
                responseTask.Wait();
                var result = responseTask.Result;
                var responsetext = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    dynamic data = JObject.Parse(responsetext);
                    JArray array = data.labels;
                    var newModel = new OrderViewModel()
                    {
                        ReceiverId = item.ReceiverId,
                        SourceId = model.SourceId,
                        DeviceTypeId = model.DeviceTypeId,
                        PracticeId = item.PracticeId,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        PatientId = item.ReceiverId.Value,
                        NewPracticeId = item.NewPracticeId
                    };

                    var singleOrder = orderService.AddSingleOrder(newModel);
                    var apiResp = new APIResponseViewModel
                    {
                        OrderId = singleOrder.Id,
                        ResponseText = responsetext,
                        SendleOrderId = data.order_id,
                        SendleOrderUrl = data.order_url,
                        SendleRefrence = data.sendle_reference,
                        SendleTrackingUrl = data.tracking_url,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };
                    _APIResponseService.Add(apiResp);
                    var orderLabel = new OrderLabelViewModel
                    {
                        OrderId = singleOrder.Id,
                        Formate = array[0]["format"].ToString(),
                        Size = array[0]["size"].ToString(),
                        Url = array[0]["url"].ToString()
                    };
                    orderLabelService.Add(orderLabel);
                    orderService.UpdateOrderStatus(singleOrder.Id, 2);
                    //Order Log

                    var orderLog = new OrderLogViewModel
                    {
                        DeviceTypeId = model.DeviceTypeId,
                        OrderId = singleOrder.Id,
                        ReceiverId = singleOrder.ReceiverId,
                        ReferenceOrderId = model.Id,
                        SendleRefrence = data.sendle_reference
                    };
                    _orderLogService.Add(orderLog);
                }
            }
            return RedirectToAction("ReceiverList", "ReceiverList");

        }

        [HttpGet]
        public ActionResult MakeReturnorReplacement(ReturnReplacementViewModel model)
        {
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> MakeReturnorReplacementOrders(ReturnReplacementViewModel model, DateTime Pickup_Date)
        {

            if (Request["Return"] != null)
            {
                var returnData = await ReturnForBulkPackages(model, Pickup_Date);
                if (returnData.IsOk == false)
                {
                    TempData["responseText"] = returnData.ResponseText;
                    return RedirectToAction("ErrorPage");
                }
            }
            if (Request["Replacement"] != null)
            {
                var replaceData = await ReplacementForBulkPackages(model, Pickup_Date);
                if (replaceData.IsOk == false)
                {
                    TempData["responseText"] = replaceData.ResponseText;
                    return RedirectToAction("ErrorPage");
                }
                var returnData = await ReplacementReturnForBulkPackages(model, Pickup_Date);
                if (returnData.IsOk == false)
                {
                    TempData["responseText"] = replaceData.ResponseText;
                    return RedirectToAction("ErrorPage");
                }
            }
            return RedirectToAction("ReceiverList", "ReceiverList");
        }

        //Return BulkPackages
        public async Task<ResponseMessageViewModel> ReturnForBulkPackages(ReturnReplacementViewModel model, DateTime PickUpDate)
        {
            string date = PickUpDate.ToString("yyyy-MM-dd");

            var senderData = orderService.GetSender();

            var order = new
            {

                pickup_date = date,
                first_mile_option = "pickup",
                description = model.Instructions,
                weight = new
                {
                    value = "2",
                    units = "lb"
                },
                //customer_reference = item.Customer_Reference,
                sender = new
                {
                    instructions = "Please Call Support Number - Bulk_R",
                    contact = new
                    {
                        name = model.Name,
                        phone = model.Phone,
                        company = "-"
                    },
                    address = new
                    {
                        address_line1 = model.Address_Line1,
                        suburb = model.Suburb,
                        postcode = model.Postcode,
                        state_name = model.State_Name,
                        country = model.Country
                    }
                },
                receiver = new
                {
                    instructions = "Please Call Support Number - Bulk_R",
                    contact = new
                    {
                        name = "Rizwan Khan",
                        phone = senderData.Phone,
                        email = senderData.Email,
                        company = senderData.Company
                    },
                    address = new
                    {
                        address_line1 = senderData.Address_Line1,
                        suburb = senderData.Suburb,
                        postcode = senderData.Postcode,
                        state_name = senderData.State_Name,
                        country = senderData.Country
                    }
                }
            };
            using (var client = Utility.GetHttpClient())
            {
                var myContent = JsonConvert.SerializeObject(order);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");
                var responseTask = client.PostAsync("https://api.sendle.com/api/orders", byteContent);//
                responseTask.Wait();
                var result = responseTask.Result;
                var responsetext = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    dynamic data = JObject.Parse(responsetext);
                    JArray array = data.labels;
                    var practiceId = newPractice.GetPracticeNameById(model.NewPracticeId.Value);
                    var newReceiverModel = new ReceiverViewModel()
                    {
                        Instructions = model.Instructions,
                        CreatedOn = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = model.Name,
                        Phone = model.Phone,
                        Email = model.Email,
                        Address_Line1 = model.Address_Line1,
                        Address_Line2 = model.Address_Line2,
                        Suburb = model.Suburb,
                        Postcode = model.Postcode,
                        State_Name = model.State_Name,
                        Country = model.Country,

                    };
                    var savedReceiverData = orderService.AddReceiver(newReceiverModel);

                    var newModel = new OrderViewModel()
                    {
                        ReceiverId = savedReceiverData.Id,
                        SourceId = model.SourceId,
                        DeviceTypeId = 6,
                        PracticeId = practiceId.Name,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        SenderId = null,
                        NewPracticeId = model.NewPracticeId,
                        PatientId = savedReceiverData.Id
                    };

                    var returnOrder = orderService.AddSingleOrder(newModel);

                    var apiResp = new APIResponseViewModel
                    {
                        OrderId = returnOrder.Id,
                        ResponseText = responsetext,
                        SendleOrderId = data.order_id,
                        SendleOrderUrl = data.order_url,
                        SendleRefrence = data.sendle_reference,
                        SendleTrackingUrl = data.tracking_url,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };
                    _APIResponseService.Add(apiResp);
                    var orderLabel = new OrderLabelViewModel
                    {
                        OrderId = returnOrder.Id,
                        Formate = array[0]["format"].ToString(),
                        Size = array[0]["size"].ToString(),
                        Url = array[0]["url"].ToString()
                    };

                    orderLabelService.Add(orderLabel);
                    orderService.UpdateOrderStatus(returnOrder.Id, 2);

                    //Order Log
                    var orderLog = new OrderLogViewModel
                    {
                        DeviceTypeId = returnOrder.DeviceTypeId,
                        OrderId = returnOrder.Id,
                        ReceiverId = returnOrder.ReceiverId,
                        ReferenceOrderId = returnOrder.Id,
                        SendleRefrence = data.sendle_reference
                    };
                    _orderLogService.Add(orderLog);
                    var msg = new ResponseMessageViewModel
                    {
                        ResponseText = responsetext,
                        IsOk = false
                    };
                    return msg;

                }
                else
                {
                    var msg = new ResponseMessageViewModel
                    {
                        ResponseText = responsetext,
                        IsOk = false
                    };
                    return msg;
                }
            }
        }

        //Replacement BulkPackages
        public async Task<ResponseMessageViewModel> ReplacementForBulkPackages(ReturnReplacementViewModel model, DateTime PickUpDate)
        {
            
            string date = PickUpDate.ToString("yyyy-MM-dd");

            var senderData = orderService.GetSender();

            var newOrder = new
            {
                pickup_date = date,
                first_mile_option = "pickup",
                description = model.Instructions,
                weight = new
                {
                    value = "2",
                    units = "lb"
                },
                sender = new
                {
                    instructions = "Please Call Support Number - ReplacementBulk/New",
                    contact = new
                    {
                        name = "Rizwan Khan",
                        phone = senderData.Phone,
                        company = senderData.Company
                    },
                    address = new
                    {
                        address_line1 = senderData.Address_Line1,
                        suburb = senderData.Suburb,
                        postcode = senderData.Postcode,
                        state_name = senderData.State_Name,
                        country = senderData.Country
                    }
                },
                receiver = new
                {
                    instructions = "Please Call Support Number - ReplacementBulk/New",
                    contact = new
                    {
                        name = model.Name,
                        email = model.Email,
                        company = "-"
                    },
                    address = new
                    {
                        address_line1 = model.Address_Line1,
                        suburb = model.Suburb,
                        postcode = model.Postcode,
                        state_name = model.State_Name,
                        country = model.Country
                    }
                }
            };

            using (var client = Utility.GetHttpClient())
            {
                var myContent = JsonConvert.SerializeObject(newOrder);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");
                var responseTask = client.PostAsync("https://api.sendle.com/api/orders", byteContent);//
                responseTask.Wait();
                var result = responseTask.Result;
                var responsetext = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {

                    dynamic data = JObject.Parse(responsetext);
                    JArray array = data.labels;
                    var practiceId = newPractice.GetPracticeNameById(model.NewPracticeId.Value);
                    var newReceiverModel = new ReceiverViewModel()
                    {
                        Instructions = model.Instructions,
                        CreatedOn = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = model.Name,
                        Phone = model.Phone,
                        Email = model.Email,
                        Address_Line1 = model.Address_Line1,
                        Address_Line2 = model.Address_Line2,
                        Suburb = model.Suburb,
                        Postcode = model.Postcode,
                        State_Name = model.State_Name,
                        Country = model.Country,

                    };

                    var savedReceiverData = orderService.AddReceiver(newReceiverModel);
                    ReceiverIdForBulkReplacement = savedReceiverData.Id;
                    var newModel = new OrderViewModel()
                    {
                        ReceiverId = savedReceiverData.Id,
                        SourceId = model.SourceId,
                        DeviceTypeId = 8,
                        PracticeId = practiceId.Name,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        PatientId = savedReceiverData.Id,
                        NewPracticeId = model.NewPracticeId
                    };

                    var replacement = orderService.AddSingleOrder(newModel);
                    orderIdForLog = replacement.Id;
                    var modelAPI = new APIResponseViewModel
                    {
                        OrderId = replacement.Id,
                        ResponseText = responsetext,
                        SendleOrderId = data.order_id,
                        SendleOrderUrl = data.order_url,
                        SendleRefrence = data.sendle_reference,
                        SendleTrackingUrl = data.tracking_url,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };
                    _APIResponseService.Add(modelAPI);
                    var model1 = new OrderLabelViewModel
                    {
                        OrderId = replacement.Id,
                        Formate = array[0]["format"].ToString(),
                        Size = array[0]["size"].ToString(),
                        Url = array[0]["url"].ToString()
                    };

                    orderLabelService.Add(model1);

                    orderService.UpdateOrderStatus(replacement.Id, 2);

                    //Order Log
                    var orderLog = new OrderLogViewModel
                    {
                        DeviceTypeId = 1,
                        OrderId = replacement.Id,
                        ReceiverId = replacement.ReceiverId,
                        ReferenceOrderId = replacement.Id,
                        SendleRefrence = data.sendle_reference
                    };
                    _orderLogService.Add(orderLog);
                    var msg = new ResponseMessageViewModel
                    {
                        ResponseText = responsetext,
                        IsOk = true
                    };
                    return msg;
                }
                else
                {
                    var msg = new ResponseMessageViewModel
                    {
                        ResponseText = responsetext,
                        IsOk = false
                    };
                    return msg;

                }
            }
        }

        public async Task<ResponseMessageViewModel> ReplacementReturnForBulkPackages(ReturnReplacementViewModel model, DateTime PickUpDate)
        {
           
            string date = PickUpDate.ToString("yyyy-MM-dd");

            var senderData = orderService.GetSender();

            var returnOrder = new
            {

                pickup_date = date,
                first_mile_option = "pickup",
                description = model.Instructions,
                weight = new
                {
                    value = "2",
                    units = "lb"
                },
                //customer_reference = item.Customer_Reference,
                sender = new
                {
                    instructions = "Please Call Support Number - ReplacementBulk/Return",
                    contact = new
                    {
                        name = model.Name,
                        phone = model.Phone,
                        company = "-"
                    },
                    address = new
                    {
                        address_line1 = model.Address_Line1,
                        suburb = model.Suburb,
                        postcode = model.Postcode,
                        state_name = model.State_Name,
                        country = model.Country
                    }
                },
                receiver = new
                {
                    instructions = "Please Call Support Number - Replacement/Return",
                    contact = new
                    {
                        name = "Rizwan Khan",
                        phone = senderData.Phone,
                        email = senderData.Email,
                        company = senderData.Company
                    },
                    address = new
                    {
                        address_line1 = senderData.Address_Line1,
                        suburb = senderData.Suburb,
                        postcode = senderData.Postcode,
                        state_name = senderData.State_Name,
                        country = senderData.Country
                    }
                }
            };
            using (var client = Utility.GetHttpClient())
            {
                var myContent1 = JsonConvert.SerializeObject(returnOrder);
                var buffer1 = System.Text.Encoding.UTF8.GetBytes(myContent1);
                var byteContent1 = new ByteArrayContent(buffer1);
                byteContent1.Headers.ContentType = new MediaTypeHeaderValue("application/Json");
                var responseTask1 = client.PostAsync("https://api.sendle.com/api/orders", byteContent1);//
                responseTask1.Wait();
                var result1 = responseTask1.Result;
                var responsetext1 = await result1.Content.ReadAsStringAsync();
                if (result1.IsSuccessStatusCode)
                {
                    dynamic data = JObject.Parse(responsetext1);
                    JArray array = data.labels;
                    var practiceId = newPractice.GetPracticeNameById(model.NewPracticeId.Value);
                    var newModel = new OrderViewModel()
                    {
                        ReceiverId = ReceiverIdForBulkReplacement,
                        SourceId = model.SourceId,
                        DeviceTypeId = 8,
                        PracticeId = practiceId.Name,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        SenderId = null,
                        PatientId = ReceiverIdForBulkReplacement,
                        NewPracticeId = model.NewPracticeId
                    };

                    var replacement = orderService.AddSingleReturnOrder(newModel);
                    var modelAPI = new APIResponseViewModel
                    {
                        OrderId = replacement.Id,
                        ResponseText = responsetext1,
                        SendleOrderId = data.order_id,
                        SendleOrderUrl = data.order_url,
                        SendleRefrence = data.sendle_reference,
                        SendleTrackingUrl = data.tracking_url,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };
                    _APIResponseService.Add(modelAPI);
                    var model1 = new OrderLabelViewModel
                    {
                        OrderId = replacement.Id,
                        Formate = array[0]["format"].ToString(),
                        Size = array[0]["size"].ToString(),
                        Url = array[0]["url"].ToString()
                    };

                    orderLabelService.Add(model1);

                    orderService.UpdateOrderStatus(replacement.Id, 2);


                    // Add Remarks
                    var orderLog = new OrderLogViewModel
                    {
                        DeviceTypeId = 6,
                        OrderId = replacement.Id,
                        ReceiverId = replacement.ReceiverId,
                        ReferenceOrderId = orderIdForLog,
                        SendleRefrence = data.sendle_reference
                    };
                    _orderLogService.Add(orderLog);
                    var msg = new ResponseMessageViewModel
                    {
                        ResponseText = responsetext1,
                        IsOk = true
                    };
                    return msg;
                }
                else
                {
                    var msg = new ResponseMessageViewModel
                    {
                        ResponseText = responsetext1,
                        IsOk = false
                    };
                    return msg;
                }
            }

        }

    }
}