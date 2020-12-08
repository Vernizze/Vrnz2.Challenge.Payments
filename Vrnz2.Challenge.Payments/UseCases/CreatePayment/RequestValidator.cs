using FluentValidation;
using Vrnz2.Challenge.ServiceContracts.ErrorMessageCodes;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;

namespace Vrnz2.Challenge.Payments.UseCases.CreatePayment
{
    public class RequestValidator
        : AbstractValidator<CreatePaymentModel.Request>
    {
        #region Variables

        private readonly GetPayment.GetPayment _getPayment;

        #endregion

        #region Constructors

        public RequestValidator(GetPayment.GetPayment getPayment)
        {
            _getPayment = getPayment;

            RuleFor(v => v)
                .Must(IsValid)
                .WithMessage(ErrorMessageCodesFactory.INVALID_PAYMENT_CREATION_COMMAND_ERROR);

            RuleFor(v => v)
                .Must(IsNew)
                .WithMessage(ErrorMessageCodesFactory.PAYMENT_ALREADY_EXISTS_ERROR);
        }

        #endregion

        #region Methods

        private bool IsValid(CreatePaymentModel.Request value)
            => value.IsValid();

        private bool IsNew(CreatePaymentModel.Request value)
            => _getPayment.IsNew(value);

        #endregion
    }
}
