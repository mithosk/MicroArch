using System.Threading;
using System.Threading.Tasks;

namespace StoryService.Data.Interfaces
{
    public interface IDataContext : INoSaveDataContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}