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
    // Task<MdUsersDto> PutMdUser(long id, MdUsersDto mdUsersDto);
    // void PostMdUsers(MdUsersDto mdUsersDto);
    // void DeleteMdUsers(long id);
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
    
    // public async PutMdUser(long id, MdUsersDto mdUsersDto)
    // {
        
    // }
}