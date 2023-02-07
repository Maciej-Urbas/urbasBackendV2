using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

using urbasBackendV2.Dtos;
using urbasBackendV2.Helpers;
using urbasBackendV2.Models;
using urbasBackendV2.Services;

namespace urbasBackendV2.Controllers
{
    [Route("api/mdMessages")]
    [ApiController]
    public class mdMessagesController : ControllerBase
    {
        private readonly UbContext _context;
        private IMdMessagesService _mdMessagesService;

        public mdMessagesController(UbContext context, IMdMessagesService mdMessagesService)
        {
            _context = context;
            _mdMessagesService = mdMessagesService;
        }

        // MessageToDTO Helper
        private static MdMessagesDto MessageToDTO(MdMessages mdMessages) =>
            new MdMessagesDto
            {
                Id = mdMessages.Id,
                Name = mdMessages.Name,
                Email = mdMessages.Email,
                Topic = mdMessages.Topic,
                Content = mdMessages.Content,
                IpAddress = "####",
                State = Enums.State.recived,
            };

        // MessageExists Helper with counter
        private async Task<int> MessageExists(string ipAddress)
        {
            var messages = await _context.mdMessages.CountAsync(x => x.IpAddress == ipAddress);
            if (messages == null) return -1;

            return messages;
        }

        // GET: api/mdMessages
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MdMessagesDto>>> GetMdMessages()
        {
            var messages = await _context.mdMessages.Select(x => MessageToDTO(x)).ToListAsync();
            if (messages == null) return BadRequest("There is no messages in DB");
            return Ok(await _mdMessagesService.GetMdMessages());
        }

        // GET: api/mdMessages/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MdMessagesDto>> GetMdMessage(int id)
        {
            var mdMessage = await _context.mdMessages.FindAsync(id);
            if (mdMessage==null) return BadRequest("There is no mdMessage with such id");

            return Ok(await _mdMessagesService.GetMdMessage(id));
        }

        // PUT: api/mdUsers/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<MdMessagesDto>> PutMdMessage(int id, MdMessagesDto mdMessagesDto)
        {
            var mdMessage = await _context.mdMessages.FindAsync(id);
            if (mdMessage == null) return BadRequest("There is no mdMessage with such id");
            
            return await _mdMessagesService.PutMdMessage(id, mdMessagesDto);
        }

        // POST: api/mdMessages
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<MdMessagesDto>> PostMdUsers(MdMessagesDto mdMessagesDto)
        {   
            var counter = await MessageExists(mdMessagesDto.IpAddress);
            if(counter >= 3) return BadRequest("Sorry but you can't add more than 3 messages");

            return await _mdMessagesService.PostMdMessage(mdMessagesDto);
        }

        // DELETE: api/mdMessages/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<MdMessagesDto>> DeleteMdMessage(int id)
        {
            var mdMessage = await _context.mdMessages.FindAsync(id);
            if (mdMessage == null) return BadRequest("There is no mdMessage with such id");

            return await _mdMessagesService.DeleteMdMessage(id);
        }
    }
}