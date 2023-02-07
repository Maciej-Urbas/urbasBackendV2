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
    [Route("api/mdUsers")]
    [ApiController]
    public class mdUsersController : ControllerBase
    {
        private readonly UbContext _context;
        private IMdUsersService _mdUsersService;
        private IMdUsersTokenService _mdUsersTokenService;

        public mdUsersController(UbContext context, IMdUsersService mdUsersService, IMdUsersTokenService mdUsersTokenService)
        {
            _context = context;
            _mdUsersService = mdUsersService;
            _mdUsersTokenService = mdUsersTokenService;
        }

        // UserToDTO Helper
        private static MdUsersDto UserToDTO(MdUsers mdUser) =>
            new MdUsersDto
            {
                Id = mdUser.Id,
                Login = mdUser.Login,
                Password = "####"
            };

        // UserExists string Helper
        private async Task <bool>UserExists(string login)
        {
            return await _context.mdUsers.AnyAsync(x => x.Login == login.ToLower());
        }

        // GET: api/mdUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MdUsersDto>>> GetMdUsers()
        {
            var users = await _context.mdUsers.Select(x => UserToDTO(x)).ToListAsync();
            if (users == null) return BadRequest("There is no users in DB");
            return Ok(await _mdUsersService.GetMdUsers());
        }

        // GET: api/mdUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MdUsersDto>> GetMdUser(int id)
        {
            var mdUser = await _context.mdUsers.FindAsync(id);
            if (mdUser==null) return BadRequest("There is no mdUser with such id");

            return Ok(await _mdUsersService.GetMdUser(id));
        }

        // PUT: api/mdUsers/5
        [HttpPut("{id}")]
        public async Task<ActionResult<MdUsersDto>> PutMdUsers(int id, MdUsersDto mdUsersDto)
        {
            if (await UserExists(mdUsersDto.Login)) return BadRequest("Login Is Already Taken");
            return await _mdUsersService.PutMdUser(id, mdUsersDto);
        }

        // POST: api/mdUsers
        [HttpPost]
        public async Task<ActionResult<MdUsersTokenDto>> PostMdUsers(MdUsersDto mdUsersDto)
        {
            if (await UserExists(mdUsersDto.Login)) return BadRequest("Login Is Already Taken");
            return await _mdUsersService.PostMdUser(mdUsersDto);
        }

        // DELETE: api/mdUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MdUsersDto>> DeleteMdUsers(int id)
        {
            var mdUser = await _context.mdUsers.FindAsync(id);
            if (await UserExists(mdUser.Login)) return BadRequest("Login Is Already Taken");

            return await _mdUsersService.DeleteMdUser(id);
        }

        // POST: api/mdUsers/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<MdUsersTokenDto>> Login(MdUsersDto mdUsersDto)
        {
            var mdUser = await _context.mdUsers.SingleOrDefaultAsync(x => x.Login == mdUsersDto.Login);
            if (mdUser == null) return Unauthorized("Invalid Login");

            var hmac = new HMACSHA512(mdUser.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(mdUsersDto.Password));
            for(int i=0;i<computedHash.Length;i++)
            {
                if (computedHash[i] != mdUser.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new MdUsersTokenDto
            {
                Login = mdUser.Login,
                Token = _mdUsersTokenService.CreateToken(mdUser),
            };
        }
    }
}
