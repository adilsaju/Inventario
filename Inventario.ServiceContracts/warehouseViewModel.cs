using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Inventario.ServiceContracts
{
    public class warehouseViewModel
    {
        public int? S_No { get; set; }
        [Display (Name ="Device ID")]
        [Required]
        public string Device_ID { get; set; }
        [Display(Name = "Serial No")]
        [Required]
        public string Serial_No { get; set; }
        [Display(Name = "Device Name")]
        [Required]
        public string Device_Name { get; set; }
        [Display(Name = "Device Type")]
        [Required]
        public string Device_Type { get; set; }
        [Display(Name = "Processor")]
        [Required]
        public string Processor { get; set; }
        
        [Required]
        public string Remarks { get; set; }
        [Display(Name = "Purchase Date")]
        [Required]
        public System.DateTime Purchase_Date { get; set; }

        [Required]
        public string Status { get; set; }

        public string date { get; set; }
    }
}
