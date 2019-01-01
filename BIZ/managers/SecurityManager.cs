using DAL.com._9jahealth.data.dao;
using DAL.com._9jahealth.data.models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UTIL;
using UTIL.exceptions;

namespace BIZ.managers
{
    public class SecurityManager
    {
        private static readonly Logger<SecurityManager> Log = new Logger<SecurityManager>();

        private static readonly string X_AUTH_TOKEN_NAME;
        private static readonly string COOKIE_DOMAIN_NAME;
        private static readonly string SESSION_ID_COOKIE_NAME;

        private static readonly string DEFAULT_X_AUTH_TOKEN_NAME = "X-Auth-Token";
        private static readonly string DEFAULT_SESSION_ID_COOKIE_NAME = "_kshcma_sessionid_cookie";

        static SecurityManager()
        {
            X_AUTH_TOKEN_NAME = ConfigurationManager.AppSettings.Get("X_AUTH_TOKEN_NAME") ?? DEFAULT_X_AUTH_TOKEN_NAME;
            SESSION_ID_COOKIE_NAME = ConfigurationManager.AppSettings.Get("SESSION_ID_COOKIE_NAME") ?? DEFAULT_SESSION_ID_COOKIE_NAME;

            COOKIE_DOMAIN_NAME = ConfigurationManager.AppSettings.Get("COOKIE_DOMAIN_NAME");
            if (string.IsNullOrWhiteSpace(COOKIE_DOMAIN_NAME) && !ConfigHelper.IsLocal())
                throw new AppWebException(AppWebExceptionType.MissingConfiguration, "[COOKIE_DOMAIN_NAME] Not Found in AppSettings.", null, null);
        }

        #region Public Methods

        #region Check Login
        public static string CheckLoginNoSession(HttpContext context)
        {
            string sessId = GetXAuthToken(context.Request.Headers);

            if (sessId == null && context.Request.Cookies[SESSION_ID_COOKIE_NAME] != null)
                sessId = context.Request.Cookies[SESSION_ID_COOKIE_NAME].Value;

            if (string.IsNullOrEmpty(sessId))
                throw new AppWebException("No Session ID Found", HttpStatusCode.Unauthorized);

            var session = AuthenticationDao.GetUserSession(sessId);
            if (session != null)
            {
                return session.UserId;
            }

            throw new AppWebException("No Session Found", HttpStatusCode.Unauthorized);
        }


        public static Guid? CheckLogin(string sessionId)
        {
            if (sessionId == null)
                return CheckLogin();

            if (_IsValid(sessionId))
                return GetCurrentUserId_PreAuthenticated();

            throw new AppWebException("Check Login (sessionId)", HttpStatusCode.Unauthorized, new LoggerData() { { "sessionId", sessionId } });
        }


        public static Guid? CheckLogin()
        {
            if (IsValid())
                 return GetCurrentUserId_PreAuthenticated();

            throw new AppWebException("Check Login", System.Net.HttpStatusCode.Unauthorized);
        }
        #endregion


        #region Get AccountIDs
        public static Guid? IsLoggedIn()
        {
            Guid? userId = GetCurrentUserId_PreAuthenticated();
            if (!userId.HasValue)
            {
                if (IsValid())
                    return AuthenticatedSession.Current.UserId;
            }
            else
                return userId;
            return null;
        }

        public static Guid? IsLoggedIn(HttpContext context)
        {
            return IsLoggedIn(context.Request.Headers, context.Request.Cookies);
        }

        public static Guid? IsLoggedIn(HttpContextBase context)
        {
            return IsLoggedIn(context.Request.Headers, context.Request.Cookies);
        }

        private static Guid? IsLoggedIn(System.Collections.Specialized.NameValueCollection headers, HttpCookieCollection cookies)
        {
            string sessionId = GetXAuthToken(headers);

            if (string.IsNullOrWhiteSpace(sessionId) && cookies[SESSION_ID_COOKIE_NAME] != null)
                sessionId = cookies[SESSION_ID_COOKIE_NAME].Value;

            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                var session = AuthenticationDao.GetUserSession(sessionId);
                if (session != null)
                    return Guid.Parse(session.UserId);
            }
            return null;
        }


        public static Guid GetCurrentUserId(string sessionId)
        {
            if (sessionId == null)
                return GetCurrentUserId();

            if (_IsValid(sessionId))
            {
                return AuthenticatedSession.Current.UserId.Value;
            }

            throw new AppWebException("Get Current User Id by (sessionId)", HttpStatusCode.Unauthorized, new LoggerData() { { "sessionId", sessionId } });
        }


        public static Guid GetCurrentUserId()
        {
            var userId = GetCurrentUserId_PreAuthenticated();

            if (userId.HasValue && !userId.Value.Equals(Guid.Empty))
            {
                return userId.Value;
            }
            else
            {
                if (IsValid())
                {
                    return AuthenticatedSession.Current.UserId.Value;
                }
            }

            throw new AppWebException("Get Current User Id", HttpStatusCode.Unauthorized);
        }


        public static string GetCurrentUserName()
        {
            var userId = GetCurrentUserId_PreAuthenticated();
            if (userId.HasValue)
            {
                var user = UserDao.GetByID(userId.Value);
                if (user != null)
                {
                    return user.UserName;
                }
            }
            return null;
        }


        private static Guid? GetCurrentUserId_PreAuthenticated()
        {
            var userId = AuthenticatedSession.Current.UserId;
            if (userId.HasValue)
            {
                return userId;
            }
            return null;
        }
        #endregion


        public static bool IsValid(string sessionId = null)
        {
            if (sessionId == null)
            {
                sessionId = GetSessionCookieValue();
            }
            return _IsValid(sessionId);
        }


