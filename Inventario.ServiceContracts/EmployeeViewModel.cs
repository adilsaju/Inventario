using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Inventario.ServiceContracts
{
   public class EmployeeViewModel
    {
        [Display(Name ="Serial No")]
        [Required]
        public int S_No { get; set; }
        [Display(Name = "Employee ID")]
        [Required]
        public string Employee_ID { get; set; }
        [Display(Name = "Employee Name")]
        [Required]
        public string Emp_Name { get; set; }

        [Required]
        public string Designation { get; set; }
        [Display(Name = "Email")]
        [Required]
        public string Email_Id { get; set; }
        [Display(Name = "Date of joining")]
        [Required]
        public System.DateTime Date_of_joining { get; set; }

        public string date { get; set; }
      
    }
}
