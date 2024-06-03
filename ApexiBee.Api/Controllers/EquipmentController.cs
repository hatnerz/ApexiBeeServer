using ApexiBee.Application.Interfaces;
using ApexiBee.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ApexiBee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            this.equipmentService = equipmentService;
        }

        [HttpGet("serial/all")]
        public async Task<IActionResult> GetAllSerial()
        {
            var serialData = await equipmentService.GetAllSerialData();
            return Ok(serialData);
        }

        [HttpPost("serial/{equipmentType}")]
        public async Task<IActionResult> AddNewEquipmentSerialData([FromRoute]EquipmentType equipmentType)
        {
            var serialData = await equipmentService.AddNewSerialData(equipmentType);
            return Ok(serialData);
        }

        [HttpDelete("serial/number/{serialNumber}")]
        public async Task<IActionResult> DeleteSerialDataByNumber([FromRoute]string serialNumber)
        {
            await equipmentService.RemoveSerialDataByName(serialNumber);
            return Ok();
        }

        [HttpDelete("serial/id/{serialDataId}")]
        public async Task<IActionResult> DeleteSerialDataById([FromRoute] Guid serialDataId)
        {
            await equipmentService.RemoveSerialDataById(serialDataId);
            return Ok();
        }
    }
}
