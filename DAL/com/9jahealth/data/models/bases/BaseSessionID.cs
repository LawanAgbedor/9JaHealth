using DAL.com._9jahealth.data.models.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.com._9jahealth.data.models.bases
{
    public abstract class BaseSessionID: ISessionID
    {
        public string SessionId { get; set; }
    }
}
