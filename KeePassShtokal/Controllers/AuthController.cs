using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs.Auth;
using KeePassShtokal.AppCore.Helpers;
using KeePassShtokal.AppCore.Services;
using KeePassShtokal.Filters;
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
        public async Task<IActionResult> Login([FromBody] BaseAuthDto baseAuthDto)
        {
            var ipAddress=GetRemoteIpAddress(Request);

            var loginDto = new LoginDto
            {
                IpAddress = ipAddress,
                Username = baseAuthDto.Username,
                Password = baseAuthDto.Password,
                IsReadMode = baseAuthDto.IsReadMode
            };
            var status = await _authService.Login(loginDto);
            if (!status.Success)
            {
                return BadRequest(status);
            }

            return Ok(status);
        }
        
        [HttpGet("info")]
        [Authorize]
        [CustomAuthorize]
        public async Task<IActionResult> GetUserLoginsInfo()
        {
            try
            {
                var userLoginInfo = await _authService.GetUserLoginInfo(GetUserId(HttpContext));
                return Ok(userLoginInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(new Status(false,"Something went wrong"));
            }
        }

        [HttpPut("password")]
        [Authorize]
        [CustomAuthorize]
        [OnlyWriteMode("You cannot change password in readonly mode")]
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
