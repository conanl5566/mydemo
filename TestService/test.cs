using System;
using System.Collections.Generic;
using System.Text;

namespace TestService
{
    public class test : Itest
    {
        public void demo()
        {
            Console.WriteLine(System.Guid.NewGuid());
        }
    }

    public interface Itest: IDependency
    {
        void demo();
    }
}
