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
            try
            {
                // OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\anshi\\documents\\mdtaccessdb.accdb");
                OleDbConnection myConnection = new OleDbConnection();
                myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

                myConnection.Open();

                OleDbCommand cmd = new OleDbCommand("Select * FROM [user]", myConnection);
                adapter = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet("MainDataSet");
               
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
            }
            catch
            {
                return View("Error");
            }
           
        }

        public ActionResult Create()
        {
            try { 
                    UserModel model = new UserModel();
                    return View(model);
                }
            catch
            {
                return View("Error");
            }
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