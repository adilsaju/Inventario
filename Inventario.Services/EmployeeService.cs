using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario.Data;
using Inventario.ServiceContracts;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.EntityClient;

namespace Inventario.Services
{
    public class EmployeeService
    {
        EmployeeContext employeecontext = new EmployeeContext();
        public List<EmployeeViewModel> getEmployee()
        {
            var data = (from ed in employeecontext.EmployeeDetails
                        select new EmployeeViewModel
                        {
                            S_No = ed.S_No,
                            Emp_Name = ed.Emp_Name,
                            Email_Id = ed.Email_Id,
                            Designation = ed.Designation,
                            Date_of_joining = ed.Date_of_joining,
                            Employee_ID = ed.Employee_ID,
                        }).ToList();
            return data;
        }
        
       public bool setEmployee(EmployeeViewModel employeeViewModel)
        {
            try
            {
                EmployeeDetail employeeDetail = new EmployeeDetail();
                employeeDetail.S_No = employeeViewModel.S_No;
                employeeDetail.Employee_ID = employeeViewModel.Employee_ID;
                employeeDetail.Date_of_joining = employeeViewModel.Date_of_joining;
                employeeDetail.Designation = employeeViewModel.Designation;
                employeeDetail.Email_Id = employeeViewModel.Email_Id;
                employeeDetail.Emp_Name = employeeViewModel.Emp_Name;
                employeecontext.EmployeeDetails.Add(employeeDetail);
                employeecontext.SaveChanges();
                return true;
            }
            catch(Exception )
            {
                return false;
            }
        }
        public int deleteEmployee(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string empid = "";
                List<string> serial = new List<string>();
                SqlCommand cmd = new SqlCommand("deleteEmployee", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlCommand cmd3 = new SqlCommand("select Employee_ID from EmployeeDetails where S_No = " + id,con);
                
                
                SqlParameter paramS_No = new SqlParameter();
                paramS_No.ParameterName = "@S_No";
                paramS_No.Value = id;
                cmd.Parameters.Add(paramS_No);
                con.Open();
                using (SqlDataReader reader = cmd3.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        empid = reader["Employee_ID"].ToString().Replace(" ",String.Empty);
                    }
                }
                SqlCommand cmd1 = new SqlCommand("Delete from Assign where Emp_Id = '" + empid+"'", con);
                SqlCommand cmd2 = new SqlCommand("Select Device_Id from Assign where Emp_Id = '" + empid + "'", con);
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        serial.Add(reader["Device_Id"].ToString().Replace(" ",String.Empty));
                    }
                }
                foreach(var item in serial)
                {
                    (new SqlCommand("update warehouse set Status = 'unassigned' where Device_Id = '" + item+"'",con)).ExecuteNonQuery() ;
                }
                        cmd1.ExecuteNonQuery();
                int res = cmd.ExecuteNonQuery();
                return res;
            }
            }

        public bool assignedemail(string email)
        {
            var data = (from ed in employeecontext.EmployeeDetails
                        where ed.Email_Id.Replace(" ",String.Empty) == email
                        select ed).FirstOrDefault();
            if(data == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool assigned_id(string id)
        {
            var data = (from db in employeecontext.EmployeeDetails
                        where db.Employee_ID.Replace(" ", String.Empty) == id
                        select db).FirstOrDefault();
            if (data != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public List<HistoryViewModel> getHistory(int id)
        {
            string empid = "";
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Select Employee_ID from EmployeeDetails where S_No = " + id, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        empid = reader["Employee_ID"].ToString().Replace(" ", String.Empty);
                    }
                }

            }

            List<HistoryViewModel> history = (from ed in employeecontext.Histories
                                              where ed.Emp_Id == empid
                                              orderby ed.Date_assign descending
                                              select new HistoryViewModel
                                              {
                                                  Emp_Id = ed.Emp_Id,
                                                  Device_Id = ed.Device_Id,
                                                  S_No = ed.S_No,
                                                  Date_assign = ed.Date_assign,
                                                  Date_unassign = ed.Date_unassign,
                                                  Emp_Name = ed.Emp_name

                                              }).ToList();

            return history;

        }
        public int updateEmployee(EmployeeViewModel employeeViewModel)
        {
            int res = 0;
            


                    string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("saveEmployee", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter paramS_No = new SqlParameter();
                        paramS_No.ParameterName = "@S_No";
                        paramS_No.Value = employeeViewModel.S_No;
                        cmd.Parameters.Add(paramS_No);

                        SqlParameter paramId = new SqlParameter();
                        paramId.ParameterName = "@emp_id";
                        paramId.Value = employeeViewModel.Employee_ID;
                        cmd.Parameters.Add(paramId);

                        SqlParameter paramName = new SqlParameter();
                        paramName.ParameterName = "@emp_name";
                        paramName.Value = employeeViewModel.Emp_Name;
                        cmd.Parameters.Add(paramName);

                        SqlParameter paramdes = new SqlParameter();
                        paramdes.ParameterName = "@designation";
                        paramdes.Value = employeeViewModel.Designation;
                        cmd.Parameters.Add(paramdes);

                        SqlParameter paramemail = new SqlParameter();
                        paramemail.ParameterName = "@email_id";
                        paramemail.Value = employeeViewModel.Email_Id;
                        cmd.Parameters.Add(paramemail);



                        con.Open();
                        res = cmd.ExecuteNonQuery();
                    }

                    return res;

                
               
            
          
           
        }
    }
}
