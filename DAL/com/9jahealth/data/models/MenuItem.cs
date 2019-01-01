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
    public class MenuItem : BaseCacheable<MenuItem, int>, IInitializable
    {
        public virtual int MenuItemId { get { return RecordId; } set { RecordId = value; } }
        public virtual string MenuItemName { get; set; }
        public virtual string MenuItemUrl { get; set; }

        public virtual Menu MenuValue { get; set; }

        public virtual void InitializeNHibernate()
        {
            NHibernateHelper.InitializeNHibernate(MenuValue);
        }
    }
}
