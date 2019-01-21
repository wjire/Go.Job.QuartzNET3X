using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            IPerson student = new Student();
            //Console.WriteLine(student.GetType());
            SetService(student);
            Console.ReadKey();
        }

        internal static void SetService<T>(T instance) 
        {
            Console.WriteLine(typeof(T));
        }
    }


    interface IPerson
    {

    }

    public class Student : IPerson
    {

    }
}
