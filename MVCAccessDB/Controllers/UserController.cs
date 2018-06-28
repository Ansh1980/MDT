using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using MVCAccessDB.Models;

namespace MVCAccessDB.Controllers
{
    public class UserController : Controller
    {
        OleDbDataAdapter adapter;
        // GET: User
        public ActionResult Index()
        {
            ///////test
            //using (OleDbConnection dbc = new OleDbConnection(ConfigurationManager.AppSettings["connectionString"]))
            //{
            //    using (OleDbCommand cmd = dbc.CreateCommand())
            //    {
            //        //cmd.CommandText = "select * from [user]";
            //        //adapter = new OleDbDataAdapter(cmd);
            //        //  builder = new OleDbCommandBuilder(adapter);
            //        DataSet ds = new DataSet("UserDataSet");
            //        // tempDs = new DataSet("TempDataSet");

            //        // connection.Open();
            //        //adapter.Fill(tempDs);
            //        //tempDs.Clear();
            //        //tempDs.Dispose();
            //        adapter.Fill(ds);
            // OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\anshi\\documents\\mdtaccessdb.accdb");
            OleDbConnection myConnection = new OleDbConnection();
            myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

            myConnection.Open();
           
            OleDbCommand cmd = new OleDbCommand("Select * FROM [user]" , myConnection);
            adapter = new OleDbDataAdapter(cmd);
           // builder = new OleDbCommandBuilder(adapter);
            DataSet ds = new DataSet("MainDataSet");
            // tempDs = new DataSet("TempDataSet");

            // connection.Open();
            //adapter.Fill(tempDs);
            //tempDs.Clear();
            //tempDs.Dispose();
            adapter.Fill(ds);
            IList<UserModel> users = new List<UserModel>();
            var userlist = ds.Tables[0];
            // var userList = db.Users.ToList();
            foreach (DataRow user in userlist.Rows)
            {
                users.Add(new UserModel
                {
                    FirstName = user["FirstName"].ToString(),
                    Lastname = user["lastname"].ToString(),
                    UserId = Convert.ToInt32(user[0].ToString()),
                    IsActive = Convert.ToBoolean(user["IsAdmin"].ToString())

                });
            }
            myConnection.Close();
                    return View(users);
               // }
           // }


            //var userList = db.Users.ToList();
            //foreach (var user in userList)
            //{
            //    users.Add(new UserModel
            //    {
            //        FirstName = user.FirstName,
            //        Lastname = user.Lastname,
            //        UserId = user.UserId,
            //        UserName = user.UserName

            //    });
            //}

            //return View(users);
           
        }

        public ActionResult Create()
        {
            UserModel model = new UserModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UserModel model)//FormCollection collection)
        {
            try
            {
                OleDbConnection conn = new OleDbConnection();

                conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
                    //@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\anshi\\documents\\mdtaccessdb.accdb";

                //String Firstname = "Ansh2";
                //String lastname = "Shri2";

                OleDbCommand cmd = new OleDbCommand("INSERT into [user] (Firstname, LastName, IsAdmin) Values(@FirstName, @LastName, @IsAdmin)");
                cmd.Connection = conn;

                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    cmd.Parameters.Add("@FirstName", OleDbType.VarChar).Value = model.FirstName;
                    cmd.Parameters.Add("@LastName", OleDbType.VarChar).Value = model.Lastname;
                    cmd.Parameters.Add("@IsAdmin", OleDbType.Boolean).Value = model.IsAdmin;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        //  MessageBox.Show("Data Added");
                        conn.Close();
                    }
                    catch (OleDbException ex)
                    {
                        string str = ex.ToString();
                        conn.Close();
                    }
                }
                else
                {
                    return View("Error");
                }
                
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}