using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventario.Services;
using Inventario.ServiceContracts;
using Inventario.Data;
using Inventario.Filters;

namespace Inventario.Controllers
{
    public class AssignmentController : Controller
    {
        
        // GET: Assignment
        AssignmentService assignmentservie = new AssignmentService();
       
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Index()
        {


            return View(assignmentservie.getAssign());
        }


        public class Drop
        {
            public string id { get; set; }
            public string value { get; set; }
        }
        public JsonResult getID()
        {

            return Json(assignmentservie.get_id());
        }

        [HttpGet]
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Create()
        {
            ViewBag.id = assignmentservie.getEmployeeid();

            List<Drop> a = new List<Controllers.AssignmentController.Drop>();
            List<string> serial = assignmentservie.getunassigned();
            List<string> empid = assignmentservie.get_id();
            List<Drop> b = new List<Controllers.AssignmentController.Drop>();
            b.Add(new Drop
            {
                id=" ",
                value=" "
            });
            foreach(var item in empid)
            {
                b.Add(new Drop
                {
                    id = item,
                    value = item
                });
            }
            if (b.Count == 0)
            {
                b.Add(new Drop
                {
                    id = "No Employee",
                    value = "No Employee"
                });
            }
            ViewBag.empid = new SelectList(b, "id", "value");
            a.Add(new Drop
            {
                id = " ",
                value = " "
            });
           
            foreach (var item in serial)
            {
                a.Add(new Drop()
                {
                    id = item.Replace(" ", String.Empty),
                    value = item.Replace(" ", String.Empty)
                });
            }
            if (a.Count > 0)
                ViewBag.serial = new SelectList(a, "id", "value");
            else
            {
                a.Add(new Drop()
                {
                    id = "Not Available",
                    value = "Not Available"
                });
                ViewBag.serial = new SelectList(a, "id", "value");
            }
            return PartialView("_Assignpartial");
        }

       

        [HttpPost]
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Create(AssignViewModel assignviewmodel)
        {
            assignviewmodel.Emp_Id = assignviewmodel.Emp_Id.Replace(" ", String.Empty);
            assignviewmodel.Device_Id = assignviewmodel.Device_Id.Replace(" ", String.Empty);
            assignmentservie.AddAssign(assignviewmodel);
                return RedirectToAction("Index");
          
            

        }

        [HttpPost]
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Delete(int id)
        {
             assignmentservie.Delete(id);
            return RedirectToAction("Index", assignmentservie.getAssign());
        }

        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Edit(int id)
        {
            Session["serial"] = id;
            EmployeeContext employeecontext = new EmployeeContext();
            List<Drop> a = new List<Controllers.AssignmentController.Drop>();
            var select = employeecontext.Assigns.Where(x => x.S_No == id).FirstOrDefault();
            var sel = employeecontext.warehouses.Where(x => x.Device_ID == select.Device_Id).FirstOrDefault();
            
                    
            List<string> serial = assignmentservie.getunassigned();
            serial.Add(sel.Device_ID.Replace(" ", String.Empty));
            foreach (var item in serial)
            {
                a.Add(new Drop()
                {
                    id = item.Replace(" ", String.Empty),
                    value = item.Replace(" ", String.Empty)
                });
            }
            if (a.Count > 0)
                ViewBag.serial = new SelectList(a, "id", "value");
            else
            {
                a.Add(new Drop()
                {
                    id = "No Unassigned Devices",
                    value = "No Unassigned Devices"
                });
                ViewBag.serial = new SelectList(a, "id", "value");
            }
           
            return PartialView("_EditPartial",assignmentservie.getDetails(id));
        }
        [AdminAuthorize(new string[] { "Admin" })]

        [HttpPost]
        public ActionResult Edit(AssignViewModel assign)
        {

            int serial = (int)Session["serial"];


            if (assignmentservie.Delete(serial) )
            {
                assignmentservie.AddAssign(assign);
                
                return RedirectToAction("Index");
            }
            else
            {
                
                ModelState.AddModelError("Emp_Id", "Invalid Employee Id");
                return RedirectToAction("Index");
            }
        }

       }
}