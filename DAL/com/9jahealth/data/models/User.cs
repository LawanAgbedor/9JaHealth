using DAL.com._9jahealth.data.helpers;
using DAL.com._9jahealth.data.models.bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTIL.interfaces;

namespace DAL.com._9jahealth.data.models
{
    public class User : BaseCacheable<User, Guid>, IInitializable
    {
        public virtual Guid UserId { get { return RecordId; } set { RecordId = value; } }
        public virtual int UserProfileInfoUserSource { get; set; }
        public virtual string UserEmail { get; set; }
        public virtual bool UserEmailConfirmed { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual DateTime LockoutEndDateUtc { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }
        public virtual string UserName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual void InitializeNHibernate()
        {
            NHibernateHelper.InitializeNHibernate(UserRoles);
        }
    }
}
