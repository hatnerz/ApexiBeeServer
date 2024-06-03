using ApexiBee.Application.DTO;
using ApexiBee.Application.DTO.Sensors;
using ApexiBee.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApexiBee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorService sensorService;

        public SensorController(ISensorService sensorService)
        {
            this.sensorService = sensorService;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewSensor([FromBody] NewSensorData newSensorData)
        {
            var createdSensor = await sensorService.AddNewSensor(newSensorData);
            return Ok(createdSensor);
        }

        [HttpPost("type")]
        public async Task<IActionResult> AddNewSensorType([FromBody] NewSensorTypeData newSensorTypeData)
        {
            var createdSensorType = await sensorService.AddNewSensorType(newSensorTypeData);
            return Ok(createdSensorType);
        }

        [HttpPost("readings/hub/{hubId}")]
        public async Task<IActionResult> AddSensorReadings([FromBody] IEnumerable<NewSensorReading> sensorReadings, [FromRoute]Guid hubId)
        {
            var addedReadings = await sensorService.AddSensorReadings(sensorReadings, hubId);
            return Ok(new { message = $"Successfully added {addedReadings.Item1} of {addedReadings.Item2} readings" });
        }

        [HttpDelete("{sensorId}")]
        public async Task<IActionResult> DeleteSensor([FromRoute] Guid sensorId)
        {
            await sensorService.DeleteSensor(sensorId);
            return Ok();
        }

        [HttpGet("daily-average")]
        public async Task<IActionResult> GetDailyAverage([FromQuery] Guid sensorId, [FromQuery] DateTime date)
        {
            var result = await sensorService.GetAverageDailySensorValue(sensorId, date);
            return Ok(result);
        }

        [HttpGet("hive/{hiveId}/last")]
        public async Task<IActionResult> GetLastReadings([FromRoute] Guid hiveId)
        {
            var result = await sensorService.GetLastHiveSensorData(hiveId);
            return Ok(result);
        }

        [HttpGet("period")]
        public async Task<IActionResult> GetReadingsDuringPeriod([FromQuery] Guid sensorId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await sensorService.GetSensorReadingsWithinPeriod(sensorId, startDate, endDate);
            return Ok(result);
        }

        [HttpGet("type/all")]
        public async Task<IActionResult> GetAllSensorTypes()
        {
            var sensorTypes = await sensorService.GetSensorTypes();
            return Ok(sensorTypes);
        }
    }
}
