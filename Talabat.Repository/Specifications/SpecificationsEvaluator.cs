using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Specifications;
public class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
    {   
        var query = inputQuery; // _context.Set<Product>()

        if (spec.Criteira != null)
            query = query.Where(spec.Criteira);
        // _context.Set<Product>().Where(p=>p.id = 10)

        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);
        // _context.Set<Product>().OrderBy(p=>p.Name)

        if (spec.OrderByDesc != null)
            query = query.OrderByDescending(spec.OrderByDesc);
        // _context.Set<Product>().OrderByDescending(p=>p.Name)

        if (spec.IsPaginationEnable)
            query = query.Skip(spec.Skip).Take(spec.Take);

        query = spec.Include.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
        //_context.products.Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() ;
        //_context.products.Where(P => P.Id == id).OrderBy(p=>=p.Name).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() ;

        return query;

    }
}
