using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusiCom.Infrastructure.Data.Entities;
using MusiCom.Infrastructure.Data.Entities.Events;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<NewTags>()
                .HasKey(nt => new { nt.NewId, nt.TagId });

            builder.Entity<EventPost>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.EventPosts)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<NewComment>()
                .HasOne(nc => nc.New)
                .WithMany(n => n.NewComments)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.OnModelCreating(builder);
        }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<New> News { get; set; }

        public DbSet<NewTags> NewsTags { get; set; }

        public DbSet<NewComment> NewComments { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventPost> EventPosts { get; set; }
    }
}