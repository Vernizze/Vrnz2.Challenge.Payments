using FluentValidation;
using Vrnz2.Challenge.ServiceContracts.ErrorMessageCodes;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;

namespace Vrnz2.Challenge.Payments.UseCases.GetPayment
{
    public class RequestValidator
        : AbstractValidator<GetPaymentModel.Request>
    {
        public RequestValidator()
        {
            RuleFor(v => v)
                .Must(IsValid)
                .WithMessage(ErrorMessageCodesFactory.INVALID_ITR_ERROR);
        }

        private bool IsValid(GetPaymentModel.Request request)
            => request.IsValid();
    }
}
