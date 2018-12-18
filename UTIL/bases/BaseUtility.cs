using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTIL.bases
{
    public abstract class BaseUtility<T> where T : class
    {
        protected static Logger<T> logger = new Logger<T>();
    }
}
