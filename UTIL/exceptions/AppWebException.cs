using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UTIL.exceptions
{
    public enum AppWebExceptionType
    {
        DefaultError,
        ServerError,
        InvalidSession,
        InvalidParameter,
        RecordDoesNotExist,
        RecordAlreadyExists,
        RecordNotCreated,
        RecordNotSaved,
        RecordNotUpdated,
        RecordNotDeleted,
        InvalidOperation,
        InvalidUsernamePassword,
        MissingConfiguration
    }

    public static class AppWebExceptionTypeExtension
    {
        public static string GetMessage(this AppWebExceptionType type, string moreInfo = null)
        {
            string msg = Regex.Replace(type.ToString(), @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2");
            msg = Regex.Replace(msg, @"(\p{Ll})(\P{Ll})", "$1 $2");

            if (!string.IsNullOrWhiteSpace(moreInfo)) msg += ". " + moreInfo;

            return msg;
        }

        public static HttpStatusCode GetHttpCode(this AppWebExceptionType type)
        {
            switch (type)
            {
                case AppWebExceptionType.MissingConfiguration:
                case AppWebExceptionType.RecordNotDeleted:
                case AppWebExceptionType.RecordNotCreated:
                case AppWebExceptionType.RecordNotUpdated:
                case AppWebExceptionType.RecordNotSaved:
                case AppWebExceptionType.ServerError:
                    return HttpStatusCode.InternalServerError;
                case AppWebExceptionType.RecordDoesNotExist:
                    return HttpStatusCode.NotFound;
                case AppWebExceptionType.InvalidParameter:
                    return HttpStatusCode.BadRequest;
                case AppWebExceptionType.RecordAlreadyExists:
                    return HttpStatusCode.Found;
                case AppWebExceptionType.InvalidOperation:
                    return HttpStatusCode.Forbidden;
                case AppWebExceptionType.InvalidSession:
                case AppWebExceptionType.InvalidUsernamePassword:
                    return HttpStatusCode.Unauthorized;
                default:
                    return HttpStatusCode.BadRequest;
            }

        }
    }

    public class AppWebException : BaseException<AppWebException>
    {

        public AppWebException(
            string message
            , LoggerData loggerData = null
            , Exception exception = null
        )
            : base(message, HttpStatusCode.BadRequest, exception)
        {
            Log.Fatal(ExceptionMessage, loggerData, exception ?? this);
        }


        public AppWebException(
            AppWebExceptionType error
            , HttpStatusCode status
            , string customMessage
            , LoggerData loggerData = null
            , Exception exception = null
        )
            : base(error.GetMessage() + ": " + customMessage, status, exception)
        {
            Log.Fatal(ExceptionMessage, loggerData, exception ?? this);
        }


        public AppWebException(
            AppWebExceptionType error
            , string customMessage
            , LoggerData loggerData = null
            , Exception exception = null
        )
            : base(error.GetMessage() + ": " + customMessage, error.GetHttpCode(), exception)
        {
            Log.Fatal(ExceptionMessage, loggerData, exception ?? this);
        }


        public AppWebException(
            AppWebExceptionType error
            , LoggerData loggerData = null
            , Exception exception = null
        )
            : base(error.GetMessage(), error.GetHttpCode(), exception)
        {
            Log.Fatal(ExceptionMessage, loggerData, exception ?? this);
        }


        public AppWebException(
            string message
            , HttpStatusCode status
            , LoggerData loggerData = null
            , Exception exception = null
        )
            : base(message, status, exception)
        {
            Log.Fatal(ExceptionMessage, loggerData, exception ?? this);
        }

    }
}
