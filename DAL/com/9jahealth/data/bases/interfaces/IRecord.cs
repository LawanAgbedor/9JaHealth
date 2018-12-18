using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.com._9jahealth.data.bases.interfaces
{
    public interface IRecord<T>
    {
        T RecordId { get; set; }
    }
}
