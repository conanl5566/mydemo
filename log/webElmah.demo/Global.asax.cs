using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication13
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //https://www.cnblogs.com/liquanchun/archive/2012/12/21/2827724.html
            //https://jasonwatmore.com/post/2014/03/14/aspnet-web-api-elmahaxd-404-error-no-http-resource-was-found-that-matches-the-request-uri-http-elmah-axd
            // ASP.NET Web API - elmah.axd 404 error - No HTTP resource was found that matches the request URI 'http://.../elmah.axd'

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        ///// <summary>
        ///// 排除 Elmah 404 寄信通知
        ///// </summary>
        //public void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)
        //{
        //    var httpException = e.Exception as HttpException;
        //    if (httpException != null && (httpException.GetHttpCode() == 404 || httpException.Message.StartsWith("A potentially dangerous Request.Path value was detected from the client")))
        //    {
        //        e.Dismiss();
        //    }
        //}

        /// <summary>
        /// 自定 Elmah 發信主旨
        /// </summary>
        private void ErrorMail_Mailing(object sender, Elmah.ErrorMailEventArgs e)
        {
            string machineName = "none server";
            try
            {
                if (Request != null)
                    machineName = Request.ServerVariables["HTTP_HOST"];
            }
            catch (Exception)
            {
                //throw;
            }

            // 取得 Elamh ErrorMail 的主旨
            // "$MachineName$ at $ErrorTime$ : {0}"

            string elmahSubject = e.Mail.Subject;
            //替換 ErrorMail 的主旨內容
            string emailSubject = string.Format("BigZata Error => {0}",
                elmahSubject
                    .Replace("$MachineName$", machineName)
            );

            e.Mail.Subject = emailSubject;
        }
    }
}