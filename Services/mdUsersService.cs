using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

using urbasBackendV2.Dtos;
using urbasBackendV2.Helpers;
using urbasBackendV2.Models;

namespace urbasBackendV2.Services;

public interface IMdUsersService
{
    Task<IEnumerable<MdUsersDto>> GetMdUsers();
    Task<MdUsersDto> GetMdUser(long id);
    Task<MdUsersDto> PutMdUser(long id, MdUsersDto mdUsersDto);
    Task<MdUsersDto> PostMdUser(MdUsersDto mdUsersDto);
    Task<MdUsersDto> DeleteMdUser(long id);
}

public class MdUsersService : IMdUsersService
{
    private UbContext _context;
    private readonly IMapper _mapper;

    public MdUsersService(UbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // UserToDTO Helper
    private static MdUsersDto UserToDTO(MdUsers mdUser) =>
        new MdUsersDto
        {
            Id = mdUser.Id,
            Login = mdUser.Login,
            Password = mdUser.Password
        };

    public async Task<IEnumerable<MdUsersDto>> GetMdUsers()
    {
        return await _context.mdUsers.Select(x => UserToDTO(x)).ToListAsync();
    }    

    public async Task<MdUsersDto> GetMdUser(long id)
    {
        var mdUsers = await _context.mdUsers.FindAsync(id);

        if (mdUsers == null)
        {
            return null;
        }

        return UserToDTO(mdUsers);
    }
    
    public async Task<MdUsersDto> PutMdUser(long id, MdUsersDto mdUsersDto)
    {
        var mdUsers = await _context.mdUsers.FindAsync(id);

        if(mdUsers != null)
        {
            mdUsers.Login = mdUsersDto.Login;
            mdUsers.Password = mdUsersDto.Password;
        }
        else
        {
            return null;
        }

        await _context.SaveChangesAsync();
        return UserToDTO(mdUsers);
    }

    public async Task<MdUsersDto> PostMdUser(MdUsersDto mdUsersDto)
    {
        var mdUser = new MdUsers
        {
            Login = mdUsersDto.Login,
            Password = mdUsersDto.Password
        };

        _context.mdUsers.Add(mdUser);
        await _context.SaveChangesAsync();

        return UserToDTO(mdUser);
    }

    public async Task<MdUsersDto> DeleteMdUser(long id)
    {
        var mdUser = await _context.mdUsers.FindAsync(id);
        
        if (mdUser == null)
        {
            return null;
        }

        _context.mdUsers.Remove(mdUser);
        await _context.SaveChangesAsync();

        return UserToDTO(mdUser);
    }
}