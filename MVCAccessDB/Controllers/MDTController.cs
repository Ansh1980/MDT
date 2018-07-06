using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCAccessDB.Models;
using System.Data;
using System.Data.OleDb;
using System.Configuration;


namespace MVCAccessDB.Controllers
{
    public class MDTController : Controller
    {
        // GET: MDT
        public ActionResult Index()
        {
            return View();
        }
        // GET: PatientInformation/Create
        public ActionResult Create(int? id = 0)
        {
            try {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");

                MDTModel model = new MDTModel();
            if (id != null && id != 0)
                model.MDTPatientId = Convert.ToInt32(id);
            // OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\anshi\\documents\\mdtaccessdb.accdb");
            OleDbConnection myConnection = new OleDbConnection();

            myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            myConnection.Open();

            //var datT = myConnection.GetSchema("user");
            OleDbCommand cmd = new OleDbCommand("Select * FROM [Patient] where PatientId = " + id, myConnection);
            OleDbDataAdapter adapter;
            adapter = new OleDbDataAdapter(cmd);

            DataSet ds = new DataSet("MainDataSet");

            adapter.Fill(ds);
            var patientList = ds.Tables[0];
                  
            foreach (DataRow patient in patientList.Rows)
            {
                if (patient != null)
                {
                    model.MDTPatientId = Convert.ToInt32(patient["PatientId"].ToString());
                    model.Patient.FirstName = patient["FirstName"].ToString();
                    model.Patient.LastName = patient["LastName"].ToString();
                    model.Patient.NhsNo = patient["NhsNo"].ToString();
                    model.Patient.PatientId = Convert.ToInt32(patient["PatientId"].ToString());
                    model.Patient.AddressLine1 = patient["AddressLine1"].ToString();
                    model.Patient.AddressLine2 = patient["AddressLine2"].ToString();
                    model.Patient.City = patient["City"].ToString();
                    model.Patient.DateofBirth = Convert.ToDateTime(patient["DateofBirth"].ToString());
                    model.Patient.GpAddressLine1 = patient["GpAddressLine1"].ToString();
                    model.Patient.GpAddressLine2 = patient["GpAddressLine2"].ToString();
                    model.Patient.GpCity = patient["GpCity"].ToString();
                    model.Patient.GpName = patient["GpName"].ToString();
                    model.Patient.GpPostcode = patient["GpPostcode"].ToString();
                    model.Patient.HospitalNo = patient["HospitalNo"].ToString();
                    model.Patient.Postcode = patient["Postcode"].ToString();
                }

            }
            cmd = new OleDbCommand("Select * FROM [MDT] where MDTPatientId = " + model.MDTPatientId + " order by MDTDate desc", myConnection);

            adapter = new OleDbDataAdapter(cmd);

            ds = new DataSet("MainDataSet");

            adapter.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                var mdtList = ds.Tables[0];
                foreach (DataRow mdtdetail in mdtList.Rows)
                {

                    model.MDTEpisode.Add(new MDTDetails { MDTId = Convert.ToInt32(mdtdetail["MdtId"].ToString()), MDTDate = Convert.ToDateTime(mdtdetail["MdtDate"].ToString()) });
                }
            }

                cmd = new OleDbCommand("Select * FROM [User] where IsActive = true order by FirstName ASC", myConnection);

                adapter = new OleDbDataAdapter(cmd);

                ds = new DataSet("MainDataSet");

                adapter.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    var userList = ds.Tables[0];
                    foreach (DataRow userdetail in userList.Rows)
                    {

                        model.Users.Add(new UserList { UserId = Convert.ToInt32(userdetail["UserId"].ToString()), FullName = userdetail["FirstName"].ToString() + " " + userdetail["LastName"].ToString() });
                    }
                }
                myConnection.Close();
                return View(model);
        }
        catch(Exception ex)
            {
                return View("Error");
}
        }

        [HttpPost]
        public ActionResult Create(MDTModel model)
        {
          
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");

                OleDbConnection conn = new OleDbConnection();

                conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
               
                OleDbCommand cmd = new OleDbCommand("INSERT into [MDT] (Comorbidities, History, MDTDate, MDTDiscussion, MDTPatientId, DateCreated, Userid, RowId) Values(@Comorbidities, @History , @MDTDate, @MDTDiscussion, @MDTPatientId, @DateCreated, @UserId, @RowId)");
                cmd.Connection = conn;

                conn.Open();
                Guid rowid = Guid.NewGuid();
                if (conn.State == ConnectionState.Open)
                {
                    cmd.Parameters.Add("@Comorbidities", OleDbType.VarChar).Value = model.Comorbidities;
                    cmd.Parameters.Add("@History", OleDbType.VarChar).Value = model.History;
                    cmd.Parameters.Add("@MDTDate", OleDbType.VarChar).Value = model.MDTDate;
                    cmd.Parameters.Add("@MDTDiscussion", OleDbType.VarChar).Value = model.MDTDiscussion;
                    cmd.Parameters.Add("@MDTPatientId", OleDbType.VarChar).Value = model.MDTPatientId;
                    cmd.Parameters.Add("@DateCreated", OleDbType.VarChar).Value = DateTime.Now;
                    cmd.Parameters.Add("@UserId", OleDbType.VarChar).Value = 1;
                    cmd.Parameters.Add("@RowId", OleDbType.Guid).Value = rowid;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        //var datT = myConnection.GetSchema("user");
                        cmd = new OleDbCommand("Select * FROM [MDT] where RowId = '{" + rowid + "}'", conn);
                        OleDbDataAdapter adapter;
                        adapter = new OleDbDataAdapter(cmd);

                        DataSet ds = new DataSet("MainDataSet");

                        adapter.Fill(ds);
                        var MDTList = ds.Tables[0];

                        foreach (DataRow mdt in MDTList.Rows)
                        {
                            model.MdtId = Convert.ToInt32(mdt["MDTId"].ToString());
                            break;
                        }
                        foreach(var user in model.Users.Where(x =>x.Selected))
                        { 
                        cmd = new OleDbCommand("INSERT into [MDTUser] (MDTId, UserId, FullName) Values(@MDTId, @UserId , @FullName)");
                        cmd.Connection = conn;


                        if (conn.State == ConnectionState.Open)
                        {

                            cmd.Parameters.Add("@MDTId", OleDbType.VarChar).Value = model.MdtId;
                            cmd.Parameters.Add("@UserId", OleDbType.VarChar).Value = user.UserId;
                            cmd.Parameters.Add("@FullName", OleDbType.VarChar).Value = user.FullName;
                        }
                        cmd.ExecuteNonQuery();
                    }
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

                conn.Close();
                return RedirectToAction("Details", "MDT", new { id = model.MdtId });
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            //try
            //{

            //    using (var db = new MyDbContext("Name=MDTDbConn"))
            //    {
            //        Guid rowid = Guid.NewGuid();
            //        var mdt = db.Set<MdtEpisode>();
            //        mdt.Add(new MdtEpisode
            //        {
            //            Comorbidities = model.Comorbidities,
            //            History = model.History,
            //            MdtDate = model.MDTDate,
            //            MdtDiscussion = model.MDTDiscussion,
            //            MdtPatientId = model.MDTPatientId,
            //            RowGuid = rowid,
            //            DateCreated = DateTime.Now,
            //            UserId = 1

            //        });

            //        db.SaveChanges();

            //        model.MdtId = db.MdtEpisodes.Where(x => x.RowGuid == rowid).FirstOrDefault().MdtId;

            //    };
            //    return RedirectToAction("Details", "MDT", new { id = model.MdtId });
            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }

        public ActionResult Details(int? id = 0 , string type = null) //MDT Id
        {
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");

                MDTModel model = new MDTModel();
                if (id != null && id > 0)
                {
                    OleDbDataAdapter adapter;
                    OleDbConnection myConnection = new OleDbConnection();
                    myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

                    myConnection.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    if (type == null)
                     cmd = new OleDbCommand("Select * FROM [MDT] where MDTId = " + id, myConnection); 
                    else
                      cmd = new OleDbCommand("Select * FROM [MDT] where MDTPatientId = " + id + " order by MDTDate Desc", myConnection); 


                    //OleDbCommand cmd = new OleDbCommand("Select MDT.* , User.FirstName, User.Lastname FROM [MDT] inner join MDTUser on MDTUser.MDTId = MDT.MDTId inner join USER" +
                    //    " on user.UserId = MDTUser.UserId" +
                    //    " where MDT.MDTId = " + id, myConnection);
                    adapter = new OleDbDataAdapter(cmd);
                    DataSet ds = new DataSet("MainDataSet");

                    adapter.Fill(ds);
                    //  IList<MDTModel> mdts = new List<MDTModel>();
                    if (ds.Tables.Count > 0)
                    {
                        var mdtlist = ds.Tables[0];
                        // var userList = db.Users.ToList();
                        foreach (DataRow mdt in mdtlist.Rows)
                        {
                            model.Comorbidities = mdt["Comorbidities"].ToString();
                            model.History = mdt["History"].ToString();
                            model.MDTDate = Convert.ToDateTime(mdt["MdtDate"].ToString());
                            model.MDTDiscussion = mdt["MdtDiscussion"].ToString();
                            model.MdtId = Convert.ToInt32(mdt["MdtId"].ToString());
                            model.MDTPatientId = Convert.ToInt32(mdt["MDTPatientId"].ToString());
                            // users.Add(new UserModel
                        }
                    }
                
                    cmd = new OleDbCommand("Select * FROM [Patient] where PatientId = " + model.MDTPatientId, myConnection);

                    adapter = new OleDbDataAdapter(cmd);
                    ds = new DataSet("MainDataSet");
                    adapter.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        var patientlist = ds.Tables[0];
                        // var userList = db.Users.ToList();
                        foreach (DataRow patient in patientlist.Rows)
                        {
                            model.Patient.FirstName = patient["FirstName"].ToString();
                            model.Patient.LastName = patient["LastName"].ToString();
                            model.Patient.NhsNo = patient["NhsNo"].ToString();
                            model.Patient.PatientId = Convert.ToInt32(patient["PatientId"].ToString());
                            model.Patient.AddressLine1 = patient["AddressLine1"].ToString();
                            model.Patient.AddressLine2 = patient["AddressLine2"].ToString();
                            model.Patient.City = patient["City"].ToString();
                            model.Patient.DateofBirth = Convert.ToDateTime(patient["DateofBirth"].ToString());
                            model.Patient.GpAddressLine1 = patient["GpAddressLine1"].ToString();
                            model.Patient.GpAddressLine2 = patient["GpAddressLine2"].ToString();
                            model.Patient.GpCity = patient["GpCity"].ToString();
                            model.Patient.GpName = patient["GpName"].ToString();
                            model.Patient.GpPostcode = patient["GpPostcode"].ToString();
                            model.Patient.HospitalNo = patient["HospitalNo"].ToString();
                            model.Patient.Postcode = patient["Postcode"].ToString();
                        }
                    }
                    cmd = new OleDbCommand("Select * FROM [MDTUser] where MDTId = " + model.MdtId, myConnection);
                    adapter = new OleDbDataAdapter(cmd);
                    ds = new DataSet("MainDataSet");
                    adapter.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        var userlist = ds.Tables[0];
                        // var userList = db.Users.ToList();
                        foreach (DataRow userdetail in userlist.Rows)
                        {
                            model.Users.Add(new UserList { UserId = Convert.ToInt32(userdetail["UserId"].ToString()), FullName = userdetail["FullName"].ToString(), Selected = true });
                        }
                    }

                    cmd = new OleDbCommand("Select * FROM [MDT] where MDTPatientId = " + model.MDTPatientId + " order by MDTDate desc", myConnection);

                    adapter = new OleDbDataAdapter(cmd);

                    ds = new DataSet("MainDataSet");

                    adapter.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        var mdtList = ds.Tables[0];
                        foreach (DataRow mdtdetail in mdtList.Rows)
                        {

                            model.MDTEpisode.Add(new MDTDetails { MDTId = Convert.ToInt32(mdtdetail["MdtId"].ToString()), MDTDate = Convert.ToDateTime(mdtdetail["MdtDate"].ToString()) });
                        }
                    }

                    myConnection.Close();

                    return View(model);
                }
                return View(model);
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult MDTDetails(int? id = 0) //Patient Id
        {
            try { 
            HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

            if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                return RedirectToAction("index", "Home");

            MDTModel model = new MDTModel();
            if (id != null && id > 0)
            {
                OleDbDataAdapter adapter;
                OleDbConnection myConnection = new OleDbConnection();
                myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

                myConnection.Open();

                OleDbCommand cmd = new OleDbCommand("Select * FROM [MDT] where MDTPatientId = " + id + " order by MDTDate Desc", myConnection);
                adapter = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet("MainDataSet");

                adapter.Fill(ds);
                //  IList<MDTModel> mdts = new List<MDTModel>();
                if (ds.Tables.Count > 0)
                {
                    var mdtlist = ds.Tables[0];
                    // var userList = db.Users.ToList();
                    foreach (DataRow mdt in mdtlist.Rows)
                    {
                        model.Comorbidities = mdt["Comorbidities"].ToString();
                        model.History = mdt["History"].ToString();
                        model.MDTDate = Convert.ToDateTime(mdt["MdtDate"].ToString());
                        model.MDTDiscussion = mdt["MdtDiscussion"].ToString();
                        model.MdtId = Convert.ToInt32(mdt["MdtId"].ToString());
                        model.MDTPatientId = Convert.ToInt32(mdt["MDTPatientId"].ToString());
                        // users.Add(new UserModel
                        break;
                    }


                    cmd = new OleDbCommand("Select * FROM [Patient] where PatientId = " + id, myConnection);
                    adapter = new OleDbDataAdapter(cmd);
                    ds = new DataSet("MainDataSet");
                    adapter.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        var patientlist = ds.Tables[0];
                        // var userList = db.Users.ToList();
                        foreach (DataRow patient in patientlist.Rows)
                        {
                            model.Patient.FirstName = patient["FirstName"].ToString();
                            model.Patient.LastName = patient["LastName"].ToString();
                            model.Patient.NhsNo = patient["NhsNo"].ToString();
                            model.Patient.PatientId = Convert.ToInt32(patient["PatientId"].ToString());
                            model.Patient.AddressLine1 = patient["AddressLine1"].ToString();
                            model.Patient.AddressLine2 = patient["AddressLine2"].ToString();
                            model.Patient.City = patient["City"].ToString();
                            model.Patient.DateofBirth = Convert.ToDateTime(patient["DateofBirth"].ToString());
                            model.Patient.GpAddressLine1 = patient["GpAddressLine1"].ToString();
                            model.Patient.GpAddressLine2 = patient["GpAddressLine2"].ToString();
                            model.Patient.GpCity = patient["GpCity"].ToString();
                            model.Patient.GpName = patient["GpName"].ToString();
                            model.Patient.GpPostcode = patient["GpPostcode"].ToString();
                            model.Patient.HospitalNo = patient["HospitalNo"].ToString();
                            model.Patient.Postcode = patient["Postcode"].ToString();
                        }
                    }
                }

                cmd = new OleDbCommand("Select * FROM [MDTUser] where MDTId = " + model.MdtId, myConnection);
                adapter = new OleDbDataAdapter(cmd);
                ds = new DataSet("MainDataSet");
                adapter.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    var userlist = ds.Tables[0];
                    // var userList = db.Users.ToList();
                    foreach (DataRow userdetail in userlist.Rows)
                    {
                        model.Users.Add(new UserList { UserId = Convert.ToInt32(userdetail["UserId"].ToString()), FullName = userdetail["FullName"].ToString(), Selected = true });
                    }
                }

                cmd = new OleDbCommand("Select * FROM [MDT] where MDTPatientId = " + model.MDTPatientId + " order by MDTDate desc", myConnection);

                adapter = new OleDbDataAdapter(cmd);

                ds = new DataSet("MainDataSet");

                adapter.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    var mdtList = ds.Tables[0];
                    foreach (DataRow mdtdetail in mdtList.Rows)
                    {

                        model.MDTEpisode.Add(new MDTDetails { MDTId = Convert.ToInt32(mdtdetail["MdtId"].ToString()), MDTDate = Convert.ToDateTime(mdtdetail["MdtDate"].ToString()) });
                    }
                }
                myConnection.Close();

                return View(model);
            }
            return View(model);
        }
            catch
            {
                return View("Error");
    }
}

        public ActionResult MDTUser()
        {
            HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

            if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                return RedirectToAction("index", "Home");
            return View();
        }

        // GET: PatientInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");

                MDTModel model = new MDTModel();
                if (id != null && id > 0)
                {

                    OleDbConnection myConnection = new OleDbConnection();

                    myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
                    myConnection.Open();

                    //var datT = myConnection.GetSchema("user");
                    OleDbCommand cmd = new OleDbCommand("Select * FROM [MDT] where MDTId = " + id, myConnection);
                    OleDbDataAdapter adapter;
                    adapter = new OleDbDataAdapter(cmd);

                    DataSet ds = new DataSet("MainDataSet");

                    adapter = new OleDbDataAdapter(cmd);

                    adapter.Fill(ds);
                    IList<MDTModel> users = new List<MDTModel>();
                    var mdtlist = ds.Tables[0];
                    // var userList = db.Users.ToList();
                    foreach (DataRow user in mdtlist.Rows)
                    {

                        model.Comorbidities = user["Comorbidities"].ToString();
                        model.History = user["History"].ToString();
                        model.MDTDate = Convert.ToDateTime(user["MDTDate"].ToString());
                        model.MDTDiscussion = user["MDTDiscussion"].ToString();
                       // model.Diagnosis = (DiagnosisList)user["MDTEpisode"].ToString();
                        model.MdtId = Convert.ToInt32(user["MDTId"].ToString());


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
        public ActionResult Details(MDTModel model)
        {
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");
                OleDbConnection conn = new OleDbConnection();

                conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

                OleDbCommand cmd = new OleDbCommand("update [MDT] set Comorbidities = @Comorbidities, " +
                    "History = @History, MDTDate = @MDTDate,  MDTDiscussion= @MDTDiscussion" +
                    " where MDTId = " + model.MdtId);

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
                    cmd.Parameters.Add("@Comorbidities", OleDbType.VarChar).Value = model.Comorbidities;
                    cmd.Parameters.Add("@History", OleDbType.VarChar).Value = model.History;
                    cmd.Parameters.Add("@MDTDate", OleDbType.Date).Value = model.MDTDate;
                    cmd.Parameters.Add("@MDTDiscussion", OleDbType.VarChar).Value = model.MDTDiscussion;
                    cmd.Parameters.Add("@MDTId", OleDbType.Integer).Value = model.MdtId;


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


                return RedirectToAction("Index" , "Patient");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}