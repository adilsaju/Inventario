using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Inventario.ServiceContracts
{
   public class AssignViewModel
    {
        public int S_No { get; set; }
        [Required]
        [Display(Name ="Employee ID")]
        public string Emp_Id { get; set; }
        [Required]
        [Display(Name="Device Id")]
        public string Device_Id { get; set; }
    }
}
