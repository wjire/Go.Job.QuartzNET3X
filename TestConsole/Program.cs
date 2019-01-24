using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var config = new ConnectionConfig
            //{
            //    ConnectionString = "server=192.168.10.9;database=goquartz;user=root;pwd=admin",
            //    DbType = DbType.MySql,
            //    IsAutoCloseConnection = true,
            //    //InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            //};
            //var sql = "select JOB_DATA from qrtz_job_details ";
            //var job = new Job();
            //try
            //{
            //    using (SqlSugarClient db = new SqlSugarClient(config))
            //    {
            //        job = db.SqlQueryable<Job>(sql).Single();
            //    }

            //    var str = Encoding.UTF8.GetString(job.JOB_DATA);
            //    Console.WriteLine(str);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}

            Console.WriteLine(JsonConvert.SerializeObject(new {}));


            Console.ReadKey();
        }
    }

    internal class Job
    {
        public byte[] JOB_DATA { get; set; }
    }
}
