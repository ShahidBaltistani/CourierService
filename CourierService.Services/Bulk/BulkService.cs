using CourierService.Repositories.Bulk;
using CourierService.ViewModels.Bulk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services.Bulk
{
    public class BulkService : AutoMapperProfile
    {
        BulkPackageRepository Bulk = new BulkPackageRepository();

        public IEnumerable<BulkPackageViewModel> GetAll()
        {
            var data = Bulk.GetAll();
            var res = _mapper.Map<IEnumerable<BulkPackageViewModel>>(data);
            return res;
        }


    }
}
