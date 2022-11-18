using CourierService.Database;
using CourierService.Entities.NewPracticeF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Repositories.NewPracticeF
{
    public class NewPracticeRepository
    {
        CourierServiceContext _context = new CourierServiceContext();
        public IEnumerable<NewPractice> GetAll()
        {
            var data = _context.NewPractices.ToList();
            return data;
        }
        public NewPractice GetIdByPracticeName(string PracticeName)
        {
            try
            {
                var data = _context.NewPractices.Where(x => x.Name.Replace(" ", "").ToLower() == PracticeName.Replace(" ", "").ToLower()).FirstOrDefault();
                if (data!= null)
                {
                    return data;
                }
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public NewPractice GetPracticeNameById(int Id)
        {
            try
            {
                var data = _context.NewPractices.Where(x => x.Id==Id).FirstOrDefault();
                if (data != null)
                {
                    return data;
                }
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public NewPractice AddNewPractice(NewPractice model)
        {
            try
            {
                var data = _context.NewPractices.Add(model);
                _context.SaveChanges();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
