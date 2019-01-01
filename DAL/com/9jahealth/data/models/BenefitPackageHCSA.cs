using DAL.com._9jahealth.data.models.bases;
using DAL.com._9jahealth.data.models.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.com._9jahealth.data.models
{
    public class BenefitPackageHCSA : BaseCacheable<BenefitPackageHCSA, int>, ISessionID
    {
        public virtual int BenefitPackageHCSAId { get { return RecordId; } set { RecordId = value; } }
        public virtual BenefitPackageCategory BenefitPackageCategoryValue { get; set; }
        public virtual string HealthServiceCareArea { get; set; }
        public virtual string SessionId { get; set; }
    }
}
