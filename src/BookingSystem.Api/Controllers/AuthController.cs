using BookingSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;
using BookingSystem.Api.DTO;

namespace BookingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var token = await _userService.AuthenticateUserAsync(req.Email, req.Password);
                return Ok(new { Token = token });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await _userService.RegisterUserAsync(req.Email, req.PhoneNumber, req.Password, req.FullName, req.Role);
                return Created("", new { user.Id, user.Email, user.FullName, user.Role });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("customer-signup")]
        public async Task<IActionResult> CustomerSignup([FromBody] SignupRequest req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await _userService.RegisterCustomerAsync(req.Email, req.PhoneNumber, req.Password, req.FullName);
                return Created("", new { user.Id, user });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("partner-signup")]
        public async Task<IActionResult> PartnerSignup([FromBody] SignupRequest req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await _userService.RegisterPartnerAsync(req.Email, req.PhoneNumber, req.Password, req.FullName);
                return Created("", new { user.Id, user });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }        
    }
}
