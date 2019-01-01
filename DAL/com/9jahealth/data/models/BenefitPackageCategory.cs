using DAL.com._9jahealth.data.models.bases;
using DAL.com._9jahealth.data.models.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.com._9jahealth.data.models
{
    public class BenefitPackageCategory : BaseCacheable<BenefitPackageCategory, int>, ISessionID
    {
        public virtual int BenefitPackageCategoryId { get { return RecordId; } set { RecordId = value; } }
        public virtual string PackageName { get; set; }
        public virtual string SessionId { get; set; }
    }
}
