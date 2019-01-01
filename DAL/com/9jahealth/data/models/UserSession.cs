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
    public class UserSession : BaseCacheable<UserSession, Guid>, IInitializable//, ISessionID
    {
        public virtual Guid UserSessionId { get { return RecordId; } set { RecordId = value; } }
        public virtual string UserId { get; set; }
        public virtual DateTime LastActivity { get; set; }
        public virtual bool KeepAlive { get; set; }
        public virtual string UserAgent { get; set; }
        public virtual string BrowserName { get; set; }
        public virtual string BrowserVersion { get; set; }    
        public virtual string OS_Name { get; set; }
        public virtual string OS_Version { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual void InitializeNHibernate()
        {
        }

        public virtual void UpdateLastActivity()
        {
            this.LastActivity = DateTime.Now;
        }

    }
}
