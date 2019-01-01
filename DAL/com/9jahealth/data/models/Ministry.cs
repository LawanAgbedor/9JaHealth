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
    public class Ministry : BaseCacheable<Ministry, int>, IInitializable
    {
        public virtual int MinistryId { get { return RecordId; } set { RecordId = value; } }
        public virtual string MinistryName { get; set; }
        public virtual string SessionId { get; set; }

        public virtual void InitializeNHibernate()
        {
        }
    }
}
