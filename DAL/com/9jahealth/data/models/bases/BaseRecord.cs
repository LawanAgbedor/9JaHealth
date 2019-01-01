using DAL.com._9jahealth.data.bases.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.com._9jahealth.data.models.bases
{
    public abstract class BaseRecord<K> : IRecord<K>
    {
        private K _RecordId;
        public virtual K RecordId { get { return _RecordId; } set { _RecordId = value; } }
    }
}
