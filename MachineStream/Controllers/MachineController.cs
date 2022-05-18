using MachineStream.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MachineStream.Controllers
{
    [ApiController]
    [Route("V1/[controller]")]
    public class MachineController : ControllerBase
    {
        private readonly IMachineServices _machineServices;

        public MachineController(IMachineServices machineServices)
        {
            _machineServices = machineServices;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var machineInfo = await _machineServices.GetMachineInfo();
            return Ok(machineInfo);
        }
    }
}
