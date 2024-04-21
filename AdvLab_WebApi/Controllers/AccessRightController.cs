using AdvLab_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AdvLab_WebApi.Controllers
{
    [Route("API/AccessRights")]
    [ApiController]
    public class AccessRightController : ControllerBase
    {
        private readonly AppDBContext _appDBContext;
        private readonly IConfiguration _configuration;

        public AccessRightController(AppDBContext appDBContext, IConfiguration configuration)
        {
            _appDBContext = appDBContext;
            _configuration = configuration;
        }

        [HttpGet("{empId}")]
        [Authorize]
        public async Task<IActionResult> GetAccessRightByEmpId(int empId)
        {
            try
            {
                var accessRight = await _appDBContext.AccessRights
                    .Where(ar => ar.EmpID == empId).ToListAsync();

                if (accessRight == null)
                {
                    return NotFound($"AccessRight with EmpId {empId} not found.");
                }

                return Ok(accessRight);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
