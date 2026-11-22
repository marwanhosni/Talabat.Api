using Talabat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Products_Specs;
public class ProductWithCountSpecifications : BaseSpecification<Product>
{
    //This Constractor Will Be Used For Creating Object For GetAll Products
    public ProductWithCountSpecifications(ProductsSpecParams productSpec) :
        base(p => (!productSpec.BrandId.HasValue || p.BrandId == productSpec.BrandId.Value)
        &&
        (!productSpec.CategoryId.HasValue || p.CategoryId == productSpec.CategoryId.Value)
        )
    {

    }
}
