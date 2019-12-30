using System;
using Volo.Abp.DependencyInjection;

namespace WebApplication15
{
    public class test : Itest
    {
        public void demo()
        {
            Console.WriteLine(DateTime.Now);
        }
    }

    public interface Itest : ITransientDependency
    {
        void demo();
    }
}