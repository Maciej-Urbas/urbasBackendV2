namespace urbasBackendV2.Models;

using urbasBackendV2.Enums;

public class MdMessages
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Email  { get; set; }

    public string? Topic { get; set; }

    public string? Content { get; set; }

    public string? IpAddress { get; set; }

    // State enum
    public State State { get; set; } 
}