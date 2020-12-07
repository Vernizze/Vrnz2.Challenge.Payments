using FluentValidation;
using System;

namespace Vrnz2.Challenge.Payments.Shared.Validations
{
    public class ValidatorFactory
        : IValidatorFactory
    {
        #region Variables

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Constructors

        public ValidatorFactory(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        #endregion

        #region Methods

        public IValidator<T> GetValidator<T>()
            => _serviceProvider.GetService(typeof(IValidator<T>)) as IValidator<T>;

        public IValidator GetValidator(Type type)
        {
            var generic = typeof(IValidator<>);
            var specific = generic.MakeGenericType(type);

            return _serviceProvider.GetService(specific) as IValidator;
        }

        #endregion
    }
}
