using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication11
{
    public class SecurityService : ISecurityService
    {
        public async Task<List<string>> test()
        {
            return new List<string>() { "001" };
        }
    }

    public interface ISecurityService
    {
        Task<List<string>> test();
    }
}