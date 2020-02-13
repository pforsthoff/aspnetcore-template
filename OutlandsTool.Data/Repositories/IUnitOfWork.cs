using System.Threading.Tasks;

namespace OutlandsTool.Data.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> CommitAsync();
        void Commit();

        void Rollback();
    }
}
