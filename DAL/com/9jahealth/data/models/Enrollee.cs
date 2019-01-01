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
    public class Enrollee : BaseCacheable<Enrollee, long>, IInitializable, ISessionID
    {
        public virtual long EnrolleeId { get { return RecordId; } set { RecordId = value; } }
        public virtual string EnrolleeNumber { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Surname { get; set; }
        public virtual string OtherNames { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
        public virtual int Age { get; set; }
        public virtual string Gender { get; set; }
        public virtual string BloodGroup { get; set; }
        public virtual string MaritalStatus { get; set; }
        public virtual string ResidentAddress { get; set; }
        public virtual int LGAId { get; set; }
        public virtual DateTime TSDate { get; set; }
        public virtual int EnrolleeType { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string Email { get; set; }
        public virtual long GeneratedPinId { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string SessionId { get; set; }

        public virtual void InitializeNHibernate()
        {
        }
    }
}
