using Blog.Entities.Models;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>()
               .Metadata
               .FindNavigation(nameof(Article.Votes))
               .SetPropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<User>()
               .Metadata
               .FindNavigation(nameof(User.Votes))
               .SetPropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<User>()
              .Metadata
              .FindNavigation(nameof(User.Articles))
              .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
