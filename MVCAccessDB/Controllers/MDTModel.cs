using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MDTWeb.Models
{
    public class MDTModel
    {
        public int MdtId { get; set; } // MDTId (Primary key)
        public int MDTPatientId { get; set; } // MDTPatientId
        [DisplayName("MDT Date")]
        // [DataType(DataType.Date)]
        public System.DateTime MDTDate { get; set; } // MDtDate
        public string History { get; set; } // History
        public string Comorbidities { get; set; } // Comorbidities (length: 500)
        [DisplayName("MDT Discussion")]
        public string MDTDiscussion { get; set; } // MDTDiscussion
        public PatientModel Patient {get;set;}
        public int? RedirectFrom { get; set; }
        public IList<MDTDetails> MDTEpisode { get; set; }
        public MDTModel()
        {
            Patient = new PatientModel();
            MDTEpisode = new List<MDTDetails>();
        }
    }
}