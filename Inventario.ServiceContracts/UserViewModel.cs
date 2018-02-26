using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.ServiceContracts
{
   public class UserViewModel
    {
        public EmployeeViewModel EmployeeViewModel { get; set; }
        public List<warehouseViewModel> warehouseViewModel { get; set; }
    }
}
