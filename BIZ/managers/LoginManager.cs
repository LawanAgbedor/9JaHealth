using DAL.com._9jahealth.data.dao;
using DAL.com._9jahealth.data.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UTIL;
using UTIL.exceptions;

namespace BIZ.managers
{
    public class LoginManager
    {
        public static Guid AuthenticateUser(string username, string password, bool createSession = true)
        {
            var userId = AuthenticationDao.AuthenticateUser(username, password);

            if (!userId.HasValue)
                throw new AppWebException(AppWebExceptionType.InvalidUsernamePassword, new LoggerData() { { "username", username }, { "password", password } });

            if (createSession)
                SecurityManager.OnLogin(userId.Value.ToString(), true);

            return userId.Value;
        }

        public static Guid AuthenticateAdminForUser(string adminUsername, string adminPassword, string username)
        {
            var adminUserId = AuthenticateUser(adminUsername, adminPassword, false);

            if (!RoleManager.IsAdmin(adminUserId))
                throw new AppWebException(AppWebExceptionType.InvalidUsernamePassword, new LoggerData() { { "adminUsername", adminUsername }, { "adminPassword", adminPassword }, { "username", username } });

            var user = UserDao.GetByUserName(username);
            if (user == null)
                throw new AppWebException(AppWebExceptionType.RecordDoesNotExist, $"User with Email {username} Not Found.", new LoggerData() { { "adminUsername", adminUsername }, { "username", username } });

            SecurityManager.OnLogin(user.UserId.ToString(), true);

            return user.UserId;
        }

    }
}
