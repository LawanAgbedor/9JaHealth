using DAL.com._9jahealth.data.helpers;
using DAL.com._9jahealth.data.models.bases;
using DAL.com._9jahealth.data.models.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTIL.interfaces;

namespace DAL.com._9jahealth.data.models
{
    public class EmploymentDetail : BaseCacheable<EmploymentDetail, long>, IInitializable, ISessionID
    {
        public virtual long EmploymentDetailId { get { return RecordId; } set { RecordId = value; } }
        public virtual string EmployerType { get; set; }
        public virtual string Ministry { get; set; }
        public virtual string RankGL { get; set; }
        public virtual string RankStep { get; set; }
        public virtual string ControlNumber { get; set; }
        public virtual string FileNumber { get; set; }
        public virtual string Station { get; set; }
        public virtual string StationAddress { get; set; }
        public virtual DateTime AppointmentDate { get; set; }
        public virtual DateTime RetirementDate { get; set; }
        public virtual string BVNNumber { get; set; }
        public virtual DateTime TSDate { get; set; }
        public virtual string SessionId { get; set; }

        public virtual Enrollee EnrolleeValue { get; set; }

        public virtual void InitializeNHibernate()
        {
            NHibernateHelper.InitializeNHibernate(EnrolleeValue);
        }
    }
}
