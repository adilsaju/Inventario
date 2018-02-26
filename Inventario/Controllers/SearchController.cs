using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventario.Services;
using Inventario.ServiceContracts;
using Inventario.Filters;

namespace Inventario.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        
        warehouseService warehouseservice = new warehouseService();
        SearchService searchservice = new SearchService();
        EmployeeService employeeService = new EmployeeService();
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Index(string id)
        {
            
            List<HistoryViewModel> history = new List<HistoryViewModel>();
            IEnumerable<warehouseViewModel> res;
            IEnumerable<EmployeeViewModel> emp;
            if (id != "")
            {
                res = warehouseservice.getAsset().Where(s => (s.Device_ID.Replace(" ", String.Empty).Contains(id)) || (s.Device_Name.Replace(" ", String.Empty) == id) || s.Processor.Replace(" ", String.Empty) == id || s.Remarks.Replace(" ", String.Empty) == id || s.Device_Type.Replace(" ", String.Empty).Contains(id) || s.Serial_No.Replace(" ", String.Empty) == id || s.Status.Replace(" ", String.Empty) == id);
                if (res != null && res.Any())
                {
                    if (res.First().Device_ID.Replace(" ", String.Empty) == id)
                    {
                        return RedirectToAction("History", "warehouse", new { id = res.First().S_No });
                    }
                    foreach (var item in res)
                    {
                        item.Device_ID = item.Device_ID.Replace(" ", String.Empty);
                        item.Device_Name = item.Device_Name.Replace(" ", String.Empty);
                        item.Device_Type = item.Device_Type.Replace(" ", String.Empty);
                        item.Processor = item.Processor.Replace(" ", String.Empty);
                        item.Remarks = item.Remarks.Replace(" ", String.Empty);
                        item.date = item.Purchase_Date.ToString("dd/MM/yyyy");
                    }
                    return View("list", res);

                }
                emp = employeeService.getEmployee().Where(s => s.Employee_ID.Replace(" ", String.Empty).Contains(id) || s.Designation.Replace(" ", String.Empty) == id || s.Emp_Name.Replace(" ", String.Empty).Contains(id) || s.Email_Id.Replace(" ", String.Empty) == id);
                if (emp != null && emp.Any())
                {
                    if (emp.First().Employee_ID.Replace(" ", String.Empty) == id)
                    {

                        return RedirectToAction("History", "Employee", new { id = emp.First().S_No });
                    }
                    foreach (var item in emp)
                    {
                        item.date = item.Date_of_joining.ToString("dd/MM/yyyy");
                    }

                    return View("list_emp", emp);
                }
            }
            return View("empty");



        }
    }
}