using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;

namespace Vrnz2.Challenge.Payments.Shared.Validations
{
    public static class ValiationServiceExtensions
    {
        public static IServiceCollection AddValidations(this IServiceCollection services)
            => services
                .AddScoped<IValidatorFactory, ValidatorFactory>()
                .AddScoped<ValidationHelper>();
                //.AddTransient<IValidator<CreateCustomerModel.Request>, CreateCustomer.RequestValidator>();
    }
}
