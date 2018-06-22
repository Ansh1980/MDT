using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace MDTWeb.Models
{
    public class PatientModel
    {
        public int PatientId { get; set; }
        [DisplayName("First Name")]
        public string  FirstName{get;set;}
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Hospital No")]
        public string HospitalNo { get; set; } // HospitalNo (length: 50)
        [DisplayName("NHS No")]
        public string NhsNo { get; set; } // NSHNo (length: 50)
        [DisplayName("Date of Birth")]
        public System.DateTime DateofBirth { get; set; } // DateofBirth
        [DisplayName("Address Line1")]
        public string AddressLine1 { get; set; } // AddressLine1 (length: 50)
        [DisplayName("Address Line2")]
        public string AddressLine2 { get; set; } // AddressLine2 (length: 50)
        public string City { get; set; } // City (length: 50)
        public string Postcode { get; set; } // Postcode (length: 50)
        [DisplayName("Gp Name")]
        public string GpName { get; set; } // GPName (length: 100)
        [DisplayName("Address Line1")]
        public string GpAddressLine1 { get; set; } // GPAddressLine1 (length: 50)
        [DisplayName("Address Line1")]
        public string GpAddressLine2 { get; set; } // GPAddressLine2 (length: 50)
        [DisplayName("City")]
        public string GpCity { get; set; } // GPCity (length: 50)
        [DisplayName("Postcode")]
        public string GpPostcode { get; set; } // GPPostcode (length: 50)
        public IList<MDTDetails> MDTEpisode { get; set; }
        public PatientModel()
        {
            MDTEpisode = new List<MDTDetails>();
        }
    }

    public class MDTDetails
    {
        public int MDTId { get; set; }
        public DateTime MDTDate { get; set; }
    }
}