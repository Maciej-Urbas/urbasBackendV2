using Microsoft.EntityFrameworkCore;
using urbasBackendV2.Models;

namespace urbasBackendV2.Helpers;

public class MdContext: DbContext
{
    public MdContext(DbContextOptions<MdContext> options) : base(options)
    {
    }

    public DbSet<MdUser> users { get; set; } = null!;
}