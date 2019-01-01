using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace UTIL.exceptions
{
    public abstract class BaseException<T> : WebFaultException<string> where T: class
    {
        protected static readonly Logger<T> Log = new Logger<T>();

        private string _message;

        protected BaseException(string detail, HttpStatusCode statusCode, Exception exception = null)
                : base(detail, statusCode)
        {
            BuildExceptionMessage(exception, detail);
        }

        //protected BaseException(string detail, HttpStatusCode statusCode, IEnumerable<Type> knownTypes)
        //    : base(detail, statusCode, knownTypes)
        //{
        //    throw new NotImplementedException();
        //}

        //protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //    throw new NotImplementedException();
        //}

        protected string ExceptionMessage
        {
            get { return _message; }
            private set { _message = value; }
        }

        private string BuildExceptionMessage(Exception exception, string message)
        {
            return _message = $"{message} : {BuildExceptionMessage(exception)}";
        }

        private static string BuildExceptionMessage(Exception exception)
        {
            if (exception == null) return "";

            return $"{exception.Message} : {BuildExceptionMessage(exception.InnerException)}";
        }

    }
}
