using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using urbasBackendV2.Dtos;
using urbasBackendV2.Helpers;
using urbasBackendV2.Models;
using urbasBackendV2.Services;

namespace urbasBackendV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mdUsersController : ControllerBase
    {
        private readonly UbContext _context;
        private IMdUsersService _mdUsersService;

        public mdUsersController(UbContext context, IMdUsersService mdUsersService)
        {
            _context = context;
            _mdUsersService = mdUsersService;
        }

        // GET: api/mdUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MdUsersDto>>> GetMdUsers()
        {
            return Ok(await _mdUsersService.GetMdUsers());
        }

        // GET: api/mdUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MdUsersDto>> GetMdUsers(long id)
        {
            return Ok(await _mdUsersService.GetMdUser(id));
        }

        // PUT: api/mdUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<MdUsersDto>> PutMdUsers(long id, MdUsersDto mdUsersDto)
        {
            return await _mdUsersService.PutMdUser(id, mdUsersDto);
        }

        // POST: api/mdUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MdUsersDto>> PostMdUsers(MdUsersDto mdUsersDto)
        {
            return await _mdUsersService.PostMdUser(mdUsersDto);
        }

        // DELETE: api/mdUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MdUsersDto>> DeleteMdUsers(long id)
        {
            return await _mdUsersService.DeleteMdUser(id);
        }

        private bool MdUsersExists(long id)
        {
            return (_context.mdUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static MdUsersDto ItemToDTO(MdUsers mdUser) =>
        new MdUsersDto
        {
            Id = mdUser.Id,
            Login = mdUser.Login,
            Password = mdUser.Password
        };
    }
}
