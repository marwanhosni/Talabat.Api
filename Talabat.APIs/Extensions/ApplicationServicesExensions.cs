using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Repository;
using Talabat.Repository.Implementations;
using Talabat.Repository.Interfaces;
using Talabat.Core.Services.Interface;
using Talabat.Service;
namespace Talabat.APIs.Extensions;

public static class ApplicationServicesExensions
{
    public static IServiceCollection ApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentServices, PaymentServices>();
        services.AddAutoMapper(typeof(MappingProfiles));
        services.Configure<ApiBehaviorOptions>(ApiBehaviorOptions =>
        {
            ApiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();
                var errorResponse = new ApiValidationResponse(_statusCode: 400, errors);
                return new BadRequestObjectResult(errorResponse);
            };
        });

        return services;
    }
}
