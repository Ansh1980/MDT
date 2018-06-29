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
    public class MDTUserController : Controller
    {
        // GET: MDTUser
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
                myConnection.Close();
                return View(patients);
            }
            catch
            {
                return View("Error");
                /// return view
            }
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
    }
}