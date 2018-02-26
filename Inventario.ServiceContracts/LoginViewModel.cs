using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Inventario.ServiceContracts
{
   public class LoginViewModel
    {
        [Required]
        public string username { get; set; }
        [Required]
       public string password { get; set; }
        public string Role { get; set; }
    }
}
