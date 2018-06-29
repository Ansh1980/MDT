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
            IList<UserModel> users = new List<UserModel>();
            var patientList = ds.Tables[0];
            //var patient = db.PatientInformations.FirstOrDefault(x => x.PatientId == id);
            IList<MDTDetails> mDTDetails = new List<MDTDetails>();
            //var MdtDetails = db.MdtEpisodes.Where(x => x.MdtPatientId == patient.PatientId).ToList();
            //if (MdtDetails != null)
            //{
            //    MdtDetails = MdtDetails.OrderByDescending(x => x.MdtDate).ToList();
            //    foreach (var mdtdetail in MdtDetails)
            //        mDTDetails.Add(new MDTDetails { MDTId = mdtdetail.MdtId, MDTDate = mdtdetail.MdtDate });
            //}

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

                    mDTDetails.Add(new MDTDetails { MDTId = Convert.ToInt32(mdtdetail["MdtId"].ToString()), MDTDate = Convert.ToDateTime(mdtdetail["MdtDate"].ToString()) });
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
        public ActionResult Create(MDTModel model)//FormCollection collection)
        {
            try
            {
                OleDbConnection conn = new OleDbConnection();

                conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
               
                OleDbCommand cmd = new OleDbCommand("INSERT into [MDT] (Comorbidities, History, MDTDate, MDTDiscussion, MDTPatientId, DateCreated, Userid) Values(@Comorbidities, @History , @MDTDate, @MDTDiscussion, @MDTPatientId, @DateCreated, @UserId)");
                cmd.Connection = conn;

                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    cmd.Parameters.Add("@Comorbidities", OleDbType.VarChar).Value = model.Comorbidities;
                    cmd.Parameters.Add("@History", OleDbType.VarChar).Value = model.History;
                    cmd.Parameters.Add("@MDTDate", OleDbType.VarChar).Value = model.MDTDate;
                    cmd.Parameters.Add("@MDTDiscussion", OleDbType.VarChar).Value = model.MDTDiscussion;
                    cmd.Parameters.Add("@MDTPatientId", OleDbType.VarChar).Value = model.MDTPatientId;
                    cmd.Parameters.Add("@DateCreated", OleDbType.VarChar).Value = DateTime.Now;
                    cmd.Parameters.Add("@UserId", OleDbType.VarChar).Value = 1;

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

                conn.Close();
                return RedirectToAction("Index");
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

        public ActionResult Details(int? id = 0) //MDT Id
        {
            MDTModel model = new MDTModel();
            if (id != null && id > 0)
            {
                OleDbDataAdapter adapter;
                OleDbConnection myConnection = new OleDbConnection();
                myConnection.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

                myConnection.Open();

                OleDbCommand cmd = new OleDbCommand("Select * FROM [MDT] where MDTId = " + id, myConnection);
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


                    cmd = new OleDbCommand("Select * FROM [Patient] where PatientId = " + model.MDTPatientId, myConnection);
                    adapter = new OleDbDataAdapter(cmd);
                    ds = new DataSet("MainDataSet");
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
                myConnection.Close();
                    //IList<MDTDetails> mDTDetails = new List<MDTDetails>();
                    //var MdtDetails = db.MdtEpisodes.Where(x => x.MdtPatientId == patient.PatientId).OrderByDescending(x => x.MdtDate).ToList();
                    //if (MdtDetails != null)
                    //    foreach (var mdtdetail in MdtDetails)
                    //        mDTDetails.Add(new MDTDetails { MDTId = mdtdetail.MdtId, MDTDate = mdtdetail.MdtDate });

                    //var mdt = db.MdtEpisodes.FirstOrDefault(x => x.MdtId == id);
                    //if (mdt != null)
                    //{
                    //    model.Comorbidities = mdt.Comorbidities;
                    //    model.History = mdt.History;
                    //    model.MDTDate = mdt.MdtDate;
                    //    model.MDTDiscussion = mdt.MdtDiscussion;
                    //    model.MdtId = mdt.MdtId;
                    //   // model.MDTEpisode = mDTDetails;
                    //}
                //}
                return View(model);
            }
            return View(model);
        }

        public ActionResult MDTUser()
        {
            return View();
        }
    }
}