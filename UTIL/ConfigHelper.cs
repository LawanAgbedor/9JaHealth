using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTIL
{

    public class ConfigHelper
    {
        public enum RunEnvironment
        {
            local,
            development,
            staging,
            production
        }


        private static NameValueCollection appSettings = ConfigurationManager.AppSettings;
        private static ConnectionStringSettingsCollection connSettings = ConfigurationManager.ConnectionStrings;

        public static string GetConnectionString(string connectionName)
        {
            if (string.IsNullOrWhiteSpace(connectionName))
                throw new Exception("");

            var conn = connSettings[connectionName];
            if (conn == null)
                throw new Exception("");

            return conn.ConnectionString;
        }

        public static string GetValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("");

            var value = appSettings.Get(key);

            return value;
        }

        public static T GetValue<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("");

            var value = (object)appSettings.Get(key);

            return (T)value;
        }

        public static bool IsLocal()
        {
            return IsEnv(RunEnvironment.local);
        }

        public static bool IsProduction()
        {
            return IsEnv(RunEnvironment.production);
        }

        private static bool IsEnv(RunEnvironment env)
        {
            return GetValue("ENVIRONMENT").IndexOf(env.ToString(), StringComparison.InvariantCultureIgnoreCase) > -1;
        }
    }
}
