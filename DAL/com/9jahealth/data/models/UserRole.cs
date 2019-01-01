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
    public class UserRole : BaseCacheable<UserRole, int>, IInitializable
    {
        //[DataMember]
        public virtual int UserRoleId { get { return RecordId; } set { RecordId = value; } }
        public virtual string UserName { get; set; }
        //public virtual int RoleId { get; set; }
        public virtual Role RoleValue { get; set; }


        public virtual void InitializeNHibernate()
        {
            NHibernateHelper.InitializeNHibernate(RoleValue);
        }
    }
}
