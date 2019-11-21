using Hangfire;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hangfiretest
{
    public class test: Itest
    {
        public void demo()
        {
            Console.WriteLine(System.Guid.NewGuid());
        }
    }

    public interface Itest
    {
         void demo();
    }
}
