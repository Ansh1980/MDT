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
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");

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
                        Lastname = user["Lastname"].ToString(),
                        UserId = Convert.ToInt32(user["UserId"].ToString()),
                        IsAdmin = Convert.ToBoolean(user["IsAdmin"].ToString()),
                        UserName = user["Username"].ToString(),
                        IsActive = Convert.ToBoolean(user["IsActive"].ToString()),

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
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");

                OleDbConnection conn = new OleDbConnection();

                conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
             
                OleDbCommand cmd = new OleDbCommand("INSERT into [user] (Firstname, LastName,IsActive, IsAdmin, DateCreated, Username) Values(@FirstName, @LastName,@IsActive, @IsAdmin, @DateCreated, @Username)");
                cmd.Connection = conn;

                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    cmd.Parameters.Add("@FirstName", OleDbType.VarChar).Value = model.FirstName;
                    cmd.Parameters.Add("@LastName", OleDbType.VarChar).Value = model.Lastname;
                    cmd.Parameters.Add("@IsActive", OleDbType.Boolean).Value = true;
                    cmd.Parameters.Add("@IsAdmin", OleDbType.Boolean).Value = model.IsAdmin;
                    cmd.Parameters.Add("@DateCreated", OleDbType.Date).Value = DateTime.Now;
                    cmd.Parameters.Add("@Username", OleDbType.VarChar).Value = model.UserName.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
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

        // GET: PatientInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");

                UserModel model = new UserModel();
                if (id != null && id > 0)
                {

                    OleDbConnection myConnection = new OleDbConnection();

                    myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
                    myConnection.Open();

                    //var datT = myConnection.GetSchema("user");
                    OleDbCommand cmd = new OleDbCommand("Select * FROM [User] where UserId = " + id, myConnection);
                    OleDbDataAdapter adapter;
                    adapter = new OleDbDataAdapter(cmd);

                    DataSet ds = new DataSet("MainDataSet");

                    adapter = new OleDbDataAdapter(cmd);
                   
                    adapter.Fill(ds);
                    IList<UserModel> users = new List<UserModel>();
                    var userlist = ds.Tables[0];
                    // var userList = db.Users.ToList();
                    foreach (DataRow user in userlist.Rows)
                    {

                        model.FirstName = user["FirstName"].ToString();
                        model.Lastname = user["Lastname"].ToString();
                        model.UserId = Convert.ToInt32(user["UserId"].ToString());
                        model.IsAdmin = Convert.ToBoolean(user["IsAdmin"].ToString());
                        model.UserName = user["Username"].ToString();
                        model.IsActive = Convert.ToBoolean(user["IsActive"].ToString());

                        
                    }
                    myConnection.Close();
                        return View(model);
                    }
               // }

                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: PatientInformation/Edit/5
        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");
                OleDbConnection conn = new OleDbConnection();

                conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];


                OleDbCommand cmd = new OleDbCommand("update [User] set Firstname = @Firstname, " +
                    "Lastname = @Lastname, IsAdmin = @IsAdmin,  Username= @Username" +
                    " where UserId = " + model.UserId);

                cmd.Connection = conn;

                conn.Open();

                int userid = 0;
                // Read the cookie information and display it.
                if (myCookie["userid"] != null)
                    userid = Convert.ToInt32(myCookie["userid"]); //mycookie.value
                else
                    return RedirectToAction("index", "Home");

                if (conn.State == ConnectionState.Open)
                {
                    cmd.Parameters.Add("@Firstname", OleDbType.VarChar).Value = model.FirstName;
                    cmd.Parameters.Add("@Lastname", OleDbType.VarChar).Value = model.Lastname;
                    cmd.Parameters.Add("@IsAdmin", OleDbType.Boolean).Value = model.IsAdmin;
                    cmd.Parameters.Add("@Username", OleDbType.VarChar).Value = model.UserName;
                    

                    try
                    {
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (OleDbException ex)
                    {
                        string str = ex.ToString();
                        conn.Close();
                        return View("Error");
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