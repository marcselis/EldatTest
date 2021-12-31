using System.Linq;
using Autohmation.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lamp = Autohmation.Models.Lamp;

namespace Autohmation.Controllers
{
    [Route("api/lamps")]
    public class LampsController : Controller
    {
        private readonly ILogger<LampsController> _logger;
        private readonly IMapper<Domain.Lamp, Lamp> _mapper;

        public LampsController(ILogger<LampsController> logger, IMapper<Domain.Lamp, Lamp> mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("", Name = "Get")]
        public IActionResult Get()
        {
            _logger.LogInformation("Getting lamps");
            var lamps = _mapper.Map(Domain.System.Instance.Lamps.Values.ToList());
            return Ok(lamps);
        }

        [HttpGet("{name}", Name="GetLamp")]
        public IActionResult Get(string name)
        {
            if (Domain.System.Instance.Lamps.TryGetValue(name, out var lamp))
               return Ok(lamp);
            return NotFound();
        }

        //[HttpPatch("{name}")]
        //public IActionResult Patch(string name, LampState state)
        //{
        //    try
        //    {
        //        Domain.System.Instance.SetLampState(name, state);
        //        return AcceptedAtRoute("Get");
        //    }
        //    catch (LampNotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}

        [HttpPost("{name}/turnon")]
        public IActionResult TurnLampOn(string name)
        {
            try
            {
                Domain.System.Instance.SetLampState(name, LampState.On);
                return AcceptedAtRoute("Get");
            }
            catch (LampNotFoundException)
            {
                return NotFound();
            }

        }
        [HttpPost("{name}/turnoff")]
        public IActionResult TurnLampOff(string name)
        {
            try
            {
                Domain.System.Instance.SetLampState(name, LampState.Off);
                return AcceptedAtRoute("Get");
            }
            catch (LampNotFoundException)
            {
                return NotFound();
            }

        }

    }

}