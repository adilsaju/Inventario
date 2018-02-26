using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario.Data;
using Inventario.ServiceContracts;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Data.SqlClient;

namespace Inventario.Services
{
    
   public class LoginService
    {
        EmployeeContext employeecontext = new EmployeeContext();

        public bool RegisterEmployee(RegistrationViewModel registrationviewmodel)
        {
            var emp = (from db in employeecontext.EmployeeDetails
                      where db.Employee_ID.Replace(" ", String.Empty) == registrationviewmodel.Emp_Id &&
                      db.Emp_Name.Replace(" ", String.Empty) == registrationviewmodel.Emp_Name
                      select db).ToList();
            if (emp.Count != 0)
            {
                Login log = new Login
                {
                    Username = registrationviewmodel.Emp_Id,
                    password = registrationviewmodel.password,
                    role = "user"
                };
                employeecontext.Logins.Add(log);
                employeecontext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public LoginViewModel getUser(string username)
        {
            LoginViewModel login = new LoginViewModel();
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Select * from Login where Username = '" + username + "'", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        login.username = reader["Username"].ToString().Replace(" ", String.Empty);
                        login.password = reader["password"].ToString().Replace(" ", String.Empty);
                        login.Role = reader["role"].ToString().Replace(" ", String.Empty);
                    }
                }
            }
            return login;
        }
       public void Logout()
        {
            FormsAuthentication.SetAuthCookie("", false);
        }
    }
}
