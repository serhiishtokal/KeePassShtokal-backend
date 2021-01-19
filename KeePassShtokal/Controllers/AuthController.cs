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
using KeePassShtokal.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

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
        //todo add login mode
        public async Task<IActionResult> Login([FromBody] BaseAuthDto loginDto)
        {
            var ipAddress=GetRemoteIpAddress(Request);

            var model = new LoginDto
            {
                IpAddress = ipAddress,
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
        [CustomAuthorize]
        public IActionResult GetUserLoginInfo(CancellationToken cancellationToken)
        {
            if (!(HttpContext.User.Identity is ClaimsIdentity identity)) return BadRequest();
            var login = identity.FindFirst(JwtRegisteredClaimNames.GivenName).Value;
            //var result = await _authService.GetAuthInfo(login, cancellationToken);

            return Ok(login);

        }

        [HttpPut("password")]
        [Authorize]
        [CustomAuthorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var status = await _authService.ChangePassword(changePasswordDto,GetUserId(HttpContext));
            return Ok(status);
        }

        [HttpGet("ip/blocked")]
        [Authorize]
        [CustomAuthorize]
        public async Task<IActionResult> GetBlockedIps()
        {
            var status = await _authService.GetBlockedIps(GetUserId(HttpContext));
            return Ok(status);
        }

        [HttpPut("ip/unblock/{ipId}")]
        [Authorize]
        [CustomAuthorize]
        public async Task<IActionResult> UnblockIp([FromRoute] int ipId)
        {
            var status = await _authService.UnBlockIp(GetUserId(HttpContext), ipId);
            return Ok(status);
        }

        //remove to helper
        private string GetRemoteIpAddress(HttpRequest request)
        {
            var remoteIpAddress = request.HttpContext.Connection.RemoteIpAddress;
            var result = "";
            if (remoteIpAddress != null)
            {
                result = remoteIpAddress.ToString();
            }
            return result;
        }

        private int GetUserId(HttpContext httpContext)
        {
            return (int)httpContext.Items["userId"];
        }
    }
}
