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
    public class PatientController : Controller
    {
        // GET: PatientInformation
        public ActionResult Index()
        {
            try
            {
                IList<PatientModel> patients = new List<PatientModel>();
                OleDbConnection myConnection = new OleDbConnection();

                myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
               // OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\anshi\\documents\\mdtaccessdb.accdb");

                myConnection.Open();

                //var datT = myConnection.GetSchema("user");
                OleDbCommand cmd = new OleDbCommand("Select * FROM [Patient]", myConnection);
                OleDbDataAdapter adapter;
                adapter = new OleDbDataAdapter(cmd);
               
                DataSet ds = new DataSet("MainDataSet");
                
                adapter.Fill(ds);
               
                var patientList = ds.Tables[0];
                foreach (DataRow patient in patientList.Rows)
                {
                    // MDTDetails mDTDetails = new MDTDetails();
                    IList<MDTDetails> mDTDetails = new List<MDTDetails>();
                    //var MdtDetails = db.MdtEpisodes.Where(x => x.MdtPatientId == patient.PatientId).OrderByDescending(x => x.MdtDate).ToList();
                    //if (MdtDetails != null)
                    //    foreach (var mdt in MdtDetails)
                    //        mDTDetails.Add(new MDTDetails { MDTId = mdt.MdtId, MDTDate = mdt.MdtDate });

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

                return View(patients);
            }
            catch
            {
                return View("Error");
               /// return view
            }
           
        }

        // GET: PatientInformation/Details/5
        public ActionResult Details(int? id = 0)
        {
            try { 
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
                    if(ds.Tables.Count > 0) { 
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
                        // }
                        //OleDbCommand Mdtcmd = new OleDbCommand("Select * FROM [MdtEpisodes] where Id = " + id, myConnection);
                        //OleDbDataAdapter adapter;
                        //adapter = new OleDbDataAdapter(cmd);

                        //var mdt = db.MdtEpisodes.OrderByDescending(x => x.MdtDate).FirstOrDefault(x => x.MdtPatientId == id);
                        //if (mdt != null)
                        //{
                        //    model.Comorbidities = mdt.Comorbidities;
                        //    model.History = mdt.History;
                        //    model.MDTDate = mdt.MdtDate;
                        //    model.MDTDiscussion = mdt.MdtDiscussion;
                        //    model.MdtId = mdt.MdtId;
                        //}
                        //IList<MDTDetails> mDTDetails = new List<MDTDetails>();
                        //var MdtDetails = db.MdtEpisodes.Where(x => x.MdtPatientId == patient.PatientId).OrderByDescending(x => x.MdtDate).ToList();
                        //if (MdtDetails != null)
                        //    foreach (var mdtdetail in MdtDetails)
                        //        mDTDetails.Add(new MDTDetails { MDTId = mdtdetail.MdtId, MDTDate = mdtdetail.MdtDate });
                        //model.MDTEpisode = mDTDetails;
                    }
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

        // GET: PatientInformation/Create
        public ActionResult Create()
        {
            try { 
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
                OleDbConnection conn = new OleDbConnection();

                conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
                //@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\anshi\\documents\\mdtaccessdb.accdb";
               
                OleDbCommand cmd = new OleDbCommand("INSERT into [Patient] (Firstname, Lastname, HospitalNo, NhsNo, DateofBirth, AddressLine1, AddressLine2, City, Postcode,GpCity," +
                    "GpName, GpAddressLine1, GpAddressLine2, GpPostcode, DateCreated,  UserId) " +
                    "Values(@Firstname, @Lastname, @HospitalNo, @NhsNo, @DateofBirth," +
                    "@AddressLine1, @AddressLine2, @City, @Postcode,@GpCity,@GpName, @GpAddressLine1, @GpAddressLine2, @GpPostcode, @DateCreated,  @UserId)");
                cmd.Connection = conn;

                conn.Open();
                HttpCookie myCookie = Request.Cookies["MyTestCookie"];
                string userid = "1";
                // Read the cookie information and display it.
                //if (myCookie != null)
                //    userid =  myCookie.Value;
                //else
                //    return RedirectToAction("index", "Home");

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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PatientInformation/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: PatientInformation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatientInformation/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Filter(FormCollection formCollection)
        {
            try { 
            var filterBy = formCollection.Get("txtFilter");
                // OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\users\\anshi\\documents\\mdtaccessdb.accdb");
                OleDbConnection myConnection = new OleDbConnection();

                myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
                myConnection.Open();

            //var datT = myConnection.GetSchema("user");
            OleDbCommand cmd = new OleDbCommand("Select * FROM [Patient] where FirstName = '" + filterBy + "' OR LastName= '" + filterBy + "' OR NhsNo= '" + filterBy + "' OR HospitalNo= '" + filterBy + "'", myConnection);
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
            return View("Index", patientList);
        }
            catch (Exception ex)
            {
                return View("Error");
        }

    }
    }
}
