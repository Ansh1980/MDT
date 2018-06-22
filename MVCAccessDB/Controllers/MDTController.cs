using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MDTWeb.Models;

namespace MDTWeb.Controllers
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
            MDTModel model = new MDTModel();
            if (id != null && id != 0)
                model.MDTPatientId = Convert.ToInt32(id);
            using (var db = new MyDbContext("Name=MDTDbConn"))
            {
                var patient = db.PatientInformations.FirstOrDefault(x => x.PatientId == id);
                IList<MDTDetails> mDTDetails = new List<MDTDetails>();
                var MdtDetails = db.MdtEpisodes.Where(x => x.MdtPatientId == patient.PatientId).ToList();
                if (MdtDetails != null)
                {
                    MdtDetails = MdtDetails.OrderByDescending(x => x.MdtDate).ToList();
                    foreach (var mdtdetail in MdtDetails)
                        mDTDetails.Add(new MDTDetails { MDTId = mdtdetail.MdtId, MDTDate = mdtdetail.MdtDate });
                }
                if (patient != null)
                {
                    model.Patient.FirstName = patient.FirstName;
                    model.Patient.LastName = patient.LastName;
                    model.Patient.NhsNo = patient.NhsNo;
                    model.Patient.PatientId = patient.PatientId;
                    model.Patient.AddressLine1 = patient.AddressLine1;
                    model.Patient.AddressLine2 = patient.AddressLine2;
                    model.Patient.City = patient.City;
                    model.Patient.DateofBirth = patient.DateofBirth;
                    model.Patient.GpAddressLine1 = patient.GpAddressLine1;
                    model.Patient.GpAddressLine2 = patient.GpAddressLine2;
                    model.Patient.GpCity = patient.GpCity;
                    model.Patient.GpName = patient.GpName;
                    model.Patient.GpPostcode = patient.GpPostcode;
                    model.Patient.HospitalNo = patient.HospitalNo;
                    model.Patient.Postcode = patient.Postcode;
                    model.MDTEpisode = mDTDetails;
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MDTModel model)//FormCollection collection)
        {
            try
            {
                
                using (var db = new MyDbContext("Name=MDTDbConn"))
                {
                    Guid rowid = Guid.NewGuid();
                    var mdt = db.Set<MdtEpisode>();
                    mdt.Add(new MdtEpisode
                    {
                        Comorbidities = model.Comorbidities,
                        History = model.History,
                        MdtDate = model.MDTDate,
                        MdtDiscussion = model.MDTDiscussion,
                        MdtPatientId = model.MDTPatientId,
                        RowGuid = rowid,
                        DateCreated = DateTime.Now,
                        UserId = 1
                        
                    });

                    db.SaveChanges();

                    model.MdtId = db.MdtEpisodes.Where(x => x.RowGuid == rowid).FirstOrDefault().MdtId;
                    
                };
                return RedirectToAction("Details","MDT", new {id = model.MdtId });
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        public ActionResult Details(int? id = 0 ) // page = 1 is coming from patiend details & Id is patient id, show latest MDT 
        {
            MDTModel model = new MDTModel();
            if (id != null && id > 0)
            {
              //  model.RedirectFrom = page;                                
                using (var db = new MyDbContext("Name=MDTDbConn"))
                {
                   
                    model.MDTPatientId = db.MdtEpisodes.Where(x => x.MdtId == id).FirstOrDefault().MdtPatientId;
                    var patient = db.PatientInformations.FirstOrDefault(x => x.PatientId == model.MDTPatientId);
                    if (patient != null)
                    {
                        model.Patient.FirstName = patient.FirstName;
                        model.Patient.LastName = patient.LastName;
                        model.Patient.NhsNo = patient.NhsNo;
                        model.Patient.PatientId = patient.PatientId;
                        model.Patient.AddressLine1 = patient.AddressLine1;
                        model.Patient.AddressLine2 = patient.AddressLine2;
                        model.Patient.City = patient.City;
                        model.Patient.DateofBirth = patient.DateofBirth;
                        model.Patient.GpAddressLine1 = patient.GpAddressLine1;
                        model.Patient.GpAddressLine2 = patient.GpAddressLine2;
                        model.Patient.GpCity = patient.GpCity;
                        model.Patient.GpName = patient.GpName;
                        model.Patient.GpPostcode = patient.GpPostcode;
                        model.Patient.HospitalNo = patient.HospitalNo;
                        model.Patient.Postcode = patient.Postcode;
                    }
                    IList<MDTDetails> mDTDetails = new List<MDTDetails>();
                    var MdtDetails = db.MdtEpisodes.Where(x => x.MdtPatientId == patient.PatientId).OrderByDescending(x => x.MdtDate).ToList();
                    if (MdtDetails != null)
                        foreach (var mdtdetail in MdtDetails)
                            mDTDetails.Add(new MDTDetails { MDTId = mdtdetail.MdtId, MDTDate = mdtdetail.MdtDate });

                    var mdt = db.MdtEpisodes.FirstOrDefault(x => x.MdtId == id);
                    if (mdt != null)
                    {
                        model.Comorbidities = mdt.Comorbidities;
                        model.History = mdt.History;
                        model.MDTDate = mdt.MdtDate;
                        model.MDTDiscussion = mdt.MdtDiscussion;
                        model.MdtId = mdt.MdtId;
                        model.MDTEpisode = mDTDetails;
                    }
                }
                return View(model);
            }
            return View(model);
        }
    }
}