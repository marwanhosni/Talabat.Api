using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Interfaces;

public interface IGenaricRepository<T> where T : BaseEntity
{
    
     Task<T?> GetByIdAsync(int id);
     Task<T?> GetByIdWithSpecAsync(ISpecification<T> spec);

    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

    Task<int> GetCountAsync(ISpecification<T> spec);


    Task Add(T item);
    void Update(T item);
    void Delete(T item);
}
