using CourierService.Entities.TaskManagements;
using CourierService.Repositories.TaskManagements;
using CourierService.ViewModels.DTOForTaskManagements;
using CourierService.ViewModels.Orders;
using CourierService.ViewModels.TaskManagements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services.TaskManagements
{
  public class TaskService : AutoMapperProfile
    {
        TaskRepository _repo = new TaskRepository();
        public IEnumerable<TaskDetailViewModel> GetAllTaskDetail()
        {
            var data = _repo.GetAllTaskDetail();
            var res = _mapper.Map<IEnumerable<TaskDetailViewModel>>(data);
            return res;
        }
        public TaskDetailViewModel GetTaskDetail(int id)
        {
            var data = _repo.GetTaskDetail(id);
            var res = _mapper.Map<TaskDetailViewModel>(data);
            return res;
        }

        public TaskDetail AddTaskDetail(TaskDetailViewModel model)
        {
            var res = _mapper.Map<TaskDetail>(model);
            var data = _repo.AddTaskDetail(res);
            return data;
        }

        public bool UpdateTaskDetail(TaskDetailViewModel model)
        {
            var res = _mapper.Map<TaskDetail>(model);
            var data = _repo.UpdateTaskDetail(res);
            return data;
        }

        public bool DeleteTaskDetail(int id)
        {
            var data = _repo.DeleteTaskDetail(id);
            return data;
        }


        //Task Mail
        public IEnumerable<TaskMailViewModel> GetAllTaskMail()
        {
            var data = _repo.GetAllTaskMail();
            var res = _mapper.Map<IEnumerable<TaskMailViewModel>>(data);
            return res;
        }
        public TaskMailViewModel GetTaskMail(int id)
        {
            var data = _repo.GetTaskMail(id);
            var res = _mapper.Map<TaskMailViewModel>(data);
            return res;
        }

        public TaskMail AddTaskMail(TaskMailViewModel model)
        {
            var res = _mapper.Map<TaskMail>(model);
            var data = _repo.AddTaskMail(res);
            return data;
        }
        public bool UpdateTaskMail(TaskMailViewModel model)
        {
            var res = _mapper.Map<TaskMail>(model);
            var data = _repo.UpdateTaskMail(res);
            return data;
        }

        public bool DeleteTaskMail(int id)
        {
            var data = _repo.DeleteTaskMail(id);
            return data;
        }

        //
        public bool IsStatusUpdate(int id)
        {
            var res = _repo.IsStatusUpdate(id);
            return res;
        }

        public bool UpdateSendelReference(TaskDetailViewModel taskDetail)
        {
            var data = _mapper.Map<TaskDetail>(taskDetail);
            var res = _repo.UpdateSendelReference(data);
            return res;
        }
        //Practice

        //public IEnumerable<PracticeViewModel> GetAllPractice()
        //{
        //    var data = _repo.GetAllPractice();
        //    var res = _mapper.Map<IEnumerable<PracticeViewModel>>(data);
        //    return res;
        //}

        //public Practice AddPractice(PracticeViewModel model)
        //{
        //    var res = _mapper.Map<Practice>(model);
        //    var data = _repo.AddPractice(res);
        //    return data;
        //}


        public APIResponseViewModel GetBySendleRefrence(string SendleRefrence)
        {
            var data = _repo.GetBySendleRefrence(SendleRefrence);
            var res = _mapper.Map<APIResponseViewModel>(data);
            return res;
        }



        // For Tabing
        public IEnumerable<TaskMailDTOViewModel> GetAllPendingTasks()
        {
            var data = _repo.GetAllPendingTasks();
            var res = _mapper.Map<IEnumerable<TaskMailDTOViewModel>>(data);
            return res;
        }
        public IEnumerable<TaskMailDTOViewModel> GetAllCompletedTasks()
        {
            var data = _repo.GetAllCompletedTasks();
            var res = _mapper.Map<IEnumerable<TaskMailDTOViewModel>>(data);
            return res;
        }

        public bool CompleteAll(int id)
        {
            var res = _repo.CompleteAll(id);
            return res;
        }

        public IEnumerable<TaskMailDTOViewModel> FilterPendingTask(TaskDetailViewModel model)
        {
            var task = _mapper.Map<TaskDetail>(model);
            var data = _repo.FilterPendingTask(task);
            var res = _mapper.Map<IEnumerable<TaskMailDTOViewModel>>(data);
            return res;
        }

        public IEnumerable<TaskMailDTOViewModel> FilterCompletedTask(TaskDetailViewModel model)
        {
            var task = _mapper.Map<TaskDetail>(model);
            var data = _repo.FilterCompletedTask(task);
            var res = _mapper.Map<IEnumerable<TaskMailDTOViewModel>>(data);
            return res;
        }

    }
}
