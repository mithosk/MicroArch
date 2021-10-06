using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using UserService.Data.Models;

namespace UserService.Data.Interfaces
{
    public interface INoSaveDataContext : IDisposable
    {
        Guid SessionId { get; set; }

        DbSet<Story> Stories { get; set; }
        DbSet<User> Users { get; set; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}