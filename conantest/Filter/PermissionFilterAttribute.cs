using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace conantest.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PermissionFilterAttribute : TypeFilterAttribute
    {
        public PermissionFilterAttribute(string permissionCode) : base(typeof(PermissionFilter))
        {
            this.Arguments = new object[] { permissionCode };
        }
        private class PermissionFilter : IAsyncActionFilter
        {
            private readonly ISecurityService _securityService;
            private readonly string _permissionCode;
            public PermissionFilter(string permissionCode, ISecurityService securityService)
            {
                _securityService = securityService;
                _permissionCode = permissionCode;
            }
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                string user = "";
                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    user = context.HttpContext.User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.Name).Value;
                }
                List<string> code = await _securityService.test();
                if (!code.Any(o => o == _permissionCode))
                {
                    JsonResult json = new JsonResult(new { IsSucceeded = false, Message = "您没有操作权限,user:" + user });
                    context.Result = json;
                }
                else
                {
                    await next();
                }
            }
        }
    }
}