﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace conantest.Filter
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            context.ExceptionHandled = true;// 表明异常已处理，客户端可得到正常返回
            context.HttpContext.Response.WriteAsync(context.Exception.ToString());
            context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
            return Task.CompletedTask;
        }
    }
}
