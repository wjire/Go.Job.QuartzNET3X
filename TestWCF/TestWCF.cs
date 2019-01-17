using Go.Job.BaseJob;

namespace TestWCF
{
    public class TestWCF : BaseJob
    {
        public override void Run()
        {
            //var res = ProductServiceHelper.GetCategory(717, Go.WeiXinShop.ProductService.Model.AreaTypeEnum.市);
            ////string path1 = @"C:\Users\Administrator\Desktop\testwcf.txt";
            //string path1 = @"C:\Users\gongwei.LONG\Desktop\testwcf.txt";
            //using (FileStream fs = new FileStream(path1, FileMode.Append, FileAccess.Write))
            //{
            //    byte[] bytes = Encoding.Default.GetBytes(res + Environment.NewLine);
            //    fs.Write(bytes, 0, bytes.Length);
            //}

            //string name = Thread.GetDomain().FriendlyName;
            //Tools.FileHelper.WriteString(name);
        }
    }
}
