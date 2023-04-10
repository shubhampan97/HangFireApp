using Hangfire;
using HangFireApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace HangFireApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangFireJobController : ControllerBase
    {

        private readonly IHangFireJobService _hangFireJobService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        public HangFireJobController(IHangFireJobService hangFireJobService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _hangFireJobService = hangFireJobService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet("FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJobs()
        {
            _backgroundJobClient.Enqueue(() => _hangFireJobService.FireAndForgetJob());
            return Ok();
        }

        [HttpGet("DelayedJob")]
        public ActionResult CreateDelayedJob()
        {
            _backgroundJobClient.Schedule(() => _hangFireJobService.DelayedJob(), TimeSpan.FromSeconds(30));
            return Ok();
        }

        [HttpGet("ReccuringJob")]
        public ActionResult CreateReccuringJob()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => _hangFireJobService.RecurringJob(), Cron.Minutely);
            return Ok();
        }

        [HttpGet("ContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            var parentJobId = _backgroundJobClient.Enqueue(() => _hangFireJobService.FireAndForgetJob());
            _backgroundJobClient.ContinueJobWith(parentJobId, () => _hangFireJobService.ContinuationJob());
            return Ok();
        }
    }
}
