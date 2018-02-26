using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventario.Services;
using Inventario.ServiceContracts;
using Inventario.Filters;
using Inventario.Data;

namespace Inventario.Controllers
{
    public class warehouseController : Controller
    {
        // GET: warehouse
        AssignmentService assignmentservice = new AssignmentService();
        warehouseService warehouseservice = new warehouseService();
        EmployeeContext employeecontext = new EmployeeContext();
        [AdminAuthorize( new string[] { "Admin", "user" })]
        public ActionResult Index1()
        {
            
            IEnumerable < warehouseViewModel > warehouseviewmodel;
            bool user = HttpContext.User.IsInRole("Admin");
            ViewBag.user = user;
            if (user)
            {
                warehouseviewmodel = warehouseservice.getdesktop();
            }
            else
            {
                ViewBag.name = Session["name"];
                warehouseviewmodel = warehouseservice.getdesktop().Where(x => x.Status == "Unassigned");
            }
            foreach (var item in warehouseviewmodel)
            {
                item.date = item.Purchase_Date.ToString("dd/MM/yyyy");
            }
            return View("desktop", warehouseviewmodel);
        }
        [AdminAuthorize(new string[] { "Admin","user"})]
        public ActionResult Index2()
        {
           
            bool user = HttpContext.User.IsInRole("Admin");
            ViewBag.user = user;
            IEnumerable<warehouseViewModel> warehouseviewmodel;
            if (user)
            {
                
              warehouseviewmodel = warehouseservice.getLaptop();
            }
            else
            {
                ViewBag.name = Session["name"];
                warehouseviewmodel = warehouseservice.getLaptop().Where(x => x.Status == "Unassigned");
            }
            foreach(var item in warehouseviewmodel)
            {
                item.date = item.Purchase_Date.ToString("dd/MM/yyyy");
            }
            return View("laptop", warehouseviewmodel);
        }
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult saveAsset(warehouseViewModel warehouse)
        {

            bool result = warehouseservice.setwarehouse(warehouse);

            if (result)
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
        public ActionResult Add(int id)
        {
            if(id ==1 )
            {
                ViewBag.type = "Laptop";
            }
            else
            {
                ViewBag.type = "Desktop";
            }
            
            return View();
        }
        [HttpPost]
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Add(warehouseViewModel warehouseviewmodel)
        {

            if (ModelState.IsValid)
            {
                if (warehouseservice.getdevice_id(warehouseviewmodel.Device_ID))
                {
                    bool res = warehouseservice.setwarehouse(warehouseviewmodel);
                    if (res)
                    {
                        if (warehouseviewmodel.Device_Type.Replace(" ", String.Empty) == "Laptop")
                            return RedirectToAction("Index2");
                        else if (warehouseviewmodel.Device_Type.Replace(" ", String.Empty) == "Desktop")
                            return RedirectToAction("Index1");
                        else
                        {
                            return RedirectToAction("Index1");
                        }
                    }
                    else
                    {
                        var errorList = (from item in ModelState
                                         where item.Value.Errors.Any()
                                         select item.Value.Errors[0].ErrorMessage).ToList();
                        return Json(errorList, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ModelState.AddModelError("Device_ID", "This Device Id is already inserted ");
                    return RedirectToAction("Index1");
                }
            }
            else
            {
               
                return RedirectToAction("Index1");
            }
        }
        [HttpGet]
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Edit(int id)
        {
            var res = warehouseservice.getAsset().Where(s => s.S_No == id).FirstOrDefault();
            res.Device_ID = res.Device_ID.Replace(" ", String.Empty);
            res.Device_Name = res.Device_Name.Replace(" ", String.Empty);
            res.Device_Type=res.Device_Type.Replace(" ", String.Empty);
            res.Processor = res.Processor.Replace(" ", String.Empty);
            res.Remarks = res.Remarks.Replace(" ", String.Empty);
            res.date = res.Purchase_Date.ToString("dd/MM/yyyy");
            return PartialView("_EditModal",res);

        }
        [HttpPost]
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Edit(warehouseViewModel warehouseviewmodel)
        {
            
                int res = warehouseservice.editasset(warehouseviewmodel);

                if (res == 1)
                {
                    if (warehouseviewmodel.Device_Type == "Laptop")
                        return RedirectToAction("Index2");
                    else if (warehouseviewmodel.Device_Type == "Desktop")
                        return RedirectToAction("Index1");

                }
            
           
                return View();
        }
        public ActionResult EditModal()
        {
            return PartialView("_EditModal");
        }

        public ActionResult DisplayModal(int id)
        {
            if (id == 2)
            {
                ViewBag.type = "Laptop";
            }
            else
            {
                ViewBag.type = "Desktop";
            }
            return PartialView("_AddModal");
        }
      
        [HttpPost]
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult Delete(int id)
        {
            string res = warehouseservice.Delete(id);
            if(res == "Laptop")
            return RedirectToAction("Index2", "warehouse");
            else
            {
                return RedirectToAction("Index1", "warehouse");
            }
            
        }
        [AdminAuthorize(new string[] { "Admin" })]
        public ActionResult History(int id)
        {
            
            var res = warehouseservice.getAsset().Where(s => s.S_No == id).FirstOrDefault();
            res.Device_ID = res.Device_ID.Replace(" ", String.Empty);
            res.Device_Name = res.Device_Name.Replace(" ", String.Empty);
            res.Device_Type = res.Device_Type.Replace(" ", String.Empty);
            res.Processor = res.Processor.Replace(" ", String.Empty);
            res.Remarks = res.Remarks.Replace(" ", String.Empty);
            res.date = res.Purchase_Date.ToString("dd/MM/yyyy");

           
            List<HistoryViewModel> history = warehouseservice.getHistory(id);
            if (history.Count == 0)
            {
                HistoryViewModel his = new HistoryViewModel();
                his.warehouseViewModel = res;
                history.Add(his);

            }
            else
            {
                history[0].warehouseViewModel = res;
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