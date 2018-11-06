namespace P2.Platform.Api.Resources
{
    using LandManagement;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    public class LandController : ControllerBase
    {
        private readonly ILandService service;
        private readonly ILogger logger;

        public LandController(ILandService service, ILogger<ContactsController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [Route("test")]
        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
