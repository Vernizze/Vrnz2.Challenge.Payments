using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using CreatePayment = Vrnz2.Challenge.Payments.UseCases.CreatePayment;
using GetPayment = Vrnz2.Challenge.Payments.UseCases.GetPayment;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;

namespace Vrnz2.Challenge.Payments.Shared.Validations
{
    public static class ValiationServiceExtensions
    {
        public static IServiceCollection AddValidations(this IServiceCollection services)
            => services
                .AddScoped<IValidatorFactory, ValidatorFactory>()
                .AddScoped<ValidationHelper>()
                .AddTransient<IValidator<CreatePaymentModel.Request>, CreatePayment.RequestValidator>()
                .AddTransient<IValidator<GetPaymentModel.Request>, GetPayment.RequestValidator>();
    }
}
