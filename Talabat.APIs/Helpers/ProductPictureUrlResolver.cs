using AutoMapper;
using AutoMapper.Execution;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers;

public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
{
    private readonly IConfiguration _cong;

    public ProductPictureUrlResolver(IConfiguration cong)
    {
        _cong = cong;
    }
    public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
        {
            return $"{_cong["BaseUrl"]}/{source.PictureUrl}";
        }
        else 
        {
            return string.Empty;
        }
    }
}
