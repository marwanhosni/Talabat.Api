using Talabat.Core.Entities;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Products_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Talabat.Core.Products_Specs
{
    public class ProductWithBrandAndCategorySpecification : BaseSpecification<Product>
    {
        //This Constractor Will Be Used For Creating Object For GetAll Products
        public ProductWithBrandAndCategorySpecification(ProductsSpecParams productSpec) : 
            base(p=>
            (string.IsNullOrEmpty(productSpec.Search) || p.Name.Contains(productSpec.Search))
            &&
            (!productSpec.BrandId.HasValue || p.BrandId == productSpec.BrandId.Value)
            &&
            (!productSpec.CategoryId.HasValue || p.CategoryId == productSpec.CategoryId.Value)
            )
        {
            Include.Add(P => P.Brand);
            Include.Add(P => P.Category);

            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort) 
                { 
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;  
                    case "pricedesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }

            //Total = 1000;
            //pageIndex = 9 
            //pageSize = 50

            ApplyPagination(productSpec.PageSize * (productSpec.PageIndex - 1), productSpec.PageSize); 
        }

        public ProductWithBrandAndCategorySpecification(int id) : base(P => P.Id == id)
        {
            Include.Add(P => P.Brand);
            Include.Add(P => P.Category);
        }
    }
}
