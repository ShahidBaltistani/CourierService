using CourierService.Entities.NewPracticeF;
using CourierService.Repositories.NewPracticeF;
using CourierService.ViewModels.NewPracticeF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services.NewPracticeF
{
    public class NewPracticeService : AutoMapperProfile
    {
        NewPracticeRepository _repo = new NewPracticeRepository();

        public IEnumerable<NewPracticeViewModel> GetAll()
        {
            var data = _repo.GetAll();
            var res = _mapper.Map<IEnumerable<NewPracticeViewModel>>(data);
            return res;
        }
        public NewPracticeViewModel GetIdByPracticeName(string PracticeName)
        {
            var data = _repo.GetIdByPracticeName(PracticeName);
            var res = _mapper.Map<NewPracticeViewModel>(data);
            return res;
        }

        public NewPracticeViewModel GetPracticeNameById(int Id)
        {
            var data = _repo.GetPracticeNameById(Id);
            var res = _mapper.Map<NewPracticeViewModel>(data);
            return res;
        }
        public NewPractice AddNewPractice(NewPracticeViewModel model)
        {
            var res = _mapper.Map<NewPractice>(model);
            var data = _repo.AddNewPractice(res);
            return data;
        }
    }
}
