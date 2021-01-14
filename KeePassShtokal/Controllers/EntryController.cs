using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs;
using KeePassShtokal.AppCore.Services;
using KeePassShtokal.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeePassShtokal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [CustomAuthorize]
    public class EntryController : ControllerBase
    {
        private readonly IEntryService _entryService;
        public EntryController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        private int GetUserId()
        {
            return (int)HttpContext.Items["userId"];
        }

        [HttpPost]
        public async Task<IActionResult> AddEntry([FromBody] AddEntryDto addPasswordModel)
        {
            var userId = GetUserId();
            var status = await _entryService.AddEntry(addPasswordModel, userId);
            return Ok(status);
        }

        [HttpPut]
        public async Task<IActionResult> EditEntry([FromBody] EditEntryDto editEntryDto)
        {
            var userId = GetUserId();
            var result = await _entryService.EditEntry(editEntryDto, userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
