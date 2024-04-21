using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AdvLab_WebApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AdvLab_WebApi.Models.Temp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdvLab_WebApi.Controllers
{
    [Route("API/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _appDBContext;
        private readonly IConfiguration _configuration;

        public UserController(AppDBContext appDBContext, IConfiguration configuration)
        {
            _appDBContext = appDBContext;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLoginStatus([FromBody] Login logins)
        {
            try
            {
                var query = from us in _appDBContext.Users
                            join adcl in _appDBContext.AddConnLabs
                                on us.CNL equals adcl.LocCate into adclGroup
                            from adcl in adclGroup.DefaultIfEmpty()
                            join adl in _appDBContext.AddLocations
                                on us.Location equals adl.LocCate into adlGroup
                            from adl in adlGroup.DefaultIfEmpty()
                            where us.EmpID == logins.EmpID && us.UPassword == logins.UPassword
                            select new { User = us, AddConnLab = adcl, AddLocation = adl };

                var result = await query.FirstOrDefaultAsync();

                if (result == null || result.User == null)
                {
                    return NotFound("Wrong UserID and UserPassword");
                }

                var user = result.User;
                var adcl1 = result.AddConnLab;
                var adl1 = result.AddLocation;

                if (adcl1 != null && adcl1.LocActive == "No")
                {
                    return NotFound("Your ConnectingLab is Deactive");
                }

                if (adl1 != null && adl1.LocActive == "No")
                {
                    return NotFound("Your Location is Deactive");
                }

                if (user.Status == "No")
                {
                    return BadRequest("Your ID is Deactive");
                }

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Userid", Convert.ToUInt32(user.EmpID).ToString()),
                    new Claim("UserName", user.UName),
                    new Claim("RoleID", Convert.ToUInt32(user.RoleID).ToString()),
                    new Claim("Roles", "Admin"), // Adjust based on user's actual role
                    new Claim("Date", DateTime.Now.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: signIn);

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                var loginLog = new LoginLog
                {
                    CompName = "YourCompName",
                    CompUser = "YourCompUser",
                    IP = "127.0.0.1",
                    MacAddress = "00:00:00:00:00:00",
                    LoginStatus = "Successful",
                    UserID = user.EmpID,
                    UserName = user.UName
                };
                //await InsertLoginLog(loginLog);

                return Ok(new
                {
                    user.EmpID,
                    user.UName,
                    user.CNL,
                    user.ClientV,
                    user.Depart,
                    user.SubDepart,
                    user.Placement,
                    user.Designation,
                    user.RoleID,
                    user.Location,
                    Token = jwtToken,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    
        [HttpGet("Select/{empId}")]
        [Authorize]
        public async Task<IActionResult> GetUserByEmpId(int empId)
        {
            try
            {
                var user = await _appDBContext.Users
                                .Where(us => us.EmpID == empId)
                                .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound($"User with EmpId {empId} not found.");
                }

                var userIdClaim = User.FindFirst("Userid")?.Value;

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("All")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _appDBContext.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
       
        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> InsertUser([FromBody] User newUser)
        {
            try
            {
                int? maxEmpID = _appDBContext.Users.Max(u => u.EmpID);
                newUser.EmpID = (maxEmpID.HasValue) ? maxEmpID.Value + 1 : 1;
                _appDBContext.Users.Add(newUser);
                await _appDBContext.SaveChangesAsync();
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("Update/{empId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int empId, [FromBody] User updatedUser)
        {
            try
            {
                var existingUser = await _appDBContext.Users
                    .FirstOrDefaultAsync(us => us.EmpID == empId);

                if (existingUser == null)
                {
                    return NotFound($"User with EmpId {empId} not found.");
                }

                // Update properties based on your requirements
                existingUser.UName = updatedUser.UName;
                existingUser.Status = updatedUser.Status;
                existingUser.Location = updatedUser.Location;
                existingUser.ClientV = updatedUser.ClientV;
                existingUser.Depart = updatedUser.Depart;
                existingUser.UPassword = updatedUser.UPassword;
                existingUser.RoleID = updatedUser.RoleID;
                existingUser.CNL = updatedUser.CNL;
                existingUser.Designation = updatedUser.Designation;
                existingUser.SubDepart = updatedUser.SubDepart;
                existingUser.Placement = updatedUser.Placement;



                await _appDBContext.SaveChangesAsync();

                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpDelete("Delete/{empId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int empId)
        {
            try
            {
                var userToDelete = await _appDBContext.Users
                    .FirstOrDefaultAsync(us => us.EmpID == empId);

                if (userToDelete == null)
                {
                    return NotFound($"User with EmpId {empId} not found.");
                }

                _appDBContext.Users.Remove(userToDelete);
                await _appDBContext.SaveChangesAsync();

                return Ok($"User with EmpId {empId} deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("GetAllRole")]
        [Authorize]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _appDBContext.Roles.ToListAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetAllLocation")]
        [Authorize]
        public async Task<IActionResult> GetAllLocation()
        {
            try
            {
                var addlocations = await _appDBContext.AddLocations.ToListAsync();
                return Ok(addlocations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetAllCNL")]
        [Authorize]
        public async Task<IActionResult> GetAllCNL()
        {
            try
            {
                var addconnLabs = await _appDBContext.AddConnLabs.ToListAsync();
                return Ok(addconnLabs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetAllDepart")]
        [Authorize]
        public async Task<IActionResult> GetAllDepart()
        {
            try
            {
                var adddeparts = await _appDBContext.AddDeparts.ToListAsync();
                return Ok(adddeparts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetAllSubDepart")]
        [Authorize]
        public async Task<IActionResult> GetAllSubDepart()
        {
            try
            {
                var addsubDeparts = await _appDBContext.AddSubDeparts.ToListAsync();
                return Ok(addsubDeparts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetAllSubDepartbyDepart")]
        [Authorize]
        public async Task<IActionResult> GetAllSubDepartbyDepart(string depart)
        {
            try
            {
                //var addsubDeparts = await _appDBContext.AddSubDeparts.ToListAsync();
                //return Ok(addsubDeparts);
                var addsubDeparts = await _appDBContext.AddSubDeparts
            .Where(s => s.Depart == depart)
            .ToListAsync();

                return Ok(addsubDeparts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetAllPlacementandDepartbySubDepart")]
        [Authorize]
        public async Task<IActionResult> GetAllPlacementandDepartbySubDepart(string subDepartValue)
        {
            try
            {
                //var addsubDeparts = await _appDBContext.AddSubDeparts.ToListAsync();
                //return Ok(addsubDeparts);
                var addsubDeparts = await _appDBContext.AddSubDeparts
            .Where(s => s.SubDepart == subDepartValue)
            .ToListAsync();

                return Ok(addsubDeparts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetCNLByLocation")]
        [Authorize]
        public async Task<IActionResult> GetCNLByLocation(string LocationValue)
        {
            try
            {
                var crystalreportpaths = await _appDBContext.CrystalReportPaths
            .Where(s => s.Loc == LocationValue)
            .ToListAsync();

                return Ok(crystalreportpaths);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetAllPlacement")]
        [Authorize]
        public async Task<IActionResult> GetAllPlacement()
        {
            try
            {
                var addplacements = await _appDBContext.AddPlacements.ToListAsync();
                return Ok(addplacements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetAllDesignation")]
        [Authorize]
        public async Task<IActionResult> GetAllDesignation()
        {
            try
            {
                var adddesignations = await _appDBContext.AddDesignations.ToListAsync();
                return Ok(adddesignations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetUsers")]
        [Authorize]
        public async Task<IActionResult> GetUsers(string prefix)
        {
            try
            {
                var users = await _appDBContext.Users
                    .Where(us =>
                        EF.Functions.Like(us.EmpID.ToString(), $"%{prefix}%") ||
                        EF.Functions.Like(us.UName, $"%{prefix}%") ||
                        EF.Functions.Like(us.Location, $"%{prefix}%") ||
                        EF.Functions.Like(us.Depart, $"%{prefix}%"))
                    .Select(us => new { us.UName, us.EmpID, us.Location })
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPut("ChangePassword/{empId}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(int empId, [FromBody] ChangePassword updatedUser)
        {
            try
            {
                var existingUser = await _appDBContext.Users
                .FirstOrDefaultAsync(us => us.EmpID == empId && us.UPassword == updatedUser.CurrentPassword);
                if (existingUser == null)
                {
                    return NotFound($"User with EmpId {empId} not found. or Password are Wrong");
                }

                // Update properties based on your requirements
                existingUser.UPassword = updatedUser.ConfirmNewPassword;
                await _appDBContext.SaveChangesAsync();

                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPut("ChangeLocation/{empId}")]
        [Authorize]
        public async Task<IActionResult> ChangeLocation(int empId, [FromBody] ChangeLocation updatedUser)
        {
            try
            {
                var existingUser = await _appDBContext.Users
                .FirstOrDefaultAsync(us => us.EmpID == empId);
                if (existingUser == null)
                {
                    return NotFound($"User with EmpId {empId} not found");
                }

                // Update properties based on your requirements
                existingUser.Location = updatedUser.Location;
                await _appDBContext.SaveChangesAsync();

                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}