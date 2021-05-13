using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAdReader {
    class SearchDay {
        public readonly DateTime Date;
        private readonly JobAd[] _programmerAds;
        private readonly JobAd[] _tattarjobbAds;

        public IReadOnlyCollection<JobAd> ProgrammerAds => _programmerAds;
        public IReadOnlyCollection<JobAd> TattarjobbAds => _tattarjobbAds;


        public SearchDay(DateTime date, JobAd[] programmerAds, JobAd[] tattarjobbAds) {
            Date = date;
            _programmerAds = programmerAds;
            _tattarjobbAds = tattarjobbAds;
        }
    }
}
