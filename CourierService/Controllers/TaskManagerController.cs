using CourierService.Services;
using CourierService.Services.Bulk;
using CourierService.Services.NewPracticeF;
using CourierService.Services.OrderLogs;
using CourierService.Services.Orders;
using CourierService.Services.TaskManagements;
using CourierService.ViewModels.NewPracticeF;
using CourierService.ViewModels.TaskManagements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CourierService.Controllers
{
    [Authorize]
    public class TaskManagerController : Controller
    {
        // Adding Service
        OrderService orderService = new OrderService();
        APIResponseService aPIResponseService = new APIResponseService();
        OrderLabelService orderLabelService = new OrderLabelService();
        OrderLogService _orderLogService = new OrderLogService();
        BulkService _Bulk = new BulkService();
        NewPracticeService newPractice = new NewPracticeService();
        //
        TaskService _service = new TaskService();
        // GET: TaskManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pending()
        {
            var data = _service.GetAllPendingTasks();
            return PartialView("_Pending", data);
        }

        public ActionResult Completed()
        {
            var data = _service.GetAllCompletedTasks();
            return PartialView("_Completed", data);
        }

        [HttpGet]
        public ActionResult AddMail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddMail(TaskMailViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedOn = DateTime.Now;
                model.IsDeleted = false;
                model.IsActive = true;
                _service.AddTaskMail(model);

            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult UpdateMail(int id)
        {
            var data = _service.GetTaskMail(id);
            return View(data);
        }
        [HttpPost]
        public ActionResult UpdateMail(TaskMailViewModel model)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateTaskMail(model);

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteMail(int id)
        {
           var data = _service.DeleteTaskMail(id);
            if (data == true)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddTaskDetail(TaskDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Replacement == true)
                {
                    var returns = model.Remarks+ "_(Return_Device)";
                    var news = model.Remarks + "_(Replacement_Device)";
                    for (int i =0; i < 2; i++)
                    {
                        model.CreatedOn = DateTime.Now;
                        model.IsDeleted = false;
                        model.IsActive = true;
                        model.Remarks =i == 0 ? returns : news;
                        _service.AddTaskDetail(model);
                    }
                }
                else
                {
                    model.CreatedOn = DateTime.Now;
                    model.IsDeleted = false;
                    model.IsActive = true;
                    _service.AddTaskDetail(model);
                }

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateTaskDetails(int id)
        {
            var data = _service.GetTaskDetail(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateTaskDetail(TaskDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateTaskDetail(model);

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteTaskDetail(int id)
        {
            var data = _service.DeleteTaskDetail(id);
            if (data == true)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult IsStatusUpdate(int id)
        {
            var data = _service.IsStatusUpdate(id);
            if (data == true)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult CompleteAll(int id)
        {
            var data = _service.CompleteAll(id);
            if (data == true)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSendelReference(TaskDetailViewModel taskDetail)
        {
            _service.UpdateSendelReference(taskDetail);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DownloadPdfLandScape(string SendleRefrence)
        {

            var jsonString = _service.GetBySendleRefrence(SendleRefrence);
            if (jsonString.ResponseText != null)
            {
                dynamic data = JObject.Parse(jsonString.ResponseText);
                JArray array = data.labels;
                string Url = array[1]["url"].ToString();
                using (var client = Utility.GetHttpClient())
                {
                    var responseTask = client.GetAsync(Url);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        orderService.UpdateIsPrinted(jsonString.OrderId.Value);

                        var contentStream = await result.Content.ReadAsStreamAsync();
                        return File(contentStream, "Label.pdf");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            else
            {
                string order_id = jsonString.SendleOrderId;
                using (var client = Utility.GetHttpClient())
                {
                    var responseTask = client.GetAsync("https://api.sendle.com/api/orders/" + order_id + "");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var response = await result.Content.ReadAsStringAsync();
                        dynamic data1 = JObject.Parse(response);
                        JArray array1 = data1.labels;
                        if (array1 == null)
                        {
                            return Json("Sorry ! Don't Print Label due to missing response text.", JsonRequestBehavior.AllowGet);
                        }
                        string Url = array1[1]["url"].ToString();

                        var responseTask1 = client.GetAsync(Url);
                        responseTask1.Wait();
                        var result1 = responseTask1.Result;
                        if (result1.IsSuccessStatusCode)
                        {
                            orderService.UpdateIsPrinted(jsonString.OrderId.Value);

                            var contentStream1 = await result1.Content.ReadAsStreamAsync();
                            return File(contentStream1, "Label.pdf");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error try after some time.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            return View();
        }



        public ActionResult GetAllPractices()
        {
            var data = newPractice.GetAll();
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddNewPractice(NewPracticeViewModel model)
        {
            try
            {
                newPractice.AddNewPractice(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public ActionResult FilterPendingTask(TaskDetailViewModel model)
        {
            var data = _service.FilterPendingTask(model);
            return PartialView("_Pending", data);
        }
        public ActionResult FilterCompletedTask(TaskDetailViewModel model)
        {
            var data = _service.FilterCompletedTask(model);
            return PartialView("_Completed", data);
        }

    }
}