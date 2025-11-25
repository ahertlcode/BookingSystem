using BookingSystem.Api.DTO;
using BookingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "super-admin")]
    [Route("api/sectors")]
    public class SectorController : ControllerBase
    {
        private readonly SectorService _sectorService;
        public SectorController(SectorService sectorService)
        {
            _sectorService = sectorService;
        }

        
        [HttpPost("create-sector")]
        public async Task<IActionResult> CreateSector([FromBody] CreateSectorRequest req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var sector = await _sectorService.CreateSectorAsync(req.Name, req.Description);
                return Created("", sector);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update-sector/{id}")]
        public async Task<IActionResult> UpdateSector(Guid id, [FromBody] CreateSectorRequest req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var sector = await _sectorService.UpdateSectorAsync(id, req.Name, req.Description);
                return Ok(sector);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("delete-sector/{id}")]
        public async Task<IActionResult> DeleteSector(Guid id)
        {
            try
            {
                await _sectorService.DeleteSectorAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("get-all-sectors")]
        public async Task<IActionResult> GetAllSectors()
        {
            try
            {
                var sectors = await _sectorService.GetAllSectorsAsync();
                return Ok(sectors);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
