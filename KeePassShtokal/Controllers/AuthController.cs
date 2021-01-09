using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs;
using KeePassShtokal.AppCore.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace KeePassShtokal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var status = await _authService.Register(registrationDto);
            if (!status.Success)
            {
                return BadRequest(status);
            }
            return Ok(status);
        }

        [HttpPost("sigin")]
        public async Task<IActionResult> Login([FromBody] BaseAuthDto loginDto)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            string result = "";
            if (remoteIpAddress != null)
            {
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = (await System.Net.Dns.GetHostEntryAsync(remoteIpAddress)).AddressList
                        .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                result = remoteIpAddress.ToString();
            }
            var model = new LoginDto
            {
                IpAddress = result,
                Username = loginDto.Username,
                Password = loginDto.Password
            };
            var status = await _authService.Login(model);
            if (!status.Success)
            {
                return BadRequest(status);
            }

            return Ok(status);
        }


        [HttpGet("info")]
        [Authorize]
        public IActionResult GetUserLoginInfo(CancellationToken cancellationToken)
        {
            if (!(HttpContext.User.Identity is ClaimsIdentity identity)) return BadRequest();
            var login = identity.FindFirst(JwtRegisteredClaimNames.GivenName).Value;
            //var result = await _authService.GetAuthInfo(login, cancellationToken);

            return Ok(login);

        }
    }
}
