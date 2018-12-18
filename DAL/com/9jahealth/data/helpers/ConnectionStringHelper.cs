using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTIL;

namespace DAL.com._9jahealth.data.helpers
{
    public class ConnectionStringHelper
    {
        private static readonly string ACTIVE_CONNECTION_STRING = "ActiveConnectionString";

        public static string GetConnectionString(string connectionStringName = null)
        {
            string cs;

            if (string.IsNullOrWhiteSpace(connectionStringName))
                cs = ConfigHelper.GetConnectionString(ActiveConnectionStringName);
            else
                cs = ConfigHelper.GetConnectionString(connectionStringName);

            return cs;
        }


        //public static string GetProviderName(string connectionStringName = null)
        //{
        //    if (string.IsNullOrWhiteSpace(connectionStringName))
        //        return ConfigurationManager.ConnectionStrings[ActiveConnectionStringName].ProviderName;
        //    else
        //        return ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
        //}


        private static string ActiveConnectionStringName
        {
            get
            {
                var activeConn = ConfigHelper.GetValue(ACTIVE_CONNECTION_STRING);

                if (string.IsNullOrWhiteSpace(activeConn))
                {
                    return activeConn.Trim();
                }

                throw new Exception($"AppSetting [{ACTIVE_CONNECTION_STRING}] is not defined. Please add to WebConfig.");
            }
        }
    }
}
