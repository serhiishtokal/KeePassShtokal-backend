using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs;
using KeePassShtokal.AppCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeePassShtokal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EntryController : ControllerBase
    {
        private readonly IEntryService _entryService;
        public EntryController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddEntry([FromBody] AddEntryDto addPasswordModel)
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                try
                {
                    var userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var status = await _entryService.AddEntry(addPasswordModel, userId);
                    return Ok(status);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            return BadRequest();
        }

        //[HttpPost]
        //public async Task<IActionResult> EditEntry([FromBody] AddEntryDto addPasswordModel)
        //{
        //    if (HttpContext.User.Identity is ClaimsIdentity identity)
        //    {
        //        try
        //        {
        //            var userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
        //            var status = await _entryService.AddEntry(addPasswordModel, userId);
        //            return Ok(status);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            return StatusCode(StatusCodes.Status500InternalServerError);
        //        }
        //    }
        //    return BadRequest();
        //}

    }
}
