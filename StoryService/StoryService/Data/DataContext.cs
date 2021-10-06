using AgileServiceBus.Additionals;
using Microsoft.EntityFrameworkCore;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using System;

namespace StoryService.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public Guid SessionId { get; set; }

        public virtual DbSet<Story> Stories { get; set; }

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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
                builder.UseSqlServer(Env.Get("SQLSERVER_CONN_STR")).UseLazyLoadingProxies();
        }
    }
}