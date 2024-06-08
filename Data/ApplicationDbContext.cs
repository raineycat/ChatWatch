using ChatWatchApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatWatchApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Player { get; set; }
    public DbSet<ChatMessage> ChatMessage { get; set; }
    public DbSet<PrivateMessage> PrivateMessage { get; set; }
}
