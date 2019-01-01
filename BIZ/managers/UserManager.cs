using DAL.com._9jahealth.data.dao;
using DAL.com._9jahealth.data.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ.managers
{
    public class UserManager
    {
        public static User GetByUserName(string userName)
        {
            var record = UserDao.GetByUserName(userName);
            return record;
        }

        public static User GetByID(Guid userId)
        {
            var record = UserDao.GetByID(userId);
            return record;
        }
    }
}
