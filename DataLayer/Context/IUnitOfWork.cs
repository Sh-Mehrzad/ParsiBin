using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DataLayer
{
    public interface IUnitOfWork
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
