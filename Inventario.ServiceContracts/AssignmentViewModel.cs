using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Inventario.ServiceContracts
{
   public class AssignmentViewModel
    {
        public int S_No { get; set; }
        [Display(Name = "Device Id")]
        public string device_id {get;set;}
        [Display(Name = "Serial No")]
        public string Serial_No { get; set; }
        [Display(Name = "Device Type")]
        public string Device_type { get; set; }
        [Display(Name = "Device Name")]
        public string Device_Name { get; set; }
        [Display(Name = "Employee Name")]
        public string employee_name { get; set; }
        [Display(Name = "Employee Id")]
        public string emp_id { get; set; }

    }
}
