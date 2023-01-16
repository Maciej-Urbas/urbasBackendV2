using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

using urbasBackendV2.Dtos;
using urbasBackendV2.Helpers;
using urbasBackendV2.Models;

namespace urbasBackendV2.Services;

public interface IMdUsersService
{
    Task<IEnumerable<MdUsersDto>> GetMdUsers();
    Task<MdUsersDto> GetMdUser(int id);
    Task<MdUsersDto> PutMdUser(int id, MdUsersDto mdUsersDto);
    Task<MdUsersTokenDto> PostMdUser(MdUsersDto mdUsersDto);
    Task<MdUsersDto> DeleteMdUser(int id);
    Task<MdUsersDto> Login(MdUsersDto mdUsersDto);
}

public class MdUsersService : IMdUsersService
{
    private UbContext _context;
    private IMdUsersTokenService _mdUsersTokenService;

    public MdUsersService(UbContext context, IMdUsersTokenService mdUsersTokenService)
    {
        _context = context;
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

    public async Task<IEnumerable<MdUsersDto>> GetMdUsers()
    {
        return await _context.mdUsers.Select(x => UserToDTO(x)).ToListAsync();
    }    

    public async Task<MdUsersDto> GetMdUser(int id)
    {
        var mdUsers = await _context.mdUsers.FindAsync(id);

        return UserToDTO(mdUsers);
    }
    
    public async Task<MdUsersDto> PutMdUser(int id, MdUsersDto mdUsersDto)
    {
        var hmac = new HMACSHA512();

        var mdUsers = await _context.mdUsers.FindAsync(id);
        mdUsers.Login = mdUsersDto.Login;
        mdUsers.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(mdUsersDto.Password));
        
        await _context.SaveChangesAsync();
        return UserToDTO(mdUsers);
    }

    public async Task<MdUsersTokenDto> PostMdUser(MdUsersDto mdUsersDto)
    {
        var hmac = new HMACSHA512();
        
        var mdUser = new MdUsers
        {
            Login = mdUsersDto.Login,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(mdUsersDto.Password)),
            PasswordSalt = hmac.Key,
        };

        _context.mdUsers.Add(mdUser);
        await _context.SaveChangesAsync();

        return new MdUsersTokenDto
        {
            Login = mdUser.Login,
            Token = _mdUsersTokenService.CreateToken(mdUser),
        };
    }

    public async Task<MdUsersDto> DeleteMdUser(int id)
    {
        var mdUser = await _context.mdUsers.FindAsync(id);

        _context.mdUsers.Remove(mdUser);
        await _context.SaveChangesAsync();

        return UserToDTO(mdUser);
    }

    public async Task<MdUsersDto>Login(MdUsersDto mdUsersDto)
    {
        var user = await _context.mdUsers.SingleOrDefaultAsync(x => x.Login == mdUsersDto.Login);

        var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(mdUsersDto.Password));

        return UserToDTO(user);
    }
}