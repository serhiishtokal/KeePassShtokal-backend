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
        [OnlyWriteMode("You cannot create entry in readonly mode")]
        public async Task<IActionResult> AddEntry([FromBody] AddEntryDto addPasswordModel)
        {
            var userId = GetUserId();
            var status = await _entryService.AddEntry(addPasswordModel, userId);
            return Ok(status);
        }

        [HttpPut]
        [OnlyWriteMode("You cannot edit entry in readonly mode")]
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

        [HttpDelete("entry/{entryId}")]
        [OnlyWriteMode("You cannot delete entry in readonly mode")]
        public async Task<IActionResult> DeletePassword([FromRoute] int entryId)
        {
            var result = await _entryService.DeleteEntry(entryId, GetUserId());
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEntries()
        {
            try
            {
                return Ok(await _entryService.GetAll(GetUserId()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpGet("getEntryPassword/{entryId}")]
        public async Task<IActionResult> GetDecryptedPassword([FromRoute] int entryId)
        {
            var status = await _entryService.GetEntryPassword(GetUserId(),entryId);
               if (status.Success)
                   return Ok(status.Message);
               
               return BadRequest(status);
        }
    }
}
