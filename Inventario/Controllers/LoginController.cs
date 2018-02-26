using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventario.ServiceContracts;
using System.Web.Security;
using Inventario.Services;
using Inventario.Data;
using System.Net.Mail;
using System.Net;

namespace Inventario.Controllers
{
    public class LoginController : Controller
    {
        EmployeeContext employeecontext = new EmployeeContext();
        // GET: Login
        public ActionResult Index()
        {
            return View("loginadmin1");
        }
        [HttpPost]
        public ActionResult Index(LoginViewModel loginviewmodel)
        {
           
            var data = (from db in employeecontext.EmployeeDetails
                        where db.Employee_ID == loginviewmodel.username
                        select db).FirstOrDefault();
            LoginService loginservice = new LoginService();
            LoginViewModel Loginviewmodel = new LoginViewModel();
            Loginviewmodel = loginservice.getUser(loginviewmodel.username);
            if (ModelState.IsValid)
            {
                if (Loginviewmodel.password == loginviewmodel.password)
                {
                    FormsAuthentication.SetAuthCookie(loginviewmodel.username, false);
                   
                    if (Loginviewmodel.Role == "user")
                    {
                        Session["name"] = data.Emp_Name;
                        ViewBag.name = data.Emp_Name;
                        Session["serial"] = data.S_No;
                        return RedirectToAction("Home", "Login");
                        
                    }
                    else
                    {
                        return RedirectToAction("Index1", "warehouse");
                    }
                }
                else
                {
                    ModelState.AddModelError("password", "Invalid username and/or password");
                    return View("loginadmin1");
                }
            }
            else
            {
                return View("Loginadmin1");
            }
        }
        [HttpGet]
        public ActionResult request()
        {
            ViewBag.name = Session["name"];
            return View();
        }
        [HttpPost]
        public ActionResult request(string device_id,string emp_id)
        {
            var data = (from db in employeecontext.EmployeeDetails
                        where db.Employee_ID == emp_id
                        select db).First();
            var dat = (from ed in employeecontext.warehouses
                       where ed.Device_ID == device_id
                       select ed).First();
            string device_type = dat.Device_Type.Replace(" ", String.Empty);
            string emp_name = data.Emp_Name.Replace(" ", String.Empty);
            MailMessage mail = new MailMessage();
            MailMessage o = new MailMessage("Inventario_requestdevice@outlook.com", "avnarayanan.mec@gmail.com", "Request for a new device",emp_name+ " ( Employee ID : "+emp_id + " ) has requested for a "+device_type+ "( Device ID : " +device_id+" )");
            NetworkCredential netCred = new NetworkCredential("Inventario_requestdevice@outlook.com", "Asd123.,");
            SmtpClient smtpobj = new SmtpClient();
           
                smtpobj.Host = "smtp.live.com";
                smtpobj.Port = 587;
           
                smtpobj.EnableSsl = true;
            smtpobj.Credentials = netCred;
            smtpobj.Send(o);
            ViewBag.name = Session["name"];
            return View("request");
        }

        public ActionResult Feedback(string Device_Id,string Emp_Id,string Feedback)
        {
            var data = (from db in employeecontext.EmployeeDetails
                        where db.Employee_ID == Emp_Id
                        select db).First();
            var dat = (from ed in employeecontext.warehouses
                       where ed.Device_ID == Device_Id
                       select ed).First();
            string device_type = dat.Device_Type.Replace(" ",String.Empty);
            string emp_name = data.Emp_Name.Replace(" ",String.Empty);
            MailMessage mail = new MailMessage();
            MailMessage o = new MailMessage("Inventario_requestdevice@outlook.com", "avnarayanan.mec@gmail.com", "Feedback", emp_name + " ( Employee ID : " + Emp_Id + " ) reports an update about a " + device_type + " ( Device ID : " + Device_Id + " )" + "      "+"Message : "+Feedback);
            NetworkCredential netCred = new NetworkCredential("Inventario_requestdevice@outlook.com", "Asd123.,");
            SmtpClient smtpobj = new SmtpClient();

            smtpobj.Host = "smtp.live.com";
            smtpobj.Port = 587;

            smtpobj.EnableSsl = true;
            smtpobj.Credentials = netCred;
            smtpobj.Send(o);
            return View("request");
        }

        public ActionResult Home()
        {
            ViewBag.name = Session["name"];
            int sno = Convert.ToInt32(Session["serial"]);
            UserServices user = new UserServices();
            UserViewModel userviewmodel =  user.getDetails(sno);
            
            return View(userviewmodel);
        }
            public ActionResult Logout()
        {
           
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
           
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult registration(string userid,string username,string pass,string cpass)
        {
            if (pass != cpass)
            {
                ModelState.AddModelError("error", "passwords do not match");
                return View();
            }
            else
            {
               
                LoginService login = new LoginService();
                bool res = login.RegisterEmployee(new RegistrationViewModel
                {
                    Emp_Id = userid,
                    Emp_Name = username,
                    password = pass
                });
                if (!res)
                {
                    ModelState.AddModelError("error", "Invalid Name and/or Id");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("error", "Successfully Registered");
                    return RedirectToAction("Index");
                }
            }
        }
       
    }
}