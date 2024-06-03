using ApexiBee.Application.DTO;
using ApexiBee.Application.DTO.Apiary;
using ApexiBee.Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApexiBee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiaryController : ControllerBase
    {
        private readonly IApiaryService apiaryService;
        private readonly IMapper mapper;

        public ApiaryController(IMapper mapper, IApiaryService apiaryService)
        {
            this.mapper = mapper;
            this.apiaryService = apiaryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateApiary([FromBody] CreateApiaryData requestData)
        {
            var apiary = await apiaryService.CreateApiary(requestData.ApiaryData, requestData.HubData);
            var apiaryResponse = mapper.Map<ApiaryCreatedResult>(apiary);
            return Ok(apiaryResponse);
        }

        [HttpDelete("{apiaryId}")]
        public async Task<IActionResult> DeleteApiary([FromRoute] Guid apiaryId)
        {
            await apiaryService.DeleteApiary(apiaryId);
            return Ok();
        }

        [HttpPost("hive")]
        public async Task<IActionResult> AddHive([FromBody] NewHiveData hiveData)
        {
            var createdHive = await apiaryService.AddNewHive(hiveData);
            return Ok(createdHive);
        }

        [HttpDelete("hive/{hiveId}")]
        public async Task<IActionResult> DeleteHive([FromRoute] Guid hiveId)
        {
            await apiaryService.RemoveHive(hiveId);
            return Ok();
        }

        [HttpPatch("hive/{hiveId}")]
        public async Task<IActionResult> CheckHive([FromRoute] Guid hiveId)
        {
            await apiaryService.CheckHive(hiveId);
            return Ok();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserApiaries([FromRoute] Guid userId)
        {
            var apiaries = await apiaryService.GetUserApiaries(userId);
            var resultApiaries = mapper.Map<ApiaryCreatedResult[]>(apiaries);
            return Ok(resultApiaries);
        }

        [HttpGet("hives/{apiaryId}")]
        public async Task<IActionResult> GetApiaryHives([FromRoute] Guid apiaryId)
        {
            var hives = await apiaryService.GetApiaryHives(apiaryId);
            return Ok(hives);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllApiaries()
        {
            var apiaries = await apiaryService.GetAllApiariesWithHives();
            return Ok(apiaries);
        }

        [HttpGet("hive/configuration/{hiveId}")]
        public async Task<IActionResult> GetHiveConfiguration(Guid hiveId)
        {
            var configuration = await apiaryService.GetHiveConfiguration(hiveId);
            return Ok(configuration);
        }
    }
}
