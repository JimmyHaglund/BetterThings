using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAdReader {
    [Serializable]
    class JobAd {
        public string Headline = null;
        public string Occupation = null;
        public string OccupationFiltered => Occupation.Split('|')[0];
        public string OccupationGroup = null;
        public string OccupationGroupFiltered => OccupationGroup.Split('|')[0];
        public string OccupationField = null;
        public string Municipality = null;
        public string Town = null;
        public string ApplicationDeadline = null;
        public string AfUrl = null;
        public string Recruiter = null;
        public string Workplace = null;
        public string ApplicationEmail = null;
        public string ApplicationUrl = null;
        public string Description = null;
        public bool Applied { get; set; }
        public int Id { get; set; }
    }
}
