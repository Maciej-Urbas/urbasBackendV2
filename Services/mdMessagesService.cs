using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

using urbasBackendV2.Dtos;
using urbasBackendV2.Helpers;
using urbasBackendV2.Models;

namespace urbasBackendV2.Services;

public interface IMdMessagesService
{
    Task<IEnumerable<MdMessagesDto>> GetMdMessages();
    Task<MdMessagesDto> GetMdMessage(int id);
    Task<MdMessagesDto> PutMdMessage(int id, MdMessagesDto mdMessagesDto);
    Task<MdMessagesDto> PostMdMessage(MdMessagesDto mdMessagesDto);
    Task<MdMessagesDto> DeleteMdMessage(int id);
}

public class MdMessagesService: IMdMessagesService
{
    private UbContext _context;
    
    public MdMessagesService(UbContext context)
    {
        _context = context;
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
            IpAddress = mdMessages.IpAddress,
            State = Enums.State.recived,
        };

    public async Task<IEnumerable<MdMessagesDto>> GetMdMessages()
    {
        return await _context.mdMessages.Select(x => MessageToDTO(x)).ToListAsync();
    }    

    public async Task<MdMessagesDto> GetMdMessage(int id)
    {
        var mdMessage = await _context.mdMessages.FindAsync(id);

        return MessageToDTO(mdMessage);
    }
    
    public async Task<MdMessagesDto> PutMdMessage(int id, MdMessagesDto mdMessagesDto)
    {
        var hmac = new HMACSHA512();

        var mdMessage = await _context.mdMessages.FindAsync(id);
        mdMessage.Name = mdMessagesDto.Name;
        mdMessage.Email = mdMessagesDto.Email;
        mdMessage.Topic = mdMessagesDto.Topic;
        mdMessage.Content = mdMessagesDto.Content;
        mdMessage.IpAddress = mdMessagesDto.IpAddress;
        mdMessage.State = mdMessagesDto.State;

        await _context.SaveChangesAsync();
        return MessageToDTO(mdMessage);
    }

    public async Task<MdMessagesDto> PostMdMessage(MdMessagesDto mdMessagesDto)
    {
        var hmac = new HMACSHA512();
        
        var mdMessage = new MdMessages
        {
            Name = mdMessagesDto.Name,
            Email = mdMessagesDto.Email,
            Topic = mdMessagesDto.Topic,
            Content = mdMessagesDto.Content,
            IpAddress = mdMessagesDto.IpAddress,
            State = mdMessagesDto.State,
        };

        _context.mdMessages.Add(mdMessage);
        await _context.SaveChangesAsync();

        return MessageToDTO(mdMessage);
    }

    public async Task<MdMessagesDto> DeleteMdMessage(int id)
    {
        var mdMessage = await _context.mdMessages.FindAsync(id);

        _context.mdMessages.Remove(mdMessage);
        await _context.SaveChangesAsync();

        return MessageToDTO(mdMessage);
    }   
}