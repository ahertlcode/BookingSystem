using BookingSystem.Api.DTO;
using BookingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Api.Controllers
{
    [ApiController]
    //[Authorize(Roles = "super-admin")]
    [Route("api/partner-types")]
    public class PartnerTypeController : ControllerBase
    {
        private readonly PartnerTypeService _partnerTypeService;

        public PartnerTypeController(PartnerTypeService partnerTypeService)
        {
            _partnerTypeService = partnerTypeService;
        }


        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPartnerTypes()
        {
            try
            {
                var partnerTypes = await _partnerTypeService.GetAllPartnerTypesAsync();
                return Ok(partnerTypes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

       
        [HttpPost("create-partner-type")]
        public async Task<IActionResult> CreatePartnerType([FromBody] CreatePartnerTypeRequest req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var partnerType = await _partnerTypeService.CreatePartnerTypeAsync(req.Name, req.Description);
                return Created("", partnerType);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        
        [HttpPut("update-partner-type/{id}")]
        public async Task<IActionResult> UpdatePartnerType(Guid id, [FromBody] CreatePartnerTypeRequest req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var partnerType = await _partnerTypeService.UpdatePartnerTypeAsync(id, req.Name, req.Description);
                return Ok(partnerType);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        
        [HttpDelete("delete-partner-type/{id}")]
        public async Task<IActionResult> DeletePartnerType(Guid id)
        {
            try
            {
                await _partnerTypeService.DeletePartnerTypeAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
