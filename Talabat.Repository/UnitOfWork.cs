using Talabat.Core.Entities;
using Talabat.Repository.Implementations;
using Talabat.Repository.Interfaces;

namespace Talabat.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }

    public IGenaricRepository<TEntity> Repository<TEntity>()
        where TEntity : BaseEntity
    {
        var entityType = typeof(TEntity);

        if (!_repositories.ContainsKey(entityType))
        {
            var repository = new GenaricRepository<TEntity>(_context);

            _repositories.Add(entityType, repository);
        }

        return (IGenaricRepository<TEntity>)_repositories[entityType];
    }
}