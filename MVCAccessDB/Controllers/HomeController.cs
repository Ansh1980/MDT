//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.OleDb;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace MVCAccessDB.Controllers
//{
//    public class HomeController : Controller
//    {
//        string databasePath = "c:\\users\\ashrivastava\\documents\\test.accdb";
//        //DataTable userTable = new DataTable();
//        OleDbDataAdapter adapter;
//        OleDbConnection connection;
//        OleDbCommand command;
//        OleDbCommandBuilder builder;
//        //DataSet ds;
//        //DataSet tempDs;
//        public string DatabaseName = null;
//        string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=";


//        public ActionResult Index()
//        {

//            using (OleDbConnection dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\ashrivastava\\documents\\test.accdb"))
//            {
//                using (OleDbCommand cmd2 = dbc.CreateCommand())
//                {
//                    cmd2.CommandText = "select * from [user]";
//                    // cmd.CommandType = System.Data.CommandType.StoredProcedure;

//                    //Now lets add the values
//                    OleDbParameter[] values = new OleDbParameter[] {
//                //new OleDbParameter("@name", model.Name?? DBNull.Value.ToString()),
//                //new OleDbParameter("@color", model.Color?? DBNull.Value.ToString()),
//                //new OleDbParameter("@taste", model.Taste?? DBNull.Value.ToString())
//            };

//                    try
//                    {
//                        cmd2.Parameters.AddRange(values);
//                        //open Connection
//                        dbc.Open();
//                        //Execute our create Query
//                        cmd2.ExecuteNonQuery();
//                        //close Connection
//                        dbc.Close();
//                    }
//                    finally
//                    {
//                        //close connection if something went wrong
//                        dbc.Close();
//                    }
//                }
//                /////////////////////////////////////
//                string getTConnection = connectionString + databasePath; ;
//                OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\ashrivastava\\documents\\test.accdb");

//                myConnection.Open();

//                //var datT = myConnection.GetSchema("user");
//                OleDbCommand cmd = new OleDbCommand("Select * FROM [user]", myConnection);
//                adapter = new OleDbDataAdapter(cmd);
//                builder = new OleDbCommandBuilder(adapter);
//                DataSet ds = new DataSet("MainDataSet");
//                // tempDs = new DataSet("TempDataSet");

//                // connection.Open();
//                //adapter.Fill(tempDs);
//                //tempDs.Clear();
//                //tempDs.Dispose();
//                adapter.Fill(ds);
//                //  userTable = ds.Tables[ddTables.Text];

//                return View();
//            }
//        }

//        public ActionResult About()
//        {
           
//                OleDbConnection conn = new OleDbConnection();
//                conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\ashrivastava\\documents\\test.accdb";

//                String Firstname = "Ansh";
//                String lastname = "Shri";

//                OleDbCommand cmd = new OleDbCommand("INSERT into [user] (Firstname, lastname) Values(@Firstname, @lastname)");
//                cmd.Connection = conn;

//                conn.Open();

//                if (conn.State == ConnectionState.Open)
//                {
//                    cmd.Parameters.Add("@Firstname", OleDbType.VarChar).Value = Firstname;
//                    cmd.Parameters.Add("@lastname", OleDbType.VarChar).Value = lastname;

//                    try
//                    {
//                        cmd.ExecuteNonQuery();
//                      //  MessageBox.Show("Data Added");
//                        conn.Close();
//                    }
//                    catch (OleDbException ex)
//                    {
//                    string str = ex.ToString();
//                        conn.Close();
//                    }
//                }
//                else
//                {
//                  //  MessageBox.Show("Connection Failed");
//                }
            
        

//            return View();
//        }

//        public ActionResult Contact()
//        {
//            ViewBag.Message = "Your contact page.";

//            return View();
//        }
//    }
//}