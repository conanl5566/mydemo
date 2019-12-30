namespace HttpClientSample.Controllers
{
    using HttpClientSample.Clients;
    using HttpClientSample.Models;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("[controller]")]
    [ApiController]
    public class RocketController : ControllerBase
    {
        private readonly IRocketClient rocketClient;

        public RocketController(IRocketClient rocketClient) => this.rocketClient = rocketClient;

        [HttpGet("takeoff")]
        public async Task<ActionResult<TakeoffStatus>> Takeoff(bool working = true)
        {
            try
            {
                return await this.rocketClient.GetStatus(working);
            }
            catch (Exception exception)
            {
                return new TakeoffStatus() { Status = exception.ToString() };
            }
        }
    }
}