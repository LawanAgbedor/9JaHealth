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
    public class Menu : BaseCacheable<Menu, int>, IInitializable
    {
        public virtual int MenuId { get { return RecordId; } set { RecordId = value; } }
        public virtual string MenuName { get; set; }

        public virtual void InitializeNHibernate()
        {
        }
    }
}
