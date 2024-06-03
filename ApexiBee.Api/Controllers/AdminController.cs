using ApexiBee.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApexiBee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IDatabaseService databaseService;

        public AdminController(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        [HttpPost("db/backup")]
        public IActionResult BackupDb()
        {
            try
            {
                databaseService.CreateDbBackup();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal error during DB backup", details = ex.ToString() });
            }
        }

        [HttpPost("db/restore")]
        public IActionResult RestoreDb()
        {
            try
            {
                databaseService.RestoreDbFromLastBackup();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal error during restoring DB", details = ex.ToString() });
            }
        }

        [HttpGet("db/lastbackup")]
        public async Task<ActionResult<DateTime?>> GetLastBackupDate()
        {
            DateTime? backupDate = databaseService.GetLastBackupDate();
            if (backupDate == null)
                return NotFound();
            else
                return backupDate;
        }

    }
}
