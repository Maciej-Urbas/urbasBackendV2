using Microsoft.EntityFrameworkCore;
using urbasBackendV2.Models;

namespace urbasBackendV2.Helpers;

public class UbContext: DbContext
{
    public UbContext(DbContextOptions<UbContext> options) : base(options)
    {
    }

    public DbSet<MdUsers> mdUsers { get; set; } = null!;
    public DbSet<MdMessages> mdMessages { get; set; } = null!;
}