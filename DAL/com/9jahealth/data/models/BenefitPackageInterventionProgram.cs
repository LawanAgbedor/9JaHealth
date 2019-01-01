using DAL.com._9jahealth.data.models.bases;
using DAL.com._9jahealth.data.models.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.com._9jahealth.data.models
{
    public class BenefitPackageInterventionProgram : BaseCacheable<BenefitPackageInterventionProgram, int>, ISessionID
    {
        public virtual int BenefitPackageInterventionProgramId { get { return RecordId; } set { RecordId = value; } }
        public virtual BenefitPackageHCSA BenefitPackageHCSAValue { get; set; }
        public virtual string ProgramName { get; set; }
        public virtual string Comment { get; set; }
        public virtual string SessionId { get; set; }
    }
}
