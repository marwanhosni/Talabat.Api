using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Products_Specs;
using Talabat.Core.Specifications.Products_Specs;
using Talabat.Repository.Interfaces;

namespace Talabat.APIs.Controllers;

public class ProductController : BaseApiController
{
    private readonly IGenaricRepository<Product> _productRepo;
    private readonly IMapper _mapper;
    private readonly IGenaricRepository<ProductBrand> _brandsRepo;
    private readonly IGenaricRepository<ProductCategory> _categoriesRepo;

    public ProductController(IGenaricRepository<Product> productRepo , IMapper mapper,IGenaricRepository<ProductBrand> brandsRepo , IGenaricRepository<ProductCategory> categoriesRepo)
    {
        _productRepo = productRepo;
        _mapper = mapper;
        _brandsRepo = brandsRepo;
        _categoriesRepo = categoriesRepo;
    }
    // If Number Of Barams Increase Than 5 Params ==> Make A class And put Params In It 
    // Default Schema Is Bearer Not Cookie
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetAll([FromQuery]ProductsSpecParams productSpec)
    {
        //var products = await _productRepo.GetAllAsync();
        var Spec = new ProductWithBrandAndCategorySpecification(productSpec);
        var products = await _productRepo.GetAllWithSpecAsync(Spec);
        var result = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
        var countSpec = new ProductWithCountSpecifications(productSpec);
        var count = await _productRepo.GetCountAsync(countSpec);
        return Ok(new Paginations<ProductToReturnDto>(productSpec.PageSize, productSpec.PageIndex, count, result));
    }

    [Authorize(AuthenticationSchemes ="Bearer")]
    [HttpGet(template: "{id}")]
    public async Task<ActionResult<ProductToReturnDto>> GetById(int Id)
    {
        //var product = await _productRepo.GetByIdAsync(Id);
        var spec = new ProductWithBrandAndCategorySpecification(Id);
        var product = await _productRepo.GetByIdWithSpecAsync(spec);
        if (product == null)
        {
            return NotFound(new ApiRequest(404));
        }
        var result = _mapper.Map<Product, ProductToReturnDto>(product);
        return Ok(result);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
    {
      var brands =  await _brandsRepo.GetAllAsync();
        return Ok(brands);
    }    

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
    {
      var categries =  await _categoriesRepo.GetAllAsync();
        return Ok(categries);
    }    
    
    
}
