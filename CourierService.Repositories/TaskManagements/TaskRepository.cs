using CourierService.Database;
using CourierService.Entities.TaskManagements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using CourierService.Entities.Orders;
using CourierService.Entities.DTOForTaskManagements;

namespace CourierService.Repositories.TaskManagements
{
   public class TaskRepository
    {
        CourierServiceContext _context = new CourierServiceContext();

        public IEnumerable<TaskDetail> GetAllTaskDetail()
        {
            var data = _context.TaskDetails
                .Include(x => x.TaskMail)
                       .ToList();

            return data;

        }

        public TaskDetail GetTaskDetail(int id)
        {
            var data = _context.TaskDetails
                   .Include(x => x.TaskMail)
           .FirstOrDefault(x=>x.Id == id);

            return data;

        }
        public TaskDetail AddTaskDetail(TaskDetail model)
        {
            try
            {
                var data = _context.TaskDetails.Add(model);
                _context.SaveChanges();
                if (data != null)
                {
                    var taskLog = new TaskLog();
                    _context.TaskLogs.Add(new TaskLog()
                    {
                        TaskStatus = false,
                        CreatedOn = DateTime.Now,
                        TaskDetailId = data.Id
                    });
                    _context.SaveChanges();
                }
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool UpdateTaskDetail(TaskDetail model)
        {
            try
            {
                if (model != null)
                {
                    var data = _context.Entry(model).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool DeleteTaskDetail(int Id)
        {
            try
            {
                var data = GetTaskDetail(Id);
                if (data != null)
                {
                    data.IsDeleted = true;
                    _context.SaveChanges();
                    return true;
                }
                return false;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        //Task Mail

        public IEnumerable<TaskMail> GetAllTaskMail()
        {
            var data = _context.TaskMails.OrderByDescending(x=>x.Id)
                .Include(x => x.TaskDetails)
                       .ToList();

            return data;

        }

        public TaskMail GetTaskMail(int id)
        {
            var data = _context.TaskMails.FirstOrDefault(x => x.Id == id);
            return data;

        }

        public TaskMail AddTaskMail(TaskMail model)
        {
            try
            {
                var data = _context.TaskMails.Add(model);
                _context.SaveChanges();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public bool UpdateTaskMail(TaskMail model)
        {
            try
            {
                if (model != null)
                {
                    var data = _context.Entry(model).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool DeleteTaskMail(int Id)
        {
            try
            {
                var data = GetTaskMail(Id);
                if (data != null)
                {
                     data.IsDeleted = true;
                    _context.SaveChanges();
                    return true;
                }
                return false;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool IsStatusUpdate(int id)
        {
            var data = _context.TaskDetails.Where(x => x.Id == id).FirstOrDefault();
            if (data == null)
            {
                return false;
            }
            else
            {
                if (data.Status == true)
                {
                    data.Status = false;
                    data.ModifiedOn = DateTime.Now;
                    _context.SaveChanges();
                    _context.TaskLogs.Add(new TaskLog()
                    {
                        TaskStatus = false,
                        CreatedOn = DateTime.Now,
                        TaskDetailId = data.Id
                    });
                    _context.SaveChanges();
                }
                else
                {
                    data.Status = true;
                    data.ModifiedOn = DateTime.Now;
                    _context.SaveChanges();
                    _context.TaskLogs.Add(new TaskLog()
                    {
                        TaskStatus = true,
                        CreatedOn = DateTime.Now,
                        TaskDetailId = data.Id
                    });
                    _context.SaveChanges();
                }

                var count = _context.TaskDetails.Where(x => x.TaskMailId == data.TaskMailId && x.Status == false).ToList().Count();

                    if (count == 0)
                    {
                       var mail =  _context.TaskMails.Where(x => x.Id == data.TaskMailId).FirstOrDefault();
                       mail.IsCompleted = true;
                       _context.SaveChanges();

                    }
                    else
                    {
                    var mail = _context.TaskMails.Where(x => x.Id == data.TaskMailId).FirstOrDefault();
                    mail.IsCompleted = false;
                    _context.SaveChanges();
                }

                return data.Status;
            }
        }


        public bool UpdateSendelReference(TaskDetail taskDetail)
        {
            var data = _context.TaskDetails.Where(x => x.Id == taskDetail.Id).FirstOrDefault();
            if (data == null)
            {
                return false;
            }
            else
            {
                data.SendleRefrence = taskDetail.SendleRefrence;
                _context.SaveChanges();
                return true;
            }
        }

        //Practice


        //public IEnumerable<Practice> GetAllPractice()
        //{
        //    var data = _context.Practices.ToList();
        //    return data;

        //}
        //public Practice AddPractice(Practice model)
        //{
        //    var data = _context.Practices.Add(model);
        //    _context.SaveChanges();
        //    return data;
        //}


        public APIResponse GetBySendleRefrence(string SendleRefrence)
        {
            var data = _context.APIResponses.Where(x => x.SendleRefrence == SendleRefrence).FirstOrDefault();
            return data;
        }



        // For Tab Working.
        public IEnumerable<TaskMailDTO> GetAllPendingTasks()
        {

            var data = _context.TaskMails.Where(x=>x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.Id).Include(x => x.TaskDetails)
                        .Select(x=> new TaskMailDTO()
                        { 
                            Id=x.Id,
                            From=x.From,
                            Subject=x.Subject,
                            ReceivedDate=x.ReceivedDate,
                            Description=x.Description,
                            IsCompleted = x.IsCompleted,
                            TaskDetails=x.TaskDetails.Where(y=>y.Status==false && y.IsActive == true && y.IsDeleted == false).Select(td=> new TaskDetailDTO()
                            {
                                Id=td.Id,
                                Remarks=td.Remarks,
                                IsDeleted =td.IsDeleted,
                                IsActive =td.IsActive,
                                PatientName=td.PatientName,
                                PatientAddress=td.PatientAddress,
                                PatientClinic=td.PatientClinic,
                                Status=td.Status,
                                TaskMailId=td.TaskMailId,
                                SendleRefrence=td.SendleRefrence
                            }).ToList()
                        })
                        .ToList();

            return data;
        }
        public IEnumerable<dynamic> GetAllCompletedTasks()
        {

            var data = _context.TaskMails.Where(x=>x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.Id).Include(x => x.TaskDetails)
                        .Select(x => new TaskMailDTO()
                        {
                            Id = x.Id,
                            From = x.From,
                            Subject = x.Subject,
                            ReceivedDate = x.ReceivedDate,
                            Description = x.Description,
                            IsCompleted = x.IsCompleted,
                            TaskDetails = x.TaskDetails.Where(y => y.Status == true && y.IsActive == true && y.IsDeleted == false).Select(td => new TaskDetailDTO()
                            {
                                Id = td.Id,
                                Remarks = td.Remarks,
                                PatientName = td.PatientName,
                                PatientAddress = td.PatientAddress,
                                PatientClinic = td.PatientClinic,
                                Status = td.Status,
                                TaskMailId = td.TaskMailId,
                                SendleRefrence = td.SendleRefrence
                            }).ToList()
                        })
                        .ToList();

            return data;
        }

        public bool CompleteAll(int Id)
        {
            try
            {
                var data = GetTaskMail(Id);
                if (data != null)
                {
                    data.ModifiedOn = DateTime.Now;
                    data.IsCompleted = true;
                    var data1 = _context.TaskDetails.Where(x=>x.TaskMailId == Id).ToList();
                    foreach (var item in data1)
                    {
                        item.Status = true;
                        item.ModifiedOn = DateTime.Now;
                    }
                    _context.SaveChanges();
                    return true;
                }
                return false;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IEnumerable<TaskMailDTO> FilterPendingTask(TaskDetail model)
        {
            var data = _context.TaskMails.Where(x => x.IsActive == true && x.IsDeleted == false).Include(x => x.TaskDetails)
                        .Select(x => new TaskMailDTO()
                        {
                            Id = x.Id,
                            From = x.From,
                            Subject = x.Subject,
                            ReceivedDate = x.ReceivedDate,
                            Description = x.Description,
                            IsCompleted = x.IsCompleted,
                            TaskDetails = x.TaskDetails.Where(y => y.Status == false && y.IsActive == true && y.IsDeleted == false)
                            .Select(td => new TaskDetailDTO()
                            {
                                Id = td.Id,
                                Remarks = td.Remarks,
                                PatientName = td.PatientName,
                                PatientAddress = td.PatientAddress,
                                PatientClinic = td.PatientClinic,
                                Status = td.Status,
                                TaskMailId = td.TaskMailId,
                                SendleRefrence = td.SendleRefrence
                            }).ToList()
                        }).Where(x =>
                        (x.From.Trim().ToLower().Contains(model.TaskMail.From.Trim().ToLower())
                        || DbFunctions.TruncateTime(x.ReceivedDate) == model.TaskMail.ReceivedDate)
                        || x.TaskDetails.FirstOrDefault().PatientName.ToLower().Contains(model.PatientName.Trim().ToLower())
                        || x.TaskDetails.FirstOrDefault().PatientClinic.Trim().ToLower().Contains(model.PatientClinic.Trim().ToLower())
                        || x.TaskDetails.FirstOrDefault().PatientAddress.Trim().ToLower().Contains(model.PatientAddress.Trim().ToLower())
                        || x.TaskDetails.FirstOrDefault().SendleRefrence.Trim().ToLower().Contains(model.SendleRefrence.Trim().ToLower())
                        ).ToList();
            return data;
        }

        public IEnumerable<TaskMailDTO> FilterCompletedTask(TaskDetail model)
        {

            var data = _context.TaskMails.Where(x => x.IsActive == true && x.IsDeleted == false).Include(x => x.TaskDetails)
                        .Select(x => new TaskMailDTO()
                        {
                            Id = x.Id,
                            From = x.From,
                            Subject = x.Subject,
                            ReceivedDate = x.ReceivedDate,
                            Description = x.Description,
                            IsCompleted = x.IsCompleted,
                            TaskDetails = x.TaskDetails.Where(y => y.Status == true && y.IsActive == true && y.IsDeleted == false)
                            .Select(td => new TaskDetailDTO()
                            {
                                Id = td.Id,
                                Remarks = td.Remarks,
                                PatientName = td.PatientName,
                                PatientAddress = td.PatientAddress,
                                PatientClinic = td.PatientClinic,
                                Status = td.Status,
                                TaskMailId = td.TaskMailId,
                                SendleRefrence = td.SendleRefrence
                            }).ToList()
                        }).Where(x =>
                        (x.From.Trim().ToLower().Contains(model.TaskMail.From.Trim().ToLower())
                        || DbFunctions.TruncateTime(x.ReceivedDate) == model.TaskMail.ReceivedDate)
                        || x.TaskDetails.FirstOrDefault().PatientName.ToLower().Contains(model.PatientName.Trim().ToLower())
                        || x.TaskDetails.FirstOrDefault().PatientClinic.Trim().ToLower().Contains(model.PatientClinic.Trim().ToLower())
                        || x.TaskDetails.FirstOrDefault().PatientAddress.Trim().ToLower().Contains(model.PatientAddress.Trim().ToLower())
                        || x.TaskDetails.FirstOrDefault().SendleRefrence.Trim().ToLower().Contains(model.SendleRefrence.Trim().ToLower())
                        ).ToList();
            return data;
        }
    }
}
