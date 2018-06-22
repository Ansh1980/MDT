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
            //using (OleDbConnection dbc = new OleDbConnection(ConfigurationManager.AppSettings["connectionString"]))
            //{
            //    using (OleDbCommand cmd = dbc.CreateCommand())
            //    {
            //        cmd.CommandText = "select * from [user]";
            //        adapter = new OleDbDataAdapter(cmd);
            //      //  builder = new OleDbCommandBuilder(adapter);
            //        DataSet ds = new DataSet("UserDataSet");
            //        // tempDs = new DataSet("TempDataSet");

            //        // connection.Open();
            //        //adapter.Fill(tempDs);
            //        //tempDs.Clear();
            //        //tempDs.Dispose();
            //        adapter.Fill(ds);
            OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\ashrivastava\\documents\\test.accdb");

            myConnection.Open();

            //var datT = myConnection.GetSchema("user");
            OleDbCommand cmd = new OleDbCommand("Select * FROM [user]", myConnection);
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
            // var userList = db.Users.ToList();
            //foreach (var user in ds.)
            //{
            //    users.Add(new UserModel
            //    {
            //        FirstName = user.FirstName,
            //        Lastname = user.Lastname,
            //        UserId = user.UserId,
            //        UserName = user.UserName

            //    });
            //}
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

        //[HttpPost]
        //public ActionResult Create(UserModel model)//FormCollection collection)
        //{
        //    try
        //    {

        //        using (var db = new MyDbContext("Name=MDTDbConn"))
        //        {
        //            var mdt = db.Set<User>();

        //            mdt.Add(new User
        //            {
        //                DateCreated = DateTime.Now,
        //                FirstName = model.FirstName,
        //                Lastname = model.Lastname,
        //                IsAdmin = model.IsAdmin,
        //                UserName = "admin",
        //                Password = "admin",
        //                RowGuid = Guid.NewGuid()

        //            });

        //            db.SaveChanges();

        //        };
        //        return View("Index", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return View();
        //    }
        //}
    }
}