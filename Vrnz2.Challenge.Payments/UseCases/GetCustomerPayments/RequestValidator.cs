using FluentValidation;
using Vrnz2.Challenge.ServiceContracts.ErrorMessageCodes;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;

namespace Vrnz2.Challenge.Payments.UseCases.GetCustomerPayments
{
    public class RequestValidator
        : AbstractValidator<GetCustomerPaymentsModel.Request>
    {
        #region Variables

        private readonly GetPayment.GetPayment _getPayment;

        #endregion

        #region Constructors

        public RequestValidator()
        {
            RuleFor(v => v)
                .Must(IsValid)
                .WithMessage(ErrorMessageCodesFactory.INVALID_PAYMENT_SEARCH_QUERY_ERROR);
        }

        #endregion

        #region Methods

        private bool IsValid(GetCustomerPaymentsModel.Request request)
            => request.IsValid();

        #endregion
    }
}
