namespace Chatapi.Models;
using Microsoft.EntityFrameworkCore;

public class ChatContext : DbContext
{
    public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
}

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Sentiment { get; set; } = string.Empty;
    public float Score { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
