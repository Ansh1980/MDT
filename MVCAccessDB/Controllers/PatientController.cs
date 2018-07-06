﻿using System;
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
    public class PatientController : Controller
    {
        // GET: PatientInformation
        public ActionResult Index()
        {
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];
               
                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");
                ViewBag.FilterBy = "";
                IList<PatientModel> patients = new List<PatientModel>();
                OleDbConnection myConnection = new OleDbConnection();

                myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
               // OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\anshi\\documents\\mdtaccessdb.accdb");

                myConnection.Open();

                //var datT = myConnection.GetSchema("user");
                OleDbCommand cmd = new OleDbCommand("Select * FROM [Patient] where Deleted = false", myConnection);
                OleDbDataAdapter adapter;
                adapter = new OleDbDataAdapter(cmd);
               
                DataSet ds = new DataSet("MainDataSet");
                
                adapter.Fill(ds);
               
                var patientList = ds.Tables[0];
                foreach (DataRow patient in patientList.Rows)
                {
                    
                    IList<MDTDetails> mDTDetails = new List<MDTDetails>();
                   
                    patients.Add(new PatientModel
                    {
                        FirstName = patient["FirstName"].ToString(),
                        LastName = patient["LastName"].ToString(),
                        NhsNo = patient["NhsNo"].ToString(),
                        HospitalNo = patient["HospitalNo"].ToString(),
                        PatientId = Convert.ToInt32(patient["PatientId"].ToString()),
                        //MDTEpisode = mDTDetails
                    });
                }
                myConnection.Close();
                return View(patients);
            }
            catch
            {
                return View("Error");
              
            }
           
        }

        // GET: PatientInformation/Details/5
        public ActionResult Details(int? id = 0)
        {
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");

                MDTModel model = new MDTModel();
                if (id != null && id > 0)
                {

                    if (id != null && id != 0)
                        model.MDTPatientId = Convert.ToInt32(id);
                    //OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\anshi\\documents\\mdtaccessdb.accdb");
                    OleDbConnection myConnection = new OleDbConnection();

                    myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
                    myConnection.Open();

                    //var datT = myConnection.GetSchema("user");
                    OleDbCommand cmd = new OleDbCommand("Select * FROM [Patient] where PatientId = " + id, myConnection);
                    OleDbDataAdapter adapter;
                    adapter = new OleDbDataAdapter(cmd);

                    DataSet ds = new DataSet("MainDataSet");

                    adapter.Fill(ds);
                    // IList<UserModel> users = new List<UserModel>();
                    if (ds.Tables.Count > 0)
                    {
                        var patientList = ds.Tables[0];
                        foreach (DataRow patient in patientList.Rows)
                        {
                            //if (patient != null)
                            //{
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

                            cmd = new OleDbCommand("Select * FROM [MDT] where MDTPatientId = " + id + " order by MDTDate Desc", myConnection);
                            adapter = new OleDbDataAdapter(cmd);
                            ds = new DataSet("MainDataSet");

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
                    }

                    return View(model);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
    }

        // GET: PatientInformation/Create
        public ActionResult Create()
        {
            try {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");

                PatientModel model = new PatientModel();
               
                return View(model);
            }
            catch
            {
                return View("Error");
            }
}

        // POST: PatientInformation/Create
        [HttpPost]
        public ActionResult Create(PatientModel model)//FormCollection collection)
        {
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");
                OleDbConnection conn = new OleDbConnection();

                conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
                //@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\anshi\\documents\\mdtaccessdb.accdb";
               
                OleDbCommand cmd = new OleDbCommand("INSERT into [Patient] (Firstname, Lastname, HospitalNo, NhsNo, DateofBirth, AddressLine1, AddressLine2, City, Postcode,GpCity," +
                    "GpName, GpAddressLine1, GpAddressLine2, GpPostcode, DateCreated,  UserId) " +
                    "Values(@Firstname, @Lastname, @HospitalNo, @NhsNo, @DateofBirth," +
                    "@AddressLine1, @AddressLine2, @City, @Postcode,@GpCity,@GpName, @GpAddressLine1, @GpAddressLine2, @GpPostcode, @DateCreated,  @UserId)");
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
                    cmd.Parameters.Add("@Lastname", OleDbType.VarChar).Value = model.LastName;
                    cmd.Parameters.Add("@HospitalNo", OleDbType.VarChar).Value = model.HospitalNo;
                    cmd.Parameters.Add("@NhsNo", OleDbType.VarChar).Value = model.NhsNo;
                    cmd.Parameters.Add("@DateofBirth", OleDbType.Date).Value = model.DateofBirth;
                    cmd.Parameters.Add("@AddressLine1", OleDbType.VarChar).Value = model.AddressLine1;
                    cmd.Parameters.Add("@AddressLine2", OleDbType.VarChar).Value = model.AddressLine2;
                    cmd.Parameters.Add("@City", OleDbType.VarChar).Value = model.City;
                    cmd.Parameters.Add("@Postcode", OleDbType.VarChar).Value = model.Postcode;
                    cmd.Parameters.Add("@GpCity", OleDbType.VarChar).Value = model.GpCity;
                    cmd.Parameters.Add("@GpName", OleDbType.VarChar).Value = model.GpName;
                    cmd.Parameters.Add("@GpAddressLine1", OleDbType.VarChar).Value = model.GpAddressLine1;
                    cmd.Parameters.Add("@GpAddressLine2", OleDbType.VarChar).Value = model.GpAddressLine2;
                    cmd.Parameters.Add("@GpPostcode", OleDbType.VarChar).Value = model.GpPostcode;
                    cmd.Parameters.Add("@DateCreated", OleDbType.Date).Value = DateTime.Now;
                  //  cmd.Parameters.Add("@RowGuid", OleDbType.Guid).Value = Guid.NewGuid();
                    cmd.Parameters.Add("@UserId", OleDbType.Integer).Value = userid;

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

        // GET: PatientInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");

                PatientModel model = new PatientModel();
                if (id != null && id > 0)
                {

                    OleDbConnection myConnection = new OleDbConnection();

                    myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
                    myConnection.Open();

                    //var datT = myConnection.GetSchema("user");
                    OleDbCommand cmd = new OleDbCommand("Select * FROM [Patient] where PatientId = " + id, myConnection);
                    OleDbDataAdapter adapter;
                    adapter = new OleDbDataAdapter(cmd);

                    DataSet ds = new DataSet("MainDataSet");

                    adapter.Fill(ds);
                    // IList<UserModel> users = new List<UserModel>();
                    if (ds.Tables.Count > 0)
                    {
                        var patientList = ds.Tables[0];
                        foreach (DataRow patient in patientList.Rows)
                        {
                            
                            model.FirstName = patient["FirstName"].ToString();
                            model.LastName = patient["LastName"].ToString();
                            model.NhsNo = patient["NhsNo"].ToString();
                            model.PatientId = Convert.ToInt32(patient["PatientId"].ToString());
                            model.AddressLine1 = patient["AddressLine1"].ToString();
                            model.AddressLine2 = patient["AddressLine2"].ToString();
                            model.City = patient["City"].ToString();
                            model.DateofBirth = Convert.ToDateTime(patient["DateofBirth"].ToString());
                            model.GpAddressLine1 = patient["GpAddressLine1"].ToString();
                            model.GpAddressLine2 = patient["GpAddressLine2"].ToString();
                            model.GpCity = patient["GpCity"].ToString();
                            model.GpName = patient["GpName"].ToString();
                            model.GpPostcode = patient["GpPostcode"].ToString();
                            model.HospitalNo = patient["HospitalNo"].ToString();
                            model.Postcode = patient["Postcode"].ToString();
                            
                        }
                        myConnection.Close();
                        return View(model);
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: PatientInformation/Edit/5
        [HttpPost]
        public ActionResult Edit(PatientModel model)
        {
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");
                OleDbConnection conn = new OleDbConnection();

                conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
                

                OleDbCommand cmd = new OleDbCommand("update [Patient] set Firstname = @Firstname, " +
                    "Lastname = @Lastname, HospitalNo = @HospitalNo,  NhsNo= @NhsNo,  DateofBirth= @DateofBirth, AddressLine1 = @AddressLine1, AddressLine2 = @AddressLine2,  City = @City," +
                    " Postcode = @Postcode ,GpCity = @GpCity, " +
                    "GpName = @GpName, GpAddressLine1 = @GpAddressLine1, GpAddressLine2 = @GpAddressLine2, GpPostcode = @GpPostcode" +
                   
                    " where PatientId = " + model.PatientId);
                    
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
                    cmd.Parameters.Add("@Lastname", OleDbType.VarChar).Value = model.LastName;
                    cmd.Parameters.Add("@HospitalNo", OleDbType.VarChar).Value = model.HospitalNo;
                    cmd.Parameters.Add("@NhsNo", OleDbType.VarChar).Value = model.NhsNo;
                    cmd.Parameters.Add("@DateofBirth", OleDbType.Date).Value = model.DateofBirth;
                    cmd.Parameters.Add("@AddressLine1", OleDbType.VarChar).Value = model.AddressLine1;
                    cmd.Parameters.Add("@AddressLine2", OleDbType.VarChar).Value = model.AddressLine2;
                    cmd.Parameters.Add("@City", OleDbType.VarChar).Value = model.City;
                    cmd.Parameters.Add("@Postcode", OleDbType.VarChar).Value = model.Postcode;
                    cmd.Parameters.Add("@GpCity", OleDbType.VarChar).Value = model.GpCity;
                    cmd.Parameters.Add("@GpName", OleDbType.VarChar).Value = model.GpName;
                    cmd.Parameters.Add("@GpAddressLine1", OleDbType.VarChar).Value = model.GpAddressLine1;
                    cmd.Parameters.Add("@GpAddressLine2", OleDbType.VarChar).Value = model.GpAddressLine2;
                    cmd.Parameters.Add("@GpPostcode", OleDbType.VarChar).Value = model.GpPostcode;
                    //cmd.Parameters.Add("@DateCreated", OleDbType.Date).Value = DateTime.Now;
                    ////  cmd.Parameters.Add("@RowGuid", OleDbType.Guid).Value = Guid.NewGuid();
                    //cmd.Parameters.Add("@UserId", OleDbType.Integer).Value = userid;

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

        // GET: PatientInformation/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

        //    if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
        //        return RedirectToAction("index", "Home");
        //    return View();
        //}

        // POST: PatientInformation/Delete/5
       // [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");
                // TODO: Add delete logic here

                OleDbConnection conn = new OleDbConnection();

                conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];


                OleDbCommand cmd = new OleDbCommand("update [Patient] set Deleted = @Deleted "+

                    " where PatientId = " + id);

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
                    cmd.Parameters.Add("@Deleted", OleDbType.Boolean).Value = true;
                    

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
                        return View("Error");
                    }
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Filter(FormCollection formCollection)
        {
            try {
                HttpCookie myCookie = Request.Cookies["MDTuserCookie"];

                if (myCookie == null || myCookie["userid"] == null || myCookie["isadmin"] != "True")
                    return RedirectToAction("index", "Home");

                var filterBy = formCollection.Get("txtFilter");
                ViewBag.FilterBy = filterBy;
                if (filterBy == "")
                   return RedirectToAction("index");
                // OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\anshi\\documents\\mdtaccessdb.accdb");
                OleDbConnection myConnection = new OleDbConnection();

                myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
                myConnection.Open();
                OleDbCommand cmd = new OleDbCommand();

            if (filterBy.Contains(" "))
                {
                    string[] arrfilter = filterBy.Split(' ');
                    cmd = new OleDbCommand("Select * FROM [Patient] where FirstName = '" + arrfilter[1] + "' and LastName= '" + arrfilter[0] + "'", myConnection);
                }
            else
             cmd = new OleDbCommand("Select * FROM [Patient] where FirstName = '" + filterBy + "' OR LastName= '" + filterBy + "' OR NhsNo= '" + filterBy + "' OR HospitalNo= '" + filterBy + "'", myConnection);
            OleDbDataAdapter adapter;
            adapter = new OleDbDataAdapter(cmd);

            DataSet ds = new DataSet("MainDataSet");

            adapter.Fill(ds);
          
          
            IList<PatientModel> patientList = new List<PatientModel>();
            if (ds.Tables.Count > 0)
            {
                var patients = ds.Tables[0];
                foreach (DataRow patient in patients.Rows)
                {
                    patientList.Add(new PatientModel
                    {
                        FirstName = patient["FirstName"].ToString(),
                        LastName = patient["LastName"].ToString(),
                        NhsNo = patient["NhsNo"].ToString(),
                        HospitalNo = patient["HospitalNo"].ToString(),
                        PatientId = Convert.ToInt32(patient["PatientId"].ToString())

                    });

                }
            }
                myConnection.Close();
                return View("Index", patientList);
        }
            catch (Exception ex)
            {
                return View("Error");
        }

    }
    }
}
