using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventario.ServiceContracts;
using Inventario.Services;
using Inventario.Filters;

namespace Inventario.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        EmployeeService employeeService = new EmployeeService();
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Index()
        {
            List<EmployeeViewModel> employeeviewmodel = new List<EmployeeViewModel>();
            employeeviewmodel = employeeService.getEmployee();
           foreach(var item in employeeviewmodel)
            {
                item.date = item.Date_of_joining.ToString("dd/MM/yyyy");
            }
            return View(employeeviewmodel);
        }
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult saveEmployee(EmployeeViewModel employeeViewModel)
        {
            EmployeeService employeeService = new EmployeeService();
             bool result = employeeService.setEmployee(employeeViewModel);

            if(result)
            {
                return View();
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Edit(int id)
        {

            var res = employeeService.getEmployee().Where(s => s.S_No == id).FirstOrDefault();
            res.date = res.Date_of_joining.ToString("dd/MM/yyyy");
            return PartialView("_EditModal", res);
        }
        [HttpPost]
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Edit(EmployeeViewModel employeeviewmodel)
        {
            if (!employeeService.assignedemail(employeeviewmodel.Email_Id))
            {
                ModelState.AddModelError("Email_Id", "Email taken");
                return View();
            }
            else
            {
                int res = employeeService.updateEmployee(employeeviewmodel);
                return RedirectToAction("Index");
            }
        }
       
        [HttpPost]
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Delete(int id)
        {
            int res = employeeService.deleteEmployee(id);
            if (res == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult AddModal()
        {
            return PartialView("_Addemp");
        }

        [HttpGet]
        [AdminAuthorize(new string[] { "Admin" })]

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Create(EmployeeViewModel employeeviewmodel)
        {
            if (ModelState.IsValid)
            {
                if (!employeeService.assignedemail(employeeviewmodel.Email_Id))
                {
                    ModelState.AddModelError("Email_Id", "Email taken");
                    return View();
                }
                else if(!employeeService.assigned_id(employeeviewmodel.Employee_ID))
                {
                    ModelState.AddModelError("Employee_ID", "This Employee Id is already registered ");
                    return View();
                }
                else
                {
                    employeeService.setEmployee(employeeviewmodel);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View();
            }
            
        }
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult History(int id)
        {
            var res = employeeService.getEmployee().Where(s => s.S_No == id).FirstOrDefault();
            EmployeeViewModel employee = new EmployeeViewModel();
            res.date = res.Date_of_joining.ToString("dd/MM/yyyy");
            List<HistoryViewModel> history = employeeService.getHistory(id);


            if (history.Count == 0)
            {
                HistoryViewModel his = new HistoryViewModel();
                his.EmployeeViewModel = res;
                history.Add(his);
            }
            else
            {
                history[0].EmployeeViewModel = res;

                foreach (var item in history)
                {
                    item.assign = item.Date_assign.ToString("dd/MM/yyyy");
                    if (item.Date_assign.Year - item.Date_unassign.Year < 50)
                        item.unassign = item.Date_unassign.ToString("dd/MM/yyyy");
                    else
                        item.unassign = "N/A";
                }
            }
            return View(history);
        }
       

    }
}