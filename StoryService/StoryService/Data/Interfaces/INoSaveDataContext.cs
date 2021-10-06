using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StoryService.Data.Models;
using System;

namespace StoryService.Data.Interfaces
{
    public interface INoSaveDataContext : IDisposable
    {
        Guid SessionId { get; set; }

        DbSet<Story> Stories { get; set; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}