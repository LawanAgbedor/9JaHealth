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
    public class GeneratedPINDetail : BaseCacheable<GeneratedPINDetail, long>, IInitializable, ISessionID
    {
        public virtual long GeneratedPINDetailId { get { return RecordId; } set { RecordId = value; } }
        public virtual long GeneratedBatchNo { get; set; }
        public virtual string PinValue { get; set; }
        public virtual int Status { get; set; }
        public virtual bool IsActived { get; set; }
        public virtual decimal Cost { get; set; }
        public virtual DateTime TSDate { get; set; }
        public virtual string SessionId { get; set; }

        public virtual void InitializeNHibernate()
        {
        }
    }
}
