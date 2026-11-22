using Talabat.Core.Entities;
using Talabat.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository;
public interface IUnitOfWork : IAsyncDisposable
{
    IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    Task<int> CompleteAsync(); 
}

