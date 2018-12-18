using DAL.com._9jahealth.data.bases.interfaces;
using DAL.com._9jahealth.data.helpers;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTIL;
using UTIL.interfaces;

namespace DAL.com._9jahealth.data.dao.bases
{
    public abstract class BaseDao<T, K> where T : class, ICacheable<T>, new()
    {
        protected Logger<T> logger = new Logger<T>();

        #region public static members
        public static T GetByID(K ID, bool useCache = false, bool saveToCache = true)
        {
            if (useCache)
            {
                var cache = CacheHelper.readFromCache<T>(ID);
                if (cache != null) return cache;
            }

            using (ISession session = NHibernateHelper.GetSession())
            {
                var res = session.Get<T>(ID);

                NHibernateHelper.InitializeNHibernate(res);

                if (res != null && saveToCache)
                    res.SaveToCache();

                return res;
            }
        }

        public static IList<T> GetAll(bool saveToCache = true)
        {
            using (ISession session = NHibernateHelper.GetSession())
            {
                var list = session.QueryOver<T>()
                    .List<T>();

                NHibernateHelper.InitializeNHibernate(list);

                if (saveToCache)
                    foreach (var l in list)
                        l.SaveToCache();

                return list;
            }
        }

        public static T SaveOrUpdate(T record, bool saveToCache = false)
        {
            if (record == null) return null;

            using (ISession session = NHibernateHelper.GetSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(record);
                transaction.Commit();

                if (record is IRecord<K>)
                {
                    record = GetByID(((IRecord<K>)record).RecordId);
                }

                //NHibernateHelper.InitializeNHibernate(record);

                if (record != null && saveToCache)
                    record.SaveToCache();

                return record;
            }
        }

        public static T Add(T record, bool saveToCache = true)
        {
            return Save(record, saveToCache);
        }

        public static T Save(T record, bool saveToCache = true)
        {
            if (record == null) return null;

            using (ISession session = NHibernateHelper.GetSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(record);
                transaction.Commit();

                if (record is IRecord<K>)
                {
                    record = GetByID(((IRecord<K>)record).RecordId);
                }

                //NHibernateHelper.InitializeNHibernate(record);

                if (record != null && saveToCache)
                    record.SaveToCache();

                return record;
            }
        }

        public static T Update(T record, bool saveToCache = true)
        {
            if (record == null) return null;

            using (ISession session = NHibernateHelper.GetSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(record);
                transaction.Commit();

                if (record is IRecord<K>)
                {
                    record = GetByID(((IRecord<K>)record).RecordId);
                }

                //NHibernateHelper.InitializeNHibernate(record);

                if (record != null && saveToCache)
                    record.SaveToCache();

                return record;
            }
        }

        public static T Delete(T record, bool deleteCache = true)
        {
            if (record == null) return null;

            if (record is IRecord<K>)
            {
                return DeleteByID(((IRecord<K>)record).RecordId);
            }
            else
            {
                using (ISession session = NHibernateHelper.GetSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(record);

                    transaction.Commit();

                    if (record != null && deleteCache && record.ExistsInCache())
                        record.RemoveFromCache();

                    return record;
                }
            }
        }

        public static T DeleteByID(K recordId, bool deleteCache = true, bool permanentDelete = false)
        {
            using (ISession session = NHibernateHelper.GetSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var record = GetByID(recordId);
                if (record != null)
                {
                    session.Delete(record);

                    transaction.Commit();

                    if (record != null && deleteCache && record.ExistsInCache())
                        record.RemoveFromCache();
                }
                return record;
            }
        }

        public static void DeleteList(IList<T> recordList)
        {
            if (recordList.Count > 0)
            {
                using (var session = NHibernateHelper.GetSession())
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var record in recordList)
                    {
                        session.Delete(record);
                    }

                    transaction.Commit();
                }
            }
        }

        public static IList<T> ExecuteHqlSelect(string sql)
        {
            using (ISession session = NHibernateHelper.GetSession())
            {
                var list = session.CreateQuery(sql)
                    .List<T>();

                NHibernateHelper.InitializeNHibernate(list);

                return list;
            }
        }

        public static bool ExecuteHqlDelete(string sql)
        {
            ISession session = NHibernateHelper.GetSession();
            ITransaction transaction = session.BeginTransaction();
            try
            {
                var query = session.CreateQuery(sql)
                    .ExecuteUpdate();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                session.Close();
            }
        }
        #endregion
    }

}
