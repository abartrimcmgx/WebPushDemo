using Microsoft.EntityFrameworkCore;
using WebPushDemo.Models;

namespace WebPushDemo.Data
{
    public class WebPushDemoContext(DbContextOptions<WebPushDemoContext> options) : DbContext(options)
    {
        public DbSet<Devices> Devices { get; init; } = default!;
    }
}