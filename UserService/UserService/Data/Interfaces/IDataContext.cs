using System.Threading;
using System.Threading.Tasks;

namespace UserService.Data.Interfaces
{
    public interface IDataContext : INoSaveDataContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}