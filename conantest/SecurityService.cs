﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace conantest
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
