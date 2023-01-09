using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using urbasBackendV2.Helpers;
using urbasBackendV2.Models;

namespace urbasBackendV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mdUsersController : ControllerBase
    {
        private readonly MdContext _context;

        public mdUsersController(MdContext context)
        {
            _context = context;
        }

        // GET: api/mdUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MdUsers>>> GetmdUsers()
        {
          if (_context.mdUsers == null)
          {
              return NotFound();
          }
            return await _context.mdUsers.ToListAsync();
        }

        // GET: api/mdUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MdUsers>> GetMdUsers(long id)
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

            return mdUsers;
        }

        // PUT: api/mdUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMdUsers(long id, MdUsers mdUsers)
        {
            if (id != mdUsers.Id)
            {
                return BadRequest();
            }

            _context.Entry(mdUsers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MdUsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/mdUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MdUsers>> PostMdUsers(MdUsers mdUsers)
        {
          if (_context.mdUsers == null)
          {
              return Problem("Entity set 'MdContext.mdUsers'  is null.");
          }
            _context.mdUsers.Add(mdUsers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMdUsers", new { id = mdUsers.Id }, mdUsers);
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
    }
}
