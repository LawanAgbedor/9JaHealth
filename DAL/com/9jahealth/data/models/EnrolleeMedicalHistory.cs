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
    public class EnrolleeMedicalHistory : BaseCacheable<EnrolleeMedicalHistory, long>, IInitializable, ISessionID
    {
        public virtual long EnrolleeMedicalHistoryId { get { return RecordId; } set { RecordId = value; } }
        public virtual string MedicalHistory { get; set; }
        public virtual string SessionId { get; set; }

        public virtual Enrollee EnrolleeValue { get; set; }

        public virtual void InitializeNHibernate()
        {
            NHibernateHelper.InitializeNHibernate(EnrolleeValue);
        }
    }
}