        private static bool _IsValid(string sessionId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sessionId) == true)
                {
                    DeleteAndInvalidateOldSessions();
                    return false;
                }

                UserSession userSession = AuthenticationDao.GetUserSession(sessionId);
                if (userSession == null)
                {
                    DeleteAndInvalidateOldSessions();
                    return false;
                }

                var loggerData = new LoggerData()
                {
                    {"userId", userSession.UserId},
                    {"userSessionId", sessionId},
                    {"userAgent", userSession.UserAgent},
                    {"browserName", userSession.BrowserName},
                    {"operatingSystem", userSession.OS_Name},
                    {"operatingSystemVersion", userSession.OS_Version},
                    {"lastActivity", userSession.LastActivity},
                };

                Log.Info("Possible bad session check", loggerData);

                if (userSession != null)
                {
                    userSession.UpdateLastActivity();
                    AuthenticationDao.SaveOrUpdateUserSession(userSession);
                }

                SetServerSessionAsValidated(userSession.UserSessionId, Guid.Parse(userSession.UserId));

                if (userSession.KeepAlive)
                    SetSessionCookie(userSession.UserSessionId.ToString(), true);
            }
            catch (Exception e)
            {
                Log.Fatal("An error occurred Validating Session", null, e);
                return false;
            }

            return true;
        }


        public static UserSession OnLogin(string userId, bool keepAlive, HttpRequestBase request = null)
        {
            return OnLogin(userId, keepAlive, null, request);
        }


        public static UserSession OnLogin(string userId, bool keepAlive, UserSession existingSession, HttpRequestBase request)
        {
            if (existingSession == null)
            {
                DeleteAndInvalidateOldSessions();
            }

            var session = existingSession ?? new UserSession();
            session.UserId = userId;
            session.KeepAlive = keepAlive;

            if (request != null)
            {
                var userAgent = request.UserAgent;
                var browser = request.Browser.Browser;
                var ipAddress = request.UserHostAddress;

                var os = request.Browser.Platform;
                if (String.IsNullOrEmpty(os) || os.ToLower() == "unknown")
                {
                    os = _GetOSfromUserAgent(userAgent);
                }

                session.UserAgent = userAgent;
                session.BrowserName = browser;
                session.BrowserVersion = request.Browser.MajorVersion.ToString();
                session.OS_Name = os;
            }
            else if (existingSession != null)
            {
                session.UserAgent = existingSession.UserAgent;
                session.BrowserName = existingSession.BrowserName;
                session.BrowserVersion = existingSession.BrowserVersion;
                session.OS_Name = existingSession.OS_Name;
            }

            //AccountDao.MarkAccountAsVerified(userId);

            AuthenticationDao.SaveOrUpdateUserSession(session);

            //Set the server's Session properties
            SetServerSessionAsValidated(session.UserSessionId, Guid.Parse(session.UserId));

            //Set the Cookies
            SetSessionCookie(session.UserSessionId.ToString(), keepAlive);

            return session;
        }


        private static String _GetOSfromUserAgent(String userAgent)
        {
            var os = "";

            if (userAgent.ToLower().Contains("windows"))
            {
                os = "Windows";
            }
            else if (userAgent.ToLower().Contains("mac"))
            {
                os = "Mac";
            }
            else if (userAgent.ToLower().Contains("x11"))
            {
                os = "Unix";
            }
            else if (userAgent.ToLower().Contains("android"))
            {
                os = "Android";
            }
            else if (userAgent.ToLower().Contains("ios"))
            {
                os = "iOS";
            }
            else if (userAgent.ToLower().Contains("blackBerry"))
            {
                os = "blackberry";
            }
            else if (userAgent.ToLower().Contains("cros"))
            {
                os = "Chromium OS";
            }
            else
            {
                os = "Unknown";
            }

            return os;
        }


        private static String _GetDevicefromUserAgent(String userAgent)
        {
            var deviceType = "";
            if (userAgent.ToLower().Contains("ipad"))
            {
                deviceType = "iPad";
            }
            else if (userAgent.ToLower().Contains("iphone"))
            {
                deviceType = "IPhone";
            }
            else if (userAgent.ToLower().Contains("kindle"))
            {
                deviceType = "Kindle";
            }
            else if (userAgent.ToLower().Contains("samsung"))
            {
                deviceType = "Samsung";
            }
            else if (userAgent.ToLower().Contains("lg"))
            {
                deviceType = "LG";
            }
            else if (userAgent.ToLower().Contains("motorola"))
            {
                deviceType = "Motorola";
            }
            else if (userAgent.ToLower().Contains("nokia"))
            {
                deviceType = "Nokia";
            }
            else
            {
                deviceType = "Unknown";
            }

            return deviceType;
        }


        public static void OnLogout(Guid? userId = null)
        {
            if (userId == null)
            {
                try
                {
                    userId = GetCurrentUserId();
                }
                catch
                {
                    return;
                }
            }

            try
            {
                DeleteAndInvalidateOldSessions();
            }
            catch (Exception e)
            {
                Log.Fatal("An error occurred Deleting And Invalidating Old Sessions OnLogout", null, e);
            }

        }


        public static void DeleteAndInvalidateOldSessions()
        {
            //Check to see if the user has an old cookie.
            String oldSessionId = GetSessionCookieValue();
            if (oldSessionId != null)
            {
                var userSession = AuthenticationDao.GetUserSession(oldSessionId);
                if (userSession != null)
                {
                    AuthenticationDao.DeleteUserSession(userSession);
                }
            }

            RemoveSessionCookie();

            if (AuthenticatedSession.Current.UserSessionId.HasValue)
            {
                if (oldSessionId == null || !AuthenticatedSession.Current.UserSessionId.Equals(oldSessionId))
                {
                    var userSession = AuthenticationDao.GetUserSession(AuthenticatedSession.Current.UserSessionId.Value.ToString());
                    if (userSession != null)
                    {
                        AuthenticationDao.DeleteUserSession(userSession);
                    }
                }
            }

            //Invalidate any old Session
            SetServerSessionAsInvalidated();
        }

        #endregion


        #region Cookies

        private static string GetXAuthToken(HttpRequestBase request)
        {
            return GetXAuthToken(request.Headers);
        }

        private static string GetXAuthToken(System.Collections.Specialized.NameValueCollection headers)
        {
            if (headers == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(headers[X_AUTH_TOKEN_NAME]) == false)
            {
                var token = headers[X_AUTH_TOKEN_NAME];
                if (token != "undefined")
                {
                    return token;
                }
            }
            return null;
        }

        public static String GetSessionCookieValue()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }

            var request = HttpContext.Current.Request;

            var token = GetXAuthToken(request.Headers);
            if (token != null)
            {
                return token;
            }

            var sessionCookie = request.Cookies.Get(SESSION_ID_COOKIE_NAME);
            if (sessionCookie != null)
            {
                return sessionCookie.Value;
            }
            return null;
        }

        private static void SetSessionCookie(String sessionId, bool KeepAlive)
        {
            if (HttpContext.Current == null)
            {
                Log.Fatal("No HTTP CONTEXT!!", null, null);
                return;
            }

            var response = HttpContext.Current.Response;

            response.AddHeader(X_AUTH_TOKEN_NAME, sessionId);

            var cookie = new HttpCookie(SESSION_ID_COOKIE_NAME, sessionId);
            cookie.Path = "/";

            var url = HttpContext.Current.Request.Url.ToString();
            if (!string.IsNullOrWhiteSpace(COOKIE_DOMAIN_NAME))
            {
                cookie.Domain = $".{COOKIE_DOMAIN_NAME}";
            }

            if (KeepAlive)
                cookie.Expires = DateTime.Now.AddDays(30);

            response.Cookies.Add(cookie);
        }

        private static void RemoveSessionCookie()
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            var response = HttpContext.Current.Response;
            var cookie = new HttpCookie(SESSION_ID_COOKIE_NAME, "");
            cookie.Path = "/";
            cookie.Domain = ".medstudy.com";
            cookie.Expires = DateTime.Now.AddDays(-1d);
            response.Cookies.Add(cookie);
        }

        #endregion


        #region Server Sessions

        private static void SetServerSessionAsValidated(Guid sessionId, Guid userId)
        {
            AuthenticatedSession.Current.UserSessionId = sessionId;
            AuthenticatedSession.Current.LastDBCheck = DateTime.Now;
            AuthenticatedSession.Current.UserId = userId;
        }

        private static void SetServerSessionAsInvalidated()
        {
            AuthenticatedSession.Current.UserSessionId = null;
            AuthenticatedSession.Current.LastDBCheck = null;
            AuthenticatedSession.Remove();
        }

        #endregion

    }


    #region Authentiatedsession Class

    public class AuthenticatedSession
    {
        private AuthenticatedSession()
        {
        }

        // Gets the current session.
        public static AuthenticatedSession Current
        {
            get
            {
                AuthenticatedSession session = (AuthenticatedSession)HttpContext.Current.Session["AUTHENTICATION_SESSION"];
                if (session == null)
                {
                    session = new AuthenticatedSession();
                    HttpContext.Current.Session["AUTHENTICATION_SESSION"] = session;
                }
                return session;
            }
        }


        public static void Remove()
        {
            try
            {
                HttpContext.Current.Session.Remove("AUTHENTICATION_SESSION");
            }
            catch
            {

            }
        }

        public Guid? UserSessionId { get; set; }
        public DateTime? LastDBCheck { get; set; }
        public Guid? UserId { get; set; }

        public bool isValid()
        {
            return UserId.HasValue && !UserId.Value.Equals(Guid.Empty) && UserSessionId != null;
        }

     }

    #endregion Authentiationsession Class

}
