using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTIL;
using UTIL.interfaces;

namespace DAL.com._9jahealth.data.helpers
{
    public class NHibernateHelper
    {
        private static Logger<NHibernateHelper> logger = new Logger<NHibernateHelper>();

        private static ISessionFactory sessionFactory = null;
        private static object _lock = new object();
        private static object _dictLock = new object();

        private static ISessionFactory getSessionFactory()
        {
            lock (_dictLock)
            {
                if (sessionFactory != null)
                    return sessionFactory;

                var appConfigSettings = ConfigurationManager.AppSettings;

                var cs = ConnectionStringHelper.GetConnectionString();

                var defaultSchema = "dbo";
                var defaultBatchSize = "25";

                var showSql = appConfigSettings.Get("log4net_show_sql");
                if (String.IsNullOrEmpty(showSql))
                {
                    showSql = "false";
                }

                var config = new NHibernate.Cfg.Configuration();
                config.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                config.SetProperty("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                config.SetProperty("connection.connection_string", cs);
                config.SetProperty("default_schema", defaultSchema);
                config.SetProperty("dialect", "NHibernate.Dialect.MsSql2008Dialect");
                config.SetProperty("show_sql", showSql);
                config.SetProperty("current_session_context_class", "thread_static");
                config.SetProperty("adonet.batch_size", defaultBatchSize);
                config.AddAssembly("DAL");

                sessionFactory = config.BuildSessionFactory();

                return sessionFactory;
            }
        }


        public static ISession GetSession()
        {
            var factory = getSessionFactory();

            ISession sess;
            try
            {
                sess = factory.GetCurrentSession();
                if (sess.IsOpen == false)
                {
                    sess.Reconnect();
                }
            }
            catch (Exception)
            {
                sess = factory.OpenSession();
            }

            if (sess == null)
            {
                return factory.OpenSession();
            }
            return sess;
        }


        public static ISession GetNewSession()
        {
            var factory = getSessionFactory();
            return factory.OpenSession();
        }


        public static IStatelessSession GetStatelessSession()
        {
            var factory = getSessionFactory();
            return factory.OpenStatelessSession();
        }


        public static void InitializeNHibernate(object model)
        {
            try
            {
                //if (model != null && (!(model is ICollection) || (model is ICollection && ((ICollection)model).Count > 0)))
                if (model != null)
                {
                    NHibernateUtil.Initialize(model);

                    if (model is IInitializable)
                    {
                        ((IInitializable)model).InitializeNHibernate();
                    }
                    //else if (model is ICollection)
                    else if (model is IEnumerable)
                    {
                        //var collection = (ICollection)model;
                        var collection = (IEnumerable)model;
                        foreach (var c in collection)
                        {
                            if (c is IInitializable)
                            {
                                ((IInitializable)c).InitializeNHibernate();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var t = e.Message;
            }
        }
    }

    public static class Extensions
    {
        public static IQueryOver<troot, tsubtype> Or<troot, tsubtype>(this IQueryOver<troot, tsubtype> input, params ICriterion[] criteria)
        {
            if (criteria.Length == 0)
                return input;
            else if (criteria.Length == 1)
                return input.Where(criteria[0]);
            else
            {
                var or = Restrictions.Or(criteria[0], criteria[1]);
                for (int i = 2; i < criteria.Length; i++)
                    or = Restrictions.Or(or, criteria[i]);

                return input.Where(or);
            }
        }


        public static IList<dynamic> DynamicList(this IQuery query)
        {
            return query.SetResultTransformer(NhTransformers.ExpandoObject)
                        .List<dynamic>();
        }
    }
}


public static class NhTransformers
{
    public static readonly IResultTransformer ExpandoObject;

    static NhTransformers()
    {
        ExpandoObject = new ExpandoObjectResultSetTransformer();
    }

    private class ExpandoObjectResultSetTransformer : IResultTransformer
    {
        public IList TransformList(IList collection)
        {
            return collection;
        }

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            var expando = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expando;
            for (int i = 0; i < tuple.Length; i++)
            {
                string alias = aliases[i];
                if (alias != null)
                {
                    dictionary[alias] = tuple[i];
                }
            }
            return expando;
        }
    }
}

