using System;
using System.Collections.Generic;
using Go.Job.Model;
using SqlSugar;

namespace Go.Job.Db
{
    public static class JobInfoDb
    {
        private static readonly string _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        private static readonly ConnectionConfig Config;

        private static string columns = " a.Id,a.JobName,a.JobGroup,a.Cron,a.AssemblyPath,a.ClassType,a.State,a.SchedName,b.TRIGGER_STATE,b.NEXT_FIRE_TIME,b.PREV_FIRE_TIME,b.START_TIME ";

        static JobInfoDb()
        {
            Config = new ConnectionConfig
            {
                ConnectionString = _connStr,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                //InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            };
        }


        public static List<JobInfo> GetJobInfoList()
        {

            List<JobInfo> list = null;

            using (SqlSugarClient db = new SqlSugarClient(Config))
            {
                //list = db.Queryable<JobInfo>().ToList();

                list = db.SqlQueryable<JobInfo>($"select {columns} from jobinfo as a left join qrtz_triggers as b on a.SchedName=b.SCHED_NAME ").ToList();
            }
            return list;
        }

        public static JobInfo GetJobInfo(int id)
        {
            try
            {
                using (SqlSugarClient db = new SqlSugarClient(Config))
                {
                   return db.SqlQueryable<JobInfo>(
                            $"select {columns} from jobinfo as a left join qrtz_triggers as b on a.SchedName=b.SCHED_NAME where a.Id = {id}")
                        .Single();
                    //return db.Queryable<JobInfo>().Where(w => w.Id == id).First();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static int AddJobInfo(JobInfo jobInfo)
        {
            jobInfo.CreateTime = DateTime.Now;
            using (SqlSugarClient db = new SqlSugarClient(Config))
            {
                return db.Insertable(jobInfo).ExecuteCommand();
            }
        }


        public static int UpdateJobInfo(JobInfo jobInfo)
        {
            int res = 0;
            try
            {
                using (SqlSugarClient db = new SqlSugarClient(Config))
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
            int res = 0;
            try
            {
                using (SqlSugarClient db = new SqlSugarClient(Config))
                {
                    res = db.Updateable(jobInfo).UpdateColumns(s => new { s.State, s.Id })
                        .WhereColumns(w => new { w.Id }).ExecuteCommand();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return res;
        }
    }
}

