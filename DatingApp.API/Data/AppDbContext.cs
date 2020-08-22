using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Value> Values { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Like> Likes { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Value>().ToTable("Values");
            builder.Entity<User>().ToTable("Users");
            builder.Entity<Photo>().ToTable("Photos");
            builder.Entity<Like>().ToTable("Likes").Property(x => x.Id).HasDefaultValueSql("NEWID()"); ;

            builder.Entity<Like>().HasKey(k => new { k.LikerId, k.LikeeId });

            builder.Entity<Like>()
                    .HasOne(u => u.Likee)
                    .WithMany(u => u.Likers)
                    .HasForeignKey(u => u.LikeeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                    .HasOne(u => u.Liker)
                    .WithMany(u => u.Likees)
                    .HasForeignKey(u => u.LikerId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}