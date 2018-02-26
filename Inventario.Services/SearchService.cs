using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario.ServiceContracts;
using System.Configuration;
using System.Data.SqlClient;
using Inventario.Services;

namespace Inventario.Services
{
    public class SearchService
    {
        public List<HistoryViewModel> getHistory(string deviceid, int type)
        {
            warehouseService warehouseservice = new warehouseService();
            int serial = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (type == 1 || type == 2)
                {
                    SqlCommand cmd = new SqlCommand("Select Serial_No from warehouse where Device_ID = '" + deviceid + "'", con);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            serial = Convert.ToInt32(reader["Serial_No"]);
                        }
                    }

                }
                else
                {
                    SqlCommand cmd = new SqlCommand("Select S_No from EmployeeDetails where Employee_ID = '" + deviceid + "'", con);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            serial = Convert.ToInt32(reader["S_No"]);
                        }
                    }
                }
            }
            
            List<HistoryViewModel> history = warehouseservice.getHistory(serial);
            return (history);



        }
    }
}

