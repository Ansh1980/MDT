using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MDTWeb.Models;

namespace MDTWeb.Controllers
{
    public class PatientController : Controller
    {
        // GET: PatientInformation
        public ActionResult Index()
        {
            IList<PatientModel> patients = new List<PatientModel>();
            //try
            //{
            using (var db = new MyDbContext("Name=MDTDbConn"))
            {
                var patientList = db.PatientInformations.ToList();
                foreach (var patient in patientList)
                {
                   // MDTDetails mDTDetails = new MDTDetails();
                    IList<MDTDetails> mDTDetails = new List<MDTDetails>();
                    var MdtDetails = db.MdtEpisodes.Where(x => x.MdtPatientId == patient.PatientId).OrderByDescending(x =>x.MdtDate).ToList();
                    if (MdtDetails != null)
                    foreach (var mdt in MdtDetails)
                        mDTDetails.Add(new MDTDetails { MDTId = mdt.MdtId, MDTDate = mdt.MdtDate });
              
                    patients.Add(new PatientModel
                    {
                        FirstName = patient.FirstName,
                        LastName = patient.LastName,
                        NhsNo = patient.NhsNo,
                        HospitalNo = patient.HospitalNo,
                        PatientId = patient.PatientId,
                        MDTEpisode = mDTDetails
                    });
                }

                return View(patients);
            }
        }

        // GET: PatientInformation/Details/5
        public ActionResult Details(int? id = 0)
        {
            MDTModel model = new MDTModel();
            if (id != null && id >0)
                {
               
                if (id != null && id != 0)
                    model.MDTPatientId = Convert.ToInt32(id);
                using (var db = new MyDbContext("Name=MDTDbConn"))
                {
                    var patient = db.PatientInformations.FirstOrDefault(x => x.PatientId == id);
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

                    var mdt = db.MdtEpisodes.OrderByDescending(x => x.MdtDate).FirstOrDefault(x => x.MdtPatientId == id);
                    if(mdt != null)
                    {
                        model.Comorbidities = mdt.Comorbidities;
                        model.History = mdt.History;
                        model.MDTDate = mdt.MdtDate;
                        model.MDTDiscussion = mdt.MdtDiscussion;
                        model.MdtId = mdt.MdtId;
                    }
                    IList<MDTDetails> mDTDetails = new List<MDTDetails>();
                    var MdtDetails = db.MdtEpisodes.Where(x => x.MdtPatientId == patient.PatientId).OrderByDescending(x => x.MdtDate).ToList();
                    if (MdtDetails != null)
                        foreach (var mdtdetail in MdtDetails)
                            mDTDetails.Add(new MDTDetails { MDTId = mdtdetail.MdtId, MDTDate = mdtdetail.MdtDate });
                    model.MDTEpisode = mDTDetails;
                }
                return View(model);
            }
            return View(model);
        }

        // GET: PatientInformation/Create
        public ActionResult Create()
        {
            PatientModel model = new PatientModel();
            return View(model);
        }

        // POST: PatientInformation/Create
        [HttpPost]
        public ActionResult Create(PatientModel model)//FormCollection collection)
        {
            try
            {
               
                using (var db = new MyDbContext("Name=MDTDbConn"))
                {
                    var pe = db.Set<PatientInformation>();
                   pe.Add(new PatientInformation { FirstName = model.FirstName,
                   LastName = model.LastName,
                   HospitalNo = model.HospitalNo,
                   NhsNo = model.NhsNo,
                   DateofBirth = model.DateofBirth,
                   AddressLine1 = model.AddressLine1,
                   AddressLine2 = model.AddressLine2,
                   City = model.City,
                   Postcode = model.Postcode,
                   GpCity = model.GpCity,
                   GpName = model.GpName,
                   GpAddressLine1 = model.GpAddressLine1,
                   GpAddressLine2 = model.GpAddressLine2,
                   GpPostcode = model.GpPostcode,
                   DateCreated = DateTime.Now,
                   RowGuid = Guid.NewGuid(),
                   UserId = 1});

                    db.SaveChanges();
                   

                };
               return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
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
                return View();
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

            var filterBy = formCollection.Get("txtFilter");
            //var filterBy2 = formCollection.Get("txtFilter2");
            //var filterBy3 = formCollection.Get("CategoryId");
            IList<PatientModel> patientList = new List<PatientModel>();
            using (var db = new MyDbContext("Name=MDTDbConn"))
            {
                var patients = db.PatientInformations.Where(x =>x.FirstName == filterBy || x.LastName == filterBy || x.NhsNo == filterBy || x.HospitalNo == filterBy ).ToList();
       
            foreach (var patient in patients)
            {
                    patientList.Add(new PatientModel
                    {
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    NhsNo = patient.NhsNo,
                    HospitalNo = patient.HospitalNo,
                    PatientId = patient.PatientId

                });
            }
            };
            return View("Index", patientList);
          
        }
    }
}
