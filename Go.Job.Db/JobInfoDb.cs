using System;
using System.Collections.Generic;
using System.Text;
using Go.Job.Model;
using SqlSugar;

namespace Go.Job.Db
{
    public static class JobInfoDb
    {
        private static readonly string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        private static readonly ConnectionConfig Config;

        static JobInfoDb()
        {
            Config = new ConnectionConfig
            {
                ConnectionString = connStr,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                //InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            };
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <returns></returns>
        public static List<JobPager> GetJobPager(string projectTeam = null)
        {
            List<JobPager> list = null;
            try
            {
                StringBuilder columns = new StringBuilder();
                columns.Append(" a.Id,a.JobName,a.JobGroup,a.Cron,a.AssemblyPath,a.ClassType,a.State,a.SchedName,a.ProjectTeam,");
                columns.Append(" b.TRIGGER_STATE,b.NEXT_FIRE_TIME,b.PREV_FIRE_TIME,b.START_TIME ");

                StringBuilder whereSql = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(projectTeam))
                {
                    whereSql.Append($" where projectTeam = '{projectTeam}' ");
                }

                using (SqlSugarClient db = new SqlSugarClient(Config))
                {
                    list = db.SqlQueryable<JobPager>($"select {columns} from jobinfo as a left join qrtz_triggers as b on a.SchedName=b.SCHED_NAME ").ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return list;

        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static JobInfo GetJobInfo(int id)
        {
            try
            {
                StringBuilder columns = new StringBuilder();
                columns.Append(" a.Id,a.JobName,a.JobGroup,a.Cron,a.AssemblyPath,a.ClassType,a.State,a.SchedName,a.ProjectTeam ");
                using (SqlSugarClient db = new SqlSugarClient(Config))
                {
                    //return db.SqlQueryable<JobInfo>($"select {columns} from jobinfo where a.Id = {id}").Single();
                    return db.Queryable<JobInfo>().Where(w => w.Id == id).Single();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public static int AddJobInfo(JobInfo jobInfo)
        {
            jobInfo.CreateTime = DateTime.Now;
            using (SqlSugarClient db = new SqlSugarClient(Config))
            {
                return db.Insertable(jobInfo).ExecuteCommand();
            }
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
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

