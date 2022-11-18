using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.NewPracticeF
{
    public class NewPracticeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid SVPracticeId { get; set; }
        public bool IsActive { get; set; }
    }
}
