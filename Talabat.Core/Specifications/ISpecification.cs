using Talabat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications;
public interface ISpecification<T> where T : BaseEntity
{
    //_context.products.Where(P => P.Id == id).FirstOrDefaultAsync() ;
    public Expression<Func<T,bool>> Criteira { get; set; }

    //_context.products.Include(P => P.Brand).Include(P => P.Category).ToListAsync();
    public List<Expression<Func<T, object>>> Include { get; set; }

    public Expression<Func<T, object>> OrderBy { get; set; }

    public Expression<Func<T, object>> OrderByDesc { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    public bool IsPaginationEnable { get; set; }


}
