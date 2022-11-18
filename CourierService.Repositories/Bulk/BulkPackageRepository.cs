using CourierService.Database;
using CourierService.Entities.Bulk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Repositories.Bulk
{
    public class BulkPackageRepository
    {
        CourierServiceContext _context = new CourierServiceContext();
        public IEnumerable<BulkPackage> GetAll()
        {
            var data = _context.BulkPackages.ToList();
            return data;
        }
    }
}
