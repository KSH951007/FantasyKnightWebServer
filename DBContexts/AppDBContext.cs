
using FantasyKnightWebServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Principal;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
    public DbSet<UserAccountDBData> users { get; set; }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseMySql(
    //        "Server=127.0.0.1;Database=FantasyKnightDB;User=root;Password=3900;",
    //        new MySqlServerVersion(new Version(8, 0, 29))
    //    );
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAccountDBData>().ToTable("account");
        modelBuilder.Entity<UserAccountDBData>().HasKey(user => user.UUID);
    }
}

