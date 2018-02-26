using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario.Data;
using Inventario.ServiceContracts;
using System.Data.SqlClient;
using System.Configuration;


namespace Inventario.Services
{
    public class warehouseService
    {

        EmployeeContext employeecontext = new EmployeeContext();

        public List<warehouseViewModel> getLaptop()
        {
            var data = (from ed in employeecontext.warehouses
                       where ed.Device_Type == "laptop"
                        select new warehouseViewModel
                        {
                            S_No = ed.S_No,
                            Device_ID = ed.Device_ID,
                            Serial_No = ed.Serial_No,
                            Device_Name = ed.Device_Name,
                            Device_Type = ed.Device_Type,
                            Processor = ed.Processor,
                            Remarks = ed.Remarks,
                            Purchase_Date = ed.Purchase_Date,
                            Status = ed.Status
                        }).ToList();
            return data;
        }

        public List<warehouseViewModel> getdesktop()
        {
            var data = (from ed in employeecontext.warehouses
                        where ed.Device_Type == "desktop"
                        select new warehouseViewModel
                        {
                            S_No = ed.S_No,
                            Device_ID = ed.Device_ID,
                            Serial_No = ed.Serial_No,
                            Device_Name = ed.Device_Name,
                            Device_Type = ed.Device_Type,
                            Processor = ed.Processor,
                            Remarks = ed.Remarks,
                            Purchase_Date = ed.Purchase_Date,
                            Status = ed.Status
                        }).ToList();
            return data;
        }

        public List<warehouseViewModel> getAsset()
        {
            var data = (from ed in employeecontext.warehouses
                        select new warehouseViewModel
                        {
                            S_No = ed.S_No,
                            Device_ID = ed.Device_ID,
                            Serial_No = ed.Serial_No,
                            Device_Name = ed.Device_Name,
                            Device_Type = ed.Device_Type,
                            Processor = ed.Processor,
                            Remarks = ed.Remarks,
                            Purchase_Date = ed.Purchase_Date,
                            Status = ed.Status
                        }).ToList();
            return data;
        }

        public bool getdevice_id(string deviceid)
        {
            var data = (from ed in employeecontext.warehouses
                        where ed.Device_ID.Replace(" ", String.Empty) == deviceid
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

        public bool setwarehouse(warehouseViewModel warehouse)
        {
           
                warehouse asset = new warehouse();
                asset.Device_ID = warehouse.Device_ID;
                asset.Device_Name = warehouse.Device_Name;
                asset.Device_Type = warehouse.Device_Type;
                asset.Processor = warehouse.Processor;
                asset.Purchase_Date = warehouse.Purchase_Date;
                asset.Remarks = warehouse.Remarks;
                asset.Serial_No = warehouse.Serial_No;
                asset.Status = warehouse.Status;
                employeecontext.warehouses.Add(asset);
                employeecontext.SaveChanges();
                return true;
          
        }
        public string Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string device = "";
                string devype = "";
                SqlCommand cmd1 = new SqlCommand("select Device_ID,Device_Type from warehouse where S_No =  " + id, con);
                
                con.Open();
                using (SqlDataReader reader = cmd1.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        devype = reader["Device_Type"].ToString().Replace(" ", String.Empty);
                        device = reader["Device_ID"].ToString().Replace(" ", String.Empty);
                    }
                }
                SqlCommand cmd = new SqlCommand("delete from warehouse where S_No = "+id, con);
                SqlCommand cmd2 = new SqlCommand("Delete from Assign where Device_Id = '" + device + "'", con);
                int res = cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                return devype;
            }

        }

        public int editasset(warehouseViewModel warehouseviewmodel)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("updateWarehouse", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter paramS_No = new SqlParameter();
                paramS_No.ParameterName = "@serial";
                paramS_No.Value = warehouseviewmodel.S_No;
                cmd.Parameters.Add(paramS_No);

                SqlParameter paramId = new SqlParameter();
                paramId.ParameterName = "@device_name";
                paramId.Value = warehouseviewmodel.Device_Name;
                cmd.Parameters.Add(paramId);

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@device_type";
                paramName.Value = warehouseviewmodel.Device_Type;
                cmd.Parameters.Add(paramName);

                SqlParameter paramdes = new SqlParameter();
                paramdes.ParameterName = "@pro";
                paramdes.Value = warehouseviewmodel.Processor;
                cmd.Parameters.Add(paramdes);

                SqlParameter paramemail = new SqlParameter();
                paramemail.ParameterName = "@rem";
                paramemail.Value = warehouseviewmodel.Remarks;
                cmd.Parameters.Add(paramemail);


                SqlParameter paramstat = new SqlParameter();
                paramstat.ParameterName = "@stat";
                paramstat.Value = warehouseviewmodel.Status;
                cmd.Parameters.Add(paramstat);

                con.Open();
                int res = cmd.ExecuteNonQuery();
                return res;
            }

        }
        public List<HistoryViewModel> getHistory(int id)
        {
            string deviceid = "";
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Select Device_ID from warehouse where S_No = " + id, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        deviceid = reader["Device_ID"].ToString().Replace(" ", String.Empty);
                    }
                }
               
            }
            
            List<HistoryViewModel> history = (from ed in employeecontext.Histories
                                              where ed.Device_Id == deviceid 
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

       

    }
}
