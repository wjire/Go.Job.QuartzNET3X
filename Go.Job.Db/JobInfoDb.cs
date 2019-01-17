using Go.Job.Model;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Go.Job.Db
{
    public static class JobInfoDb
    {
        private static readonly string _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        private static ConnectionConfig config;

        static JobInfoDb()
        {
            config = new ConnectionConfig
            {
                ConnectionString = _connStr,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            };
        }


        public static List<JobInfo> GetJobInfoList()
        {
            List<JobInfo> list = null;
            var sql = " select  ";
            using (SqlSugarClient db = new SqlSugarClient(config))
            {
                list = db.Queryable<JobInfo>().ToList();
            }
            return list;
        }

        public static JobInfo GetJobInfo(int id)
        {
            try
            {
                using (SqlSugarClient db = new SqlSugarClient(config))
                {
                    return db.Queryable<JobInfo>().Where(w => w.Id == id).First();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static int AddJobInfo(JobInfo jobInfo)
        {
            using (SqlSugarClient db = new SqlSugarClient(config))
            {
                return db.Insertable(jobInfo).ExecuteCommand();
            }
        }


        public static int UpdateJobInfo(JobInfo jobInfo)
        {
            var res = 0;
            try
            {
                using (SqlSugarClient db = new SqlSugarClient(config))
                {
                    res = db.Updateable(jobInfo).ExecuteCommand();
                }
            }
            catch (Exception e)
            {

            }
            return res;
        }


        public static int UpdateJobState(JobInfo jobInfo)
        {
            var res = 0;
            try
            {
                using (SqlSugarClient db = new SqlSugarClient(config))
                {
                    res = db.Updateable(jobInfo).UpdateColumns(s => new { s.State, s.Id })
                        .WhereColumns(w => new { w.Id }).ExecuteCommand();
                }
            }
            catch (Exception e)
            {

            }
            return res;
        }
    }
}

