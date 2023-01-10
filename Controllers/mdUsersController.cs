using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using urbasBackendV2.Dtos;
using urbasBackendV2.Helpers;
using urbasBackendV2.Models;

namespace urbasBackendV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mdUsersController : ControllerBase
    {
        private readonly UbContext _context;

        public mdUsersController(UbContext context)
        {
            _context = context;
        }

        // GET: api/mdUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MdUsersDto>>> GetmdUsers()
        {
            if (_context.mdUsers == null)
            {
                return NotFound();
            }
            return await _context.mdUsers.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET: api/mdUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MdUsersDto>> GetMdUsers(long id)
        {
            if (_context.mdUsers == null)
            {
                return NotFound();
            }
            var mdUsers = await _context.mdUsers.FindAsync(id);

            if (mdUsers == null)
            {
                return NotFound();
            }

            return ItemToDTO(mdUsers);
        }

        // PUT: api/mdUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMdUsers(long id, MdUsersDto mdUsersDto)
        {
            if (id != mdUsersDto.Id)
            {
                return BadRequest();
            }

            var mdUser = await _context.mdUsers.FindAsync(id);
            if (mdUser == null)
            {
                return NotFound();
            }

            mdUser.Login = mdUsersDto.Login;
            mdUser.Password = mdUsersDto.Password;

            try 
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MdUsersExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/mdUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MdUsersDto>> PostMdUsers(MdUsersDto mdUsersDto)
        {
            var mdUser = new MdUsers
            {
                Login = mdUsersDto.Login,
                Password = mdUsersDto.Password
            };

            _context.mdUsers.Add(mdUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMdUsers), new {id = mdUser.Id}, ItemToDTO(mdUser));
        }

        // DELETE: api/mdUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMdUsers(long id)
        {
            if (_context.mdUsers == null)
            {
                return NotFound();
            }
            var mdUsers = await _context.mdUsers.FindAsync(id);
            if (mdUsers == null)
            {
                return NotFound();
            }

            _context.mdUsers.Remove(mdUsers);
            await _context.SaveChangesAsync();

            return NoContent();
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
