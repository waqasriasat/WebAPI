using AdvLab_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvLab_WebApi.Controllers
{
    [Route("API/Receptions")]
    [ApiController]
    public class ReceptionController : ControllerBase
    {

        private readonly AppDBContext _appDBContext;
        private readonly IConfiguration _configuration;

        public ReceptionController(AppDBContext appDBContext, IConfiguration configuration)
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
                var result = from dc in _appDBContext.PatRegs
                             join pr in _appDBContext.DescCashiers on dc.MRNO equals pr.MRNO
                             select new
                             {
                                 // Select the properties you need
                                 PatReg = dc,
                                 DescCashier = pr
                             };

                var resultList = result.ToList();
                return Ok(resultList); // Change this line to return Ok(resultList)
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("GetDefaultNameKeys")]
        [Authorize]
        public async Task<IActionResult> GetDefaultNameKeys(string InitialValue)
        {
            try
            {
                var addInitials = await _appDBContext.PatReg_Shortkeys
            .Where(s => s.Initial == InitialValue)
            .ToListAsync();

                return Ok(addInitials);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetInitialList")]
        [Authorize]
        public async Task<IActionResult> GetInitialList()
        {
            try
            {
                var addInitials = await _appDBContext.PatReg_Shortkeys.ToListAsync();
                return Ok(addInitials);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetRelationList")]
        [Authorize]
        public async Task<IActionResult> GetRelationList()
        {
            try
            {
                var addRelation = await _appDBContext.PatReg_Shortkeys
                     .Where(s => s.Relation != null && s.Relation.Trim() != "")
                     .ToListAsync();
                return Ok(addRelation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetAgeTypeList")]
        [Authorize]
        public async Task<IActionResult> GetAgeTypeList()
        {
            try
            {
                var addAgeType = await _appDBContext.PatReg_Shortkeys
                     .Where(s => s.Years != null && s.Years.Trim() != "")
                     .ToListAsync();
                return Ok(addAgeType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetGenderList")]
        [Authorize]
        public async Task<IActionResult> GetGenderList()
        {
            try
            {
                var addGender = await _appDBContext.PatReg_Shortkeys
                     .Where(s => s.Gender != null && s.Gender.Trim() != "")
                     .ToListAsync();
                return Ok(addGender); ;
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
                    .Select(ac => new { ac.CID, ac.CName, ac.Location, ac.PerA })
                    .ToListAsync();

                return Ok(Clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetClientIdz")]
        [Authorize]
        public async Task<IActionResult> GetClientIdz(int prefix)
        {
            try
            {
                var Clients = await _appDBContext.AddClients
            .Where(ac => ac.CID == prefix)
            .Select(ac => new { ac.CID, ac.CName, ac.Location, ac.PerA })
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
        [HttpGet("GetDescriptionByID")]
        [Authorize]
        public async Task<IActionResult> GetDescriptionByID(int prefix)
        {
            try
            {
                var MainDescs = await _appDBContext.AddDescriptions
                .Where(ad => ad.DescID == prefix)
                .Select(ad => new { ad.DescID, ad.DescName })
                .ToListAsync();


                return Ok(MainDescs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("UpdateLocSnoWithGet")]
        [Authorize]
        public async Task<int?> UpdateLocSnoWithGet(string location)
        {
            var locationToUpdate = await _appDBContext.AddLocations
                .Where(l => l.Location == location)
                .FirstOrDefaultAsync();

            if (locationToUpdate != null)
            {
                locationToUpdate.LocSno++;
                await _appDBContext.SaveChangesAsync();
                return locationToUpdate.LocSno;
            }

            return null;
        }
        [HttpGet("ReportingDateGet")]
        [Authorize]
        public async Task<int?> ReportingDateGet(int descID)
        {
            var descValue = await _appDBContext.AddDescriptions
                 .Where(d => d.DescID == descID)
                 .Select(d => d.DValue)
                 .FirstOrDefaultAsync();

            if (descValue != 0)
            {
                return descValue;
            }
            else if (descValue == 0)
            {
                return descValue;
            }
            else
            {
                throw new Exception("DValue is 0 or not found for the given DescID.");
            }
        }
        [HttpGet("GetDiscountAllow")]
        [Authorize]
        public async Task<int?> GetDiscountAllow(int userId)
        {
            var descValue = await _appDBContext.AddDiscountAuthoritys
                .Where(d => d.UserID == userId)
                .Select(d => d.DiscountAllow)
                .FirstOrDefaultAsync();

            if (descValue != 0)
            {
                return descValue;
            }
            else if (descValue == 0)
            {
                return descValue;
            }
            else
            {
                return 0;
            }
        }
        [HttpGet("GetBalanceAllow")]
        [Authorize]
        public async Task<int?> GetBalanceAllow(int userId)
        {
            var descValue = await _appDBContext.AddBalanceAuthoritys
                .Where(d => d.UserID == userId)
                .Select(d => d.BalanceAllow)
                .FirstOrDefaultAsync();

            if (descValue != 0)
            {
                return descValue;
            }
            else if (descValue == 0)
            {
                return descValue;
            }
            else
            {
                return 0;
            }
        }
        [HttpGet("AllByUser")]
        [Authorize]
        public async Task<IActionResult> GetAllOrderBYUser()
        {
            try
            {
                var PatReg = await _appDBContext.PatRegs.ToListAsync();
                return Ok(PatReg);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> InsertBooking([FromBody] CreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                model.PatReg.DBO = ConvertJsonToNullableDate(model.PatReg.DBO);
                {
                    _appDBContext.PatRegs.Add(model.PatReg);
                    await _appDBContext.SaveChangesAsync();
                    model.DescCashier.MRNO = model.PatReg.MRNO;


                    _appDBContext.DescCashiers.Add(model.DescCashier);
                    await _appDBContext.SaveChangesAsync();

                    for (int i = 0; i < model.DescCashier.DescLabs.Count; i++)
                    {
                        var existingDescLab = await _appDBContext.DescLabs.FirstOrDefaultAsync(d => d.DescID == model.DescCashier.DescLabs[i].DescID && d.LabNo == model.DescCashier.DescLabs[i].LabNo);

                        if (existingDescLab != null)
                        {
                            existingDescLab.BarcodeNo = model.DescCashier.DescLabs[i].DescID.ToString() + model.DescCashier.LabNo.ToString();
                        }
                        else
                        {
                            throw new Exception($"DescLab with ID {model.DescCashier.DescLabs[i].LabNo} not found.");
                        }
                        await _appDBContext.SaveChangesAsync();
                    }
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPut("Update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, CreateViewModel updatedModel)
        {
            var existingDescCashier = await _appDBContext.DescCashiers
                .Include(a => a.DescLabs)
                .FirstOrDefaultAsync(a => a.LabNo == id);

            if (existingDescCashier == null)
            {
                return NotFound();
            }

            // Update DescCashier properties
            existingDescCashier.ClientID = updatedModel.DescCashier.ClientID;
            existingDescCashier.ClientNo = updatedModel.DescCashier.ClientNo;
            existingDescCashier.ConsName = updatedModel.DescCashier.ConsName;
            existingDescCashier.PaymentMode = updatedModel.DescCashier.PaymentMode;
            existingDescCashier.CreditCardNo = updatedModel.DescCashier.CreditCardNo;
            existingDescCashier.GrossA = updatedModel.DescCashier.GrossA;
            existingDescCashier.DiscPer = updatedModel.DescCashier.DiscPer;
            existingDescCashier.Discount = updatedModel.DescCashier.Discount;
            existingDescCashier.TotalA = updatedModel.DescCashier.TotalA;
            existingDescCashier.PaidA = updatedModel.DescCashier.PaidA;
            existingDescCashier.BlanceA = updatedModel.DescCashier.BlanceA;

            // Update DescLabs
            existingDescCashier.DescLabs.Clear();
            existingDescCashier.DescLabs.AddRange(updatedModel.DescCashier.DescLabs);

            var existingPatReg = await _appDBContext.PatRegs
                .FirstOrDefaultAsync(a => a.MRNO == existingDescCashier.MRNO);

            if (existingPatReg != null)
            {
                // Update PatReg properties
                existingPatReg.Initial = updatedModel.PatReg.Initial;
                existingPatReg.FirstName = updatedModel.PatReg.FirstName;
                existingPatReg.Relation = updatedModel.PatReg.Relation;
                existingPatReg.RelName = updatedModel.PatReg.RelName;
                existingPatReg.Age = updatedModel.PatReg.Age;
                existingPatReg.AgeType = updatedModel.PatReg.AgeType;
                existingPatReg.Gender = updatedModel.PatReg.Gender;
                existingPatReg.ContNo = updatedModel.PatReg.ContNo;
                existingPatReg.DBO = updatedModel.PatReg.DBO;
                existingPatReg.Email = updatedModel.PatReg.Email;
                existingPatReg.PhotoUrl = updatedModel.PatReg.PhotoUrl;
            }

            await _appDBContext.SaveChangesAsync();
            return Ok(updatedModel);
        }
        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var DescCashierToDelete = await _appDBContext.DescCashiers
                    .Include(a => a.DescLabs)
                    .FirstOrDefaultAsync(dc => dc.LabNo == id);
                if (DescCashierToDelete == null)
                {
                    return NotFound($"Order Booking with LabNo {id} not found.");
                }

                var PatRegToDelete = await _appDBContext.PatRegs
                    .FirstOrDefaultAsync(pr => pr.MRNO == DescCashierToDelete.MRNO);
                if (DescCashierToDelete == null)
                {
                    return NotFound($"Order Booking with LabNo {id} not found.");
                }
                
                _appDBContext.DescCashiers.Remove(DescCashierToDelete);
                _appDBContext.PatRegs.Remove(PatRegToDelete);
                await _appDBContext.SaveChangesAsync();

                return Ok($"Order Booking with LabNo {id} deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        private System.DateOnly? ConvertJsonToNullableDate(dynamic json)
        {
            if (json != null)
            {
                try
                {
                    int year = json.year;
                    int month = json.month;
                    int day = json.day;
                    return new System.DateOnly(year, month, day);
                }
                catch (Exception)
                {
                    // Handle conversion errors
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        [HttpGet("Select/{Id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(int Id)
        {
            try
            {
                var descCashier = await _appDBContext.DescCashiers
                .Include(dc => dc.DescLabs)
                .FirstOrDefaultAsync(dc => dc.LabNo == Id);

                if (descCashier == null)
                {
                    return NotFound();
                }
                var patReg = await _appDBContext.PatRegs.FindAsync(descCashier.MRNO);

                if (patReg == null)
                {
                    return NotFound();
                }

                var model = new CreateViewModel
                {
                    PatReg = patReg,
                    DescCashier = descCashier
                };

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
