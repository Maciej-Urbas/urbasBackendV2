using urbasBackendV2.Enums;

namespace urbasBackendV2.Dtos;

public class MdMessagesDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Topic { get; set; }

    public string? Content { get; set; }

    public string? IpAddress { get; set; }

    public State State { get; set; }
}