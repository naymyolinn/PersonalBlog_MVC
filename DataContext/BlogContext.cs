using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using personalblog.Models;
using personalblog.Models;

namespace personalblog.DataContext
{
    public class BlogContext : IdentityDbContext
    {
        public BlogContext() //constructor
        {

        }
        public BlogContext(DbContextOptions<BlogContext> options)
            : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer(@"Data Source=.\SqlExpress;Initial catalog=PersonalBlog;User Id=dbuser;Password=root");
        }


    }


}