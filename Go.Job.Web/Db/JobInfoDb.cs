using System;
using System.Collections.Generic;
using System.Text;
using EastWestWalk.NetFrameWork.Common.Write;
using Go.Job.Model;
using SqlSugar;

namespace Go.Job.Web
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
            List<JobPager> list = new List<JobPager>();
            try
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append(" select a.Id,a.JobName,a.JobGroup,a.Cron,a.Second,a.AssemblyPath,a.ClassType,a.SchedName,a.ProjectTeam, ");
                sqlBuilder.Append(" b.TRIGGER_STATE,b.NEXT_FIRE_TIME,b.PREV_FIRE_TIME,b.START_TIME ");
                sqlBuilder.Append(" from jobinfo as a left join qrtz_triggers as b on a.SchedName=b.SCHED_NAME and a.JobName = b.JOB_NAME ");
                sqlBuilder.Append(" where a.IsDeleted = 0 order by a.SchedName,a.Id ");
                if (!string.IsNullOrWhiteSpace(projectTeam))
                {
                    sqlBuilder.Append($" and projectTeam = '{projectTeam}' ");
                }

                using (SqlSugarClient db = new SqlSugarClient(Config))
                {
                    list = db.SqlQueryable<JobPager>(sqlBuilder.ToString()).ToList();
                }
            }
            catch (Exception e)
            {
                LogService.WriteLog(e, nameof(GetJobPager));
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
                using (SqlSugarClient db = new SqlSugarClient(Config))
                {
                    return db.Queryable<JobInfo>().Where(w => w.Id == id).Single();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, "查询job失败,id=" + id);
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
            var res = 0;
            try
            {
                jobInfo.CreateTime = DateTime.Now;
                using (SqlSugarClient db = new SqlSugarClient(Config))
                {
                    res =  db.Insertable(jobInfo).ExecuteCommand();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, nameof(AddJobInfo));
            }
            return res;
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
            catch (Exception ex)
            {
                LogService.WriteLog(ex, nameof(UpdateJobInfo));
            }
            return res;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public static int DeleteJobInfo(JobInfo jobInfo)
        {
            int res = 0;
            try
            {
                using (SqlSugarClient db = new SqlSugarClient(Config))
                {
                    res = db.Updateable(jobInfo).UpdateColumns(s => new { s.IsDeleted, s.Id })
                        .WhereColumns(w => new { w.Id }).ExecuteCommand();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, nameof(DeleteJobInfo));
            }
            return res;
        }
    }
}

