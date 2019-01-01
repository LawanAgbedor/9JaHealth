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
    public class Dependent : BaseCacheable<Dependent, long>, IInitializable, ISessionID
    {
        public virtual long DependentId { get { return RecordId; } set { RecordId = value; } }
        public virtual string DependentName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Surname { get; set; }
        public virtual string OtherNames { get; set; }
        public virtual string DependentEnrollmentNumber { get; set; }
        public virtual string Gender { get; set; }
        public virtual string BloodGroup { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
        public virtual DateTime RegistrationDate { get; set; }
        public virtual DateTime ExpirationDate { get; set; }
        public virtual int DependentType { get; set; }
        public virtual string SessionId { get; set; }

        public virtual Enrollee EnrolleeValue { get; set; }

        public virtual void InitializeNHibernate()
        {
            NHibernateHelper.InitializeNHibernate(EnrolleeValue);
        }
    }
}
