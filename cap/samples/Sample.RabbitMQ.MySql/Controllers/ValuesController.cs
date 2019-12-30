using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Sample.RabbitMQ.MySql.Controllers
{
    [Route("api/cap")]
    public class ValuesController : Controller
    {
        private readonly ICapPublisher _capBus;
        private readonly AppDbContext _dbContext;

        public ValuesController(ICapPublisher capPublisher, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _capBus = capPublisher;
        }

        [HttpGet, Route("test")]
        public async Task<IActionResult> WithoutTransaction()
        {
            await _capBus.PublishAsync("sample.rabbitmq.mysql", DateTime.Now);

            return Ok();
        }

        [HttpGet, Route("ef")]
        public IActionResult EntityFrameworkWithTransaction()
        {
            using (var trans = _dbContext.Database.BeginTransaction(_capBus, autoCommit: false))
            {
                _dbContext.Persons.Add(new Person() { Name = "ef.transaction" });

                for (int i = 0; i < 5; i++)
                {
                    _capBus.Publish("sample.rabbitmq.mysql", DateTime.Now);
                }

                _dbContext.SaveChanges();

                trans.Commit();
            }
            return Ok();
        }

        [NonAction]
        [CapSubscribe("#.rabbitmq.mysql")]
        public void Subscriber(DateTime time)
        {
            Console.WriteLine($@"{DateTime.Now}, Subscriber invoked, Sent time:{time}");
        }
    }
}