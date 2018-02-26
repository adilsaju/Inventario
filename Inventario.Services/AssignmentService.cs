using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario.Data;
using System.Configuration;
using System.Data.SqlClient;
using Inventario.ServiceContracts;

namespace Inventario.Services
{
   public class AssignmentService
    {
        EmployeeContext employeecontext = new EmployeeContext();

        public List<string> getEmployeeid()
        {
            List<string> id = new List<string>();
            var det = (from db in employeecontext.EmployeeDetails
                      select db);
            foreach(var item in det)
            {
                id.Add(item.Employee_ID);
            }
            return id;

        }
        public List<AssignmentViewModel> getAssign()
        {
            List<AssignmentViewModel> assignmentviewmodel = new List<AssignmentViewModel>();
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("getAssign", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        assignmentviewmodel.Add(new AssignmentViewModel()
                        {
                            S_No = Convert.ToInt32(reader["S_No"]),
                            employee_name = reader["Emp_Name"].ToString(),
                            device_id = reader["Device_ID"].ToString(),
                            Serial_No = reader["Serial_No"].ToString(),
                            Device_type = reader["Device_Type"].ToString(),
                            Device_Name = reader["Device_Name"].ToString(),
                            emp_id = reader["Emp_Id"].ToString()
                        });
                    }
                }
            }
            return assignmentviewmodel;
        }
        public bool AddAssign(AssignViewModel assignviewmodel)
        {
            string empname = "";
            int count=0;
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                
                SqlCommand cmd1 = new SqlCommand("select * from EmployeeDetails where Employee_ID = '" + assignviewmodel.Emp_Id + "'", con);
                SqlCommand cmd = new SqlCommand("update warehouse set Status= 'Assigned' where Device_ID= '"+assignviewmodel.Device_Id+"'", con);
               
                con.Open();
                using (SqlDataReader reader = cmd1.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        count++;
                        empname = reader["Emp_Name"].ToString().Replace(" ", String.Empty);
                    }
                }
                SqlCommand cmd2 = new SqlCommand("insert into History values('" + assignviewmodel.Device_Id + "','" + assignviewmodel.Emp_Id + "','"+empname+"',GETDATE(),'01/01/1950')", con);
                if (count == 1)
                    {
                        cmd.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                        Assign assign = new Assign();
                        assign.Emp_Id = assignviewmodel.Emp_Id;
                        assign.Device_Id = assignviewmodel.Device_Id;
                        employeecontext.Assigns.Add(assign);
                        employeecontext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                
                    
            }
               

            
        }
        public List<string> get_id()
        {
            List<string> id = new List<string>();
            var data = (from ed in employeecontext.EmployeeDetails
                        select ed).ToList();
            foreach(var item in data)
            {
                id.Add(item.Employee_ID);
            }
            return id;
        }

        public List<string> getunassigned()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                List<string> serial = new List<string>();
                SqlCommand cmd = new SqlCommand("select Device_ID from warehouse where Status = 'Unassigned' ", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        serial.Add(reader["Device_ID"].ToString());
                    }

                }

                return serial;
            }
                
        }
        public bool Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string serial = "";
                int sno = 0;
                
                
                SqlCommand cmd2 = new SqlCommand("select Device_Id from Assign where S_No = " + id,con);
                con.Open();
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        serial = reader["Device_Id"].ToString().Replace(" ", String.Empty);
                    }

                }
                SqlCommand cmd = new SqlCommand("update warehouse set Status= 'Unassigned' where Device_ID= '" + serial+"'", con);
                SqlCommand cmd3 = new SqlCommand("update History set Date_unassign = GETDATE() where Device_Id = '"+serial+"' AND YEAR(GETDATE())-YEAR(Date_assign) > 50", con);
                SqlCommand cmd1 = new SqlCommand("delete from Assign where S_No = " + id, con);
                cmd.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
                int res  = cmd1.ExecuteNonQuery();
                return true;
            }
        }
        public AssignViewModel getDetails(int id)
        {
            int sno = 0;
            string deviceid = "";
            string empid = "";
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
               
                SqlCommand cmd = new SqlCommand("Select * from Assign where S_No = " + id, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        deviceid = reader["Device_Id"].ToString().Replace(" ", String.Empty);
                        empid = reader["Emp_Id"].ToString().Replace(" ", String.Empty);
                        sno = Convert.ToInt32(reader["S_No"]);
                    }

                }
                
            }
            AssignViewModel assign = new AssignViewModel
            {
                Emp_Id = empid,
                Device_Id = deviceid
            };
            return assign;
        }
        public int Edit(AssignViewModel assignviewmodel)
        {
            int serial = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("Select S_No from warehouse where Device_ID = '" + assignviewmodel.Device_Id+"'", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        serial = Convert.ToInt32(reader["S_No"]);
                    }
                }
            }
            return serial;
           
        }
    }
}
