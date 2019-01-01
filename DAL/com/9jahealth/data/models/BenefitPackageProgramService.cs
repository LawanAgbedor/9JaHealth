using DAL.com._9jahealth.data.models.bases;
using DAL.com._9jahealth.data.models.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.com._9jahealth.data.models
{
    public class BenefitPackageProgramService : BaseCacheable<BenefitPackageProgramService, int>, ISessionID
    {
        public virtual int BenefitPackageProgramServiceId { get { return RecordId; } set { RecordId = value; } }
        public virtual BenefitPackageInterventionProgram BenefitPackageInterventionProgramValue { get; set; }
        public virtual string ProgramServiceName { get; set; }
        public virtual string SessionId { get; set; }
    }
}
