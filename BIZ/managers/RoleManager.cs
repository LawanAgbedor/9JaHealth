using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ.managers
{
    public class RoleManager
    {
        public static bool IsAdmin(Guid userId)
        {
            return HasAnyRole(userId, 3, 4);
        }

        private static bool HasAnyRole(Guid userId, params int[] roles)
        {
            var user = UserManager.GetByID(userId);
            var found = user.UserRoles.ToList().Any(u => roles.Contains(u.RoleValue.RoleId));
            return found;
        }

        private static bool HasAllRole(Guid userId, params int[] roles)
        {
            var user = UserManager.GetByID(userId);
            var found = user.UserRoles.ToList().All(u => roles.Contains(u.RoleValue.RoleId));
            return found;
        }
    }
}
