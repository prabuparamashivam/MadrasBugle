using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RandomBugleDB.Models.Comments;

namespace RandomBugleDB.Models
{
    public class RandomBugleDBContext : IdentityDbContext
    {

        public RandomBugleDBContext(DbContextOptions<RandomBugleDBContext> options)
: base(options)
        { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<MainComment> MainComments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=SQL8002.site4now.net;Initial Catalog=db_a93f1d_madrasbugle01;User Id=db_a93f1d_madrasbugle01_admin;Password=MadrasBugle@1");
              //  optionsBuilder.UseSqlServer("Data Source =WARLOCK\\MSSQLSERVER01;Database=RandomBugle;Trusted_Connection=True");

            }
        }


        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RandomBugleDBContext>
        {
            public RandomBugleDBContext CreateDbContext(string[] args)
            {
                //IConfigurationRoot configuration = new ConfigurationBuilder()
                //    .SetBasePath(Directory.GetCurrentDirectory())
                //    .AddJsonFile("appsettings.json")
                //    .Build();

                var builder = new DbContextOptionsBuilder<RandomBugleDBContext>();

                //var connectionString = configuration.GetConnectionString("DefaultConnection");

                builder.UseSqlServer("Data Source=SQL8002.site4now.net;Initial Catalog=db_a93f1d_madrasbugle01;User Id=db_a93f1d_madrasbugle01_admin;Password=MadrasBugle@1");
              //  builder.UseSqlServer("Data Source =WARLOCK\\MSSQLSERVER01;Database=RandomBugle;Trusted_Connection=True");

                return new RandomBugleDBContext(builder.Options);
            }
        }


    }
}
