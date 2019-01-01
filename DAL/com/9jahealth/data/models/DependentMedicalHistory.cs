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
    public class DependentMedicalHistory : BaseCacheable<DependentMedicalHistory, long>, IInitializable, ISessionID
    {
        public virtual long DependentMedicalHistoryId { get { return RecordId; } set { RecordId = value; } }
        public virtual string MedicalHistory { get; set; }
        public virtual string SessionId { get; set; }

        public virtual Dependent DependentValue { get; set; }

        public virtual void InitializeNHibernate()
        {
            NHibernateHelper.InitializeNHibernate(DependentValue);
        }
    }
}
