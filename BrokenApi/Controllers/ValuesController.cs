namespace BrokenApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> logger;

        public ValuesController(ILogger<ValuesController> logger) => this.logger = logger;

        [HttpGet("/status-failing")]
        public async Task<IActionResult> GetFailing()
        {
            this.logger.LogDebug("X-Correlation-ID: {0}", this.Request.Headers["X-Correlation-ID"]);
            this.logger.LogDebug("User-Agent: {0}", this.Request.Headers["User-Agent"]);
          //  await Task.Delay(1000);
            return new StatusCodeResult(500);
        }

        [HttpGet("/status-working")]
        public async Task<IActionResult> GetWorking([FromQuery]ss s)
        {
            this.logger.LogDebug("X-Correlation-ID: {0}", this.Request.Headers["X-Correlation-ID"]);
            this.logger.LogDebug("User-Agent: {0}", this.Request.Headers["User-Agent"]);
           // await Task.Delay(7000);
            // var r= new StatusCodeResult(200);
            JsonResult json = new JsonResult(new { IsSucceeded = true, Message = s.RequestId });
            //r.ExecuteResultAsync(json);
            json.StatusCode = 200;
            return json;
        }


        ////[HttpPost("/status-working")]
        ////public async Task<IActionResult> GetWorking([FromBody]ss s)
        ////{
        ////    this.logger.LogDebug("X-Correlation-ID: {0}", this.Request.Headers["X-Correlation-ID"]);
        ////    this.logger.LogDebug("User-Agent: {0}", this.Request.Headers["User-Agent"]);
        ////   // await Task.Delay(7000);
        ////    // var r= new StatusCodeResult(200);
        ////    JsonResult json = new JsonResult(new { IsSucceeded = true, Message = s.RequestId });
        ////    //r.ExecuteResultAsync(json);
        ////    json.StatusCode = 200;
        ////    return json;
        ////}


    }

    public class ss
    {
        public string RequestId { get; set; }

    }
}
