using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario.Data;
using Inventario.ServiceContracts;

namespace Inventario.Services
{
   public class UserServices
    {
        EmployeeContext employeecontext = new EmployeeContext();
        public UserViewModel getDetails(int id)
        {
            var data = (from db in employeecontext.EmployeeDetails
                        where db.S_No == id
                        select db).FirstOrDefault();
            UserViewModel user = new UserViewModel();
            user.EmployeeViewModel = new EmployeeViewModel
            {
                Designation = data.Designation,
                date = data.Date_of_joining.ToString("dd/MM/yyyy"),
                Employee_ID = data.Employee_ID,
                Emp_Name = data.Emp_Name,
                Email_Id = data.Email_Id,
            };
            var detail = (from ed in employeecontext.Assigns
                       where ed.Emp_Id == data.Employee_ID
                       select ed).ToList();
            List<warehouseViewModel> ware = new List<warehouseViewModel>();
            foreach (var item in detail)
            {
                var device = (from db in employeecontext.warehouses
                              where db.Device_ID == item.Device_Id
                              select db).First();
                ware.Add(new warehouseViewModel
                {
                    date = device.Purchase_Date.ToString("dd/MM/yyyy"),
                    Serial_No = device.Serial_No,
                    Device_ID = device.Device_ID,
                    Device_Type = device.Device_Type,
                    Device_Name = device.Device_Name,
                    Processor = device.Processor,
                    Status = device.Status,
                    Remarks = device.Remarks,
                });
             }
            user.warehouseViewModel = ware;

            return user;
        }
    }
}
