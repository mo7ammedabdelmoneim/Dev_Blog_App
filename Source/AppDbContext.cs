
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Source.Models;

namespace source
{
    public class ApplicationContext: IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<PostReacts> PostReactes { get; set; }

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PostReacts>()
                .HasKey(pc => new { pc.PostId, pc.UserId }); // Define composite key


            builder.Entity<ApplicationUser>()
                .HasMany(p => p.Posts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction );

            builder.Entity <IdentityRole >()
                .HasData(new List <IdentityRole >()
                {
                    new IdentityRole{Name = "user" ,NormalizedName = "USER"},
                    new IdentityRole{Name  = "manage_posts" ,NormalizedName = "MANAGE_POSTS"},
                    new IdentityRole{Name = "admin" ,NormalizedName = "ADMIN"},
                });
        }
    }
}
