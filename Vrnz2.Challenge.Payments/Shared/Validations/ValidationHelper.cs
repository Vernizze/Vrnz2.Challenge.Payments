using FluentValidation;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using Vrnz2.Challenge.ServiceContracts.ErrorMessageCodes;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models.Base;
using Vrnz2.Infra.Crosscutting.Extensions;

namespace Vrnz2.Challenge.Payments.Shared.Validations
{
    public class ValidationHelper
    {
        #region Variables

        private readonly ILogger _logger;
        private readonly IValidatorFactory _validatorFactory;

        #endregion 

        #region Variables

        public ValidationHelper(ILogger logger, IValidatorFactory validatorFactory)
        {
            _logger = logger;
            _validatorFactory = validatorFactory;
        }

        #endregion 

        #region Methods

        public (bool IsValid, List<string> ErrorCodes) Validate<T>(T request)
            where T : BaseRequestModel
        {
            try
            {
                var validator = _validatorFactory.GetValidator(request.GetType());

                if (validator.IsNull())
                    return (false, new List<string> { string.Empty });

                var context = new ValidationContext<T>(request);

                var validationResult = validator.Validate(context);

                return (validationResult.IsValid, validationResult.Errors.Select(e => ErrorMessageCodesFactory.Instance.GetMessage(e.ErrorMessage)).ToList());
            }
            catch (System.Exception ex)
            {
                _logger.Error($"Unexpected error! - Message: {ex.Message}", ex);

                return (false, new List<string> { ErrorMessageCodesFactory.UNEXPECTED_ERROR });
            }
        }

        #endregion 
    }
}
