using DAL.com._9jahealth.data.models.bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.com._9jahealth.data.models
{
    public class Role : BaseCacheable<Role, int>
    {
        public virtual int RoleId { get { return RecordId; } set { RecordId = value; } }
        public virtual string Name { get; set; }
    }
}
