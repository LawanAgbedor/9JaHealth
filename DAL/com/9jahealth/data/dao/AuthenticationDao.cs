using DAL.com._9jahealth.data.dao.bases;
using DAL.com._9jahealth.data.helpers;
using DAL.com._9jahealth.data.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.com._9jahealth.data.dao
{
    public class AuthenticationDao : BaseDao<User, Guid>
    {
        public static Guid? AuthenticateUser(string username, string password)
        {
            var user = UserDao.GetByUserName(username);
            if (user != null && user.PasswordHash == password)
                return user.UserId;

            return null;
        }

        public static UserSession GetUserSession(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) return null;

            var sessId = Guid.Parse(sessionId);

            using (var session = NHibernateHelper.GetSession())
            {
                var record = session.QueryOver<UserSession>()
                    .Where(u => u.UserSessionId == sessId)
                    .SingleOrDefault();

                record.InitializeNHibernate();

                return record;
            }
        }

        public static UserSession SaveOrUpdateUserSession(UserSession userSession)
        {
            using (var session = NHibernateHelper.GetSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(userSession);
                transaction.Commit();
                return userSession;
            }
        }

        public static void DeleteUserSession(UserSession userSession)
        {
            using (var session = NHibernateHelper.GetSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(userSession);
                transaction.Commit();
            }
        }
    }
}
