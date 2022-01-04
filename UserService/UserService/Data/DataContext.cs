using AgileServiceBus.Additionals;
using Microsoft.EntityFrameworkCore;
using System;
using UserService.Data.Interfaces;
using UserService.Data.Models;

namespace UserService.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public Guid SessionId { get; set; }

        public virtual DbSet<Story> Stories { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public DataContext()
        {
            SessionId = Guid.NewGuid();
        }

        public DataContext(DbContextOptions<DataContext> dataOptions) : base(dataOptions)
        {
            SessionId = Guid.NewGuid();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Story>(etb =>
            {
                etb.HasIndex(sto => sto.ExternalId).IsUnique();
            });

            builder.Entity<User>(etb =>
            {
                etb.HasIndex(use => use.ExternalId).IsUnique();
                etb.HasIndex(use => use.Email).IsUnique();
                etb.HasIndex(use => use.AccessKey).IsUnique();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
                builder.UseSqlServer(Env.Get("SQLSERVER_CONN_STR")).UseLazyLoadingProxies();
        }
    }
}