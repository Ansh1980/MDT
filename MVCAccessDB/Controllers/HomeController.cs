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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            UserModel model = new UserModel();
            model.Error = "";
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(UserModel model)
        {
            try
            {
                OleDbConnection myConnection = new OleDbConnection();
                myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

                myConnection.Open();
                OleDbDataAdapter adapter;

                OleDbCommand cmd = new OleDbCommand("Select * FROM [user] where IsActive = true and Username = '" + model.UserName + "'", myConnection);
                adapter = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet("MainDataSet");

                adapter.Fill(ds);
                IList<UserModel> users = new List<UserModel>();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                { 
               
                var userlist = ds.Tables[0];
                    HttpCookie myCookie = new HttpCookie("MDTuserCookie");
                    DateTime now = DateTime.Now;
                    // Set the cookie expiration date.
                    myCookie.Expires = now.AddHours(4); // For a cookie to effectively never expire

                    // Add the cookie.
                  
                    Response.Cookies.Add(myCookie);
                  
                    foreach (DataRow user in userlist.Rows)
                    {
                    users.Add(new UserModel
                    {
                        FirstName = user["FirstName"].ToString(),
                        Lastname = user["Lastname"].ToString(),
                        UserId = Convert.ToInt32(user["UserId"].ToString()),
                        IsAdmin = Convert.ToBoolean(user["IsAdmin"].ToString())

                    });
                        // Set the cookie value.
                        myCookie.Values["userid"] = user["UserId"].ToString();
                        myCookie.Values["isadmin"] = user["IsAdmin"].ToString();
                        break;
                    }
                myConnection.Close();
                    if(users[0].IsAdmin)
                    return RedirectToAction("index", "patient");
                    else
                     return RedirectToAction("index", "MDTuser");
                }
                model.Error = "Incorrect Username/Password";
                return View(model);
                
            }
            catch (Exception ex)
            {
                return View("Error");
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}