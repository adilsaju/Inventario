using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Inventario.ServiceContracts
{
    public class HistoryViewModel
    {
        public EmployeeViewModel EmployeeViewModel { get; set;}

        public warehouseViewModel warehouseViewModel { get; set; }

        [Display(Name = "Serial No")]
        public int S_No { get; set; }
        [Display(Name = "Device Id")]
        public string Device_Id { get; set; }
        [Display(Name = "Employee Id")]
        public string Emp_Id { get; set; }
        
        public DateTime Date_assign { get; set; }
        
        public DateTime Date_unassign { get; set; }

        [Display(Name ="Employee Name")]
        public string Emp_Name {get;set;}
        [Display(Name = "Assigned Date")]
        public string assign { get; set;}
        [Display(Name = "Unassigned Date")]
        public string unassign { get; set; }

    }
}
