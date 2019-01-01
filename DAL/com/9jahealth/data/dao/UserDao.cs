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
    public class UserDao : BaseDao<User, Guid>
    {
        public static User GetByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return null;

            using (var session = NHibernateHelper.GetSession())
            {
                var record = session.QueryOver<User>()
                    .Where(c => c.UserEmail == userName.Trim())
                    .SingleOrDefault();

                if (record != null)
                    record.InitializeNHibernate();

                return record;
            }
        }
    }
}
