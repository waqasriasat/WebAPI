using AdvLab_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvLab_WebApi.Controllers
{
    [Route("API/Reportings")]
    [ApiController]
    public class ReportingController : Controller
    {
        private readonly AppDBContext _appDBContext;
        private readonly IConfiguration _configuration;

        public ReportingController(AppDBContext appDBContext, IConfiguration configuration)
        {
            _appDBContext = appDBContext;
            _configuration = configuration;
        }
        [HttpGet("All")]
        [Authorize]
        public async Task<IActionResult> GetAllDescLab()
        {
            try
            {
               
                var descCashiers = await _appDBContext.DescCashiers
                //.Include(dc => dc.DescLabs)
                .Join(
                    _appDBContext.PatRegs,
                    dc => dc.MRNO,
                    pr => pr.MRNO,
                    (dc, pr) => new { DescCashier = dc, PatReg = pr }
                )
                .Join(
                    _appDBContext.DescLabs,
                    combined => combined.DescCashier.LabNo,
                    descLab => descLab.LabNo,
                    (combined, descLab) => new { combined.DescCashier, combined.PatReg, DescLab = descLab }
                )
                .Join(
                    _appDBContext.AddDescriptions,
                    combined => combined.DescLab.DescID,
                    des => des.DescID,
                    (combined, des) => new { combined.DescCashier, combined.PatReg, combined.DescLab, AddDescription = des }
                )
                .Join(
                    _appDBContext.AddClients,
                    combined => combined.DescCashier.ClientID,
                    adc => adc.CID,
                    (combined, adc) => new { combined.DescCashier, combined.PatReg, combined.DescLab, combined.AddDescription, AddClient = adc }
                )
                .OrderBy(result => result.DescCashier.LabNo)
                .ToListAsync();

                if (descCashiers == null)
                {
                    return NotFound();
                }
              
                return Ok(descCashiers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("Selected")]
        [Authorize]
        public async Task<IActionResult> GetSelectedDescLab(int mrno, int labno, string? daterange, string? patientname, string? description, string? clientname, string? status, string? contact)
        {
            try
            {

                var descCashiers = await _appDBContext.DescCashiers
                //.Include(dc => dc.DescLabs)
                .Join(
                    _appDBContext.PatRegs,
                    dc => dc.MRNO,
                    pr => pr.MRNO,
                    (dc, pr) => new { DescCashier = dc, PatReg = pr }
                )
                .Join(
                    _appDBContext.DescLabs,
                    combined => combined.DescCashier.LabNo,
                    descLab => descLab.LabNo,
                    (combined, descLab) => new { combined.DescCashier, combined.PatReg, DescLab = descLab }
                )
                .Join(
                    _appDBContext.AddDescriptions,
                    combined => combined.DescLab.DescID,
                    des => des.DescID,
                    (combined, des) => new { combined.DescCashier, combined.PatReg, combined.DescLab, AddDescription = des }
                )
                .Join(
                    _appDBContext.AddClients,
                    combined => combined.DescCashier.ClientID,
                    adc => adc.CID,
                    (combined, adc) => new { combined.DescCashier, combined.PatReg, combined.DescLab, combined.AddDescription, AddClient = adc }
                )
                .OrderBy(result => result.DescCashier.LabNo)
                .ToListAsync();

                if (descCashiers == null)
                {
                    return NotFound();
                }

                return Ok(descCashiers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetClient")]
        [Authorize]
        public async Task<IActionResult> GetClient(string prefix, string ClientV)
        {
            try
            {
                var Clients = await _appDBContext.AddClients
                    .Where(ac =>
                        EF.Functions.Like(ac.CName, $"%{prefix}%") ||
                        EF.Functions.Like(ac.Location, $"{ClientV}"))
                    .Select(ac => new { ac.CName})
                    .ToListAsync();

                return Ok(Clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetDescription")]
        [Authorize]
        public async Task<IActionResult> GetDescription(string prefix, int CID)
        {
            try
            {
                var ClientDescs = await (from adc in _appDBContext.AddClientDescs
                                         join descs in _appDBContext.AddDescriptions on adc.DescID equals descs.DescID
                                         where EF.Functions.Like(descs.DescName, $"%{prefix}%") && adc.CID == CID
                                         select new { adc.DescID, descs.DescName, adc.Price })
                                  .ToListAsync();

                return Ok(ClientDescs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetAllStatus")]
        [Authorize]
        public async Task<IActionResult> GetAllStatus()
        {
            try
            {
                var Statuss = await _appDBContext.AddStatusFIxs.ToListAsync();
                return Ok(Statuss);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
