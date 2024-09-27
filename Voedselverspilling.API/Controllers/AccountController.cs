using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.DomainServices.IServices;
using LoginRequest = Voedselverspilling.Domain.Models.LoginRequest;

namespace Voedselverspilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                Console.WriteLine(loginRequest);
                return BadRequest("Invalid login request");
            }

            try
            {
                var result = await _accountService.LoginAsync(loginRequest);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.Message);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
