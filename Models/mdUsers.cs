namespace urbasBackendV2.Models;

public class MdUsers
{
    public int Id { get; set; }
    
    public string? Login { get; set; }

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }
}