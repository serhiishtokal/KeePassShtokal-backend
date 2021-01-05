using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs;
using KeePassShtokal.AppCore.Services;
using Microsoft.AspNetCore.Authentication;

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
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var status = await _authService.Register(registerDto);
            if (!status.Success)
            {
                return BadRequest(status);
            }
            return Ok(status);
        }
        

        //Not working at current time 
        //[HttpPost("sigin")]
        //public async Task<IActionResult> Login([FromBody] BaseAuthDto loginDto)
        //{
        //    var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
        //    string result = "";
        //    if (remoteIpAddress != null)
        //    {
        //        if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
        //        {
        //            remoteIpAddress = (await System.Net.Dns.GetHostEntryAsync(remoteIpAddress)).AddressList
        //                .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        //        }
        //        result = remoteIpAddress.ToString();
        //    }
        //    var model = new LoginDto
        //    {
        //        IpAddress = result,
        //        Username = loginDto.Username,
        //        Password = loginDto.Password
        //    };
        //    var status = await _authService.Login(model);
        //    if (!status.Success)
        //    {
        //        return BadRequest(status);
        //    }

        //    return Ok(status);
        //}
    }
}
