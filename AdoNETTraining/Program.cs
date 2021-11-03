using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using NLog;

namespace AdoNETTraining
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        static string ConnectionString = "Data Source=(local);Initial Catalog=IVPPolaris;Integrated Security=true;";
        static void Main(string[] args)
        {
            try
            {
                _logger.Info("Application Started!!");
                Testconnection();
                RunSelectQuery();
                RunSelectStoredProc();
                InsertEmployee();
                Console.ReadKey();
                _logger.Info("Application End!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Error Occurred during the execution!");
                Console.WriteLine("Error occured!!\n\n" + ex.Message + "\n\nCall Stack -> " + ex.StackTrace);
                throw;
            }
        }

        static void InsertEmployee()
        {
            try
            {
                SqlConnection oConnect = new SqlConnection(ConnectionString);
                _logger.Debug("Connection Opened!");
                oConnect.Open();
                Console.WriteLine(oConnect.State.ToString());
                SqlCommand ocmd = new SqlCommand();
                ocmd.Connection = oConnect;
                ocmd.CommandText = "[Internal].[p_polaris_insert_employee_details]";
                ocmd.CommandType = CommandType.StoredProcedure;

                string EmployeeName, EmployeePhone, Department, Address;
                Console.Write("\nEmployee Name: ");
                EmployeeName = Console.ReadLine();
                Console.Write("\nEmployee Phone: ");
                EmployeePhone = Console.ReadLine();
                Console.Write("\nEmployee Department: ");
                Department = Console.ReadLine();
                Console.Write("\nEmployee Address: ");
                Address = Console.ReadLine();

                ocmd.Parameters.Add(new SqlParameter("@employeeName", EmployeeName));
                ocmd.Parameters.Add(new SqlParameter("@PhoneNo", EmployeePhone));
                ocmd.Parameters.Add(new SqlParameter("@department", Department));
                ocmd.Parameters.Add(new SqlParameter("@address", Address));

                ocmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Error Occured during the DB operation!");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        static void Testconnection()
        {

            SqlConnection oConnect = new SqlConnection(ConnectionString);

            oConnect.Open();
            _logger.Debug("Connection Opened!");
            Console.WriteLine(oConnect.State.ToString());

            oConnect.Close();
        }

        static void RunSelectStoredProc()
        {

            try
            {
                SqlConnection oConnect = new SqlConnection(ConnectionString);
                oConnect.Open();
                _logger.Debug("Connection Opened!");
                Console.WriteLine(oConnect.State.ToString());
                SqlCommand ocmd = new SqlCommand();
                ocmd.Connection = oConnect;
                ocmd.CommandText = "[Internal].[sp_ivp_polaris_get_employee_details]";
                ocmd.CommandType = CommandType.StoredProcedure;
                ocmd.Parameters.Add(new SqlParameter("@empID", 1));


                SqlDataAdapter oadapter = new SqlDataAdapter(ocmd);
                DataSet ods = new DataSet();
                oadapter.Fill(ods);

                foreach (DataColumn dc in ods.Tables[0].Columns)
                {
                    Console.WriteLine(dc.ColumnName);
                }
                foreach (DataRow dr in ods.Tables[0].Rows)
                {
                    for (int i = 0; i < ods.Tables[0].Columns.Count; i++)
                    {
                        Console.Write("{0}\t\t", dr[i].ToString());
                    }
                    Console.WriteLine("");
                }

                //  Console.ReadKey();
                oConnect.Close();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Error Occured during the DB operation!");
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }


        static void RunSelectQuery()
        {
            try
            {
                SqlConnection oConnect = new SqlConnection(ConnectionString);
                oConnect.Open();
                _logger.Debug("Connection Opened!");
                Console.WriteLine(oConnect.State.ToString());
                SqlCommand ocmd = new SqlCommand("Select * from Employees;select * from DummyTest;", oConnect);
                SqlDataAdapter oadapter = new SqlDataAdapter(ocmd);
                DataSet ods = new DataSet();
                oadapter.Fill(ods);

                foreach (DataColumn dc in ods.Tables[0].Columns)
                {
                    Console.WriteLine(dc.ColumnName);
                }
                foreach (DataRow dr in ods.Tables[0].Rows)
                {
                    for (int i = 0; i < ods.Tables[0].Columns.Count; i++)
                    {
                        Console.Write("{0}\t\t", dr[i].ToString());
                    }
                    Console.WriteLine("");
                }

                //Console.ReadKey();
                oConnect.Close();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Error Occured during the DB operation!");
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

    }
}
