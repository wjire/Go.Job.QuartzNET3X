小工具测试流程:
1.新建你的小工具类库项目(.NET Framework 4.5),比如 : TestJob2

2.安装Nuget包 : Go.Job.BaseJob 
  如果你的小工具不依赖 
  <package id="EastWestWalk.NetFrameWork.Common" version="1.0.0.6" targetFramework="net452" />
  <package id="EastWestWalk.NetFrameWork.Enum" version="1.0.0.3" targetFramework="net452" />
  那么,你还需要安装它们.

3.新建 Class,继承 BaseJob ,实现 Execute 方法,方法内部写具体的逻辑代码.
  示例:
	/// <summary>
	/// 测试 Job
	/// </summary>
	public class Job2 : BaseJob
	{
		/// <summary>
		/// 该方法已被try catch包裹,并已记录开始执行时间和结束时间.日志记录在小工具项目的 "Debug\Logs" 文件夹下.
		/// </summary>
		protected override void Execute()
		{
			//里面是具体逻辑
            string jobName = System.Configuration.ConfigurationManager.AppSettings["JobName"];
            Console.WriteLine($"{DateTime.Now} : {jobName} ");
		}
	}
	1)如果小工具项目要调用 WCF 服务,和平时一样,把你的 "WCFConfig" 文件夹放在小工具项目的 "Debug" 目录下.
	2)如果小工具项目需要使用配置文件,请右键点击小工具项目,"添加"=>"新建项",选择"应用程序配置文件",然后VS会添加一个 "App.config" 文件,右键点击该文件,选择"属性",将"复制到输出项目"修改为"始终复制".目的是在项目编译生成后,在 "Debug" 目录下生成 "TestJob2.dll.config" 文件.

4.在控制台应用程序中调用如下方法:	
	JobServiceBuilder.BuilderTest().Start(new JobInfo
	 {
		 //程序集物理路径
		 AssemblyPath = @"E:\gongwei\my\Go.Job\TestJob2\bin\Debug\TestJob2.dll",
     
		 //类型的完全限定名
		 ClassType = "TestJob2.Job2",

		 //Second 和 Cron 至少要有一个
		 //间隔时间(秒),运行后,会立刻执行一次.
		 Second = 5,

		 //时间表达式,0秒,5秒,10秒,15秒...55秒执行.
		 //Cron = "0/5 * * * * ?"
	 });
5.启动控制台应用程序.