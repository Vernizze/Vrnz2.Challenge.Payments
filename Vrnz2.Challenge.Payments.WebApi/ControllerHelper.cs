using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vrnz2.Challenge.Payments.Shared.Validations;
using Vrnz2.Challenge.Payments.WebApi.CustomResults;
using Vrnz2.Challenge.ServiceContracts.ErrorMessageCodes;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models.Base;

namespace Vrnz2.Challenge.Payments.WebApi
{
    public class ControllerHelper
    {
        #region Variables

        private readonly ILogger _logger;
        private readonly ValidationHelper _validationHelper;

        #endregion

        #region Constructors

        public ControllerHelper(ILogger logger, ValidationHelper validationHelper)
        {
            _logger = logger;
            _validationHelper = validationHelper;
        }

        #endregion

        #region Methods

        public async Task<ObjectResult> ReturnAsync<TRequest, TResult>(Func<TRequest, Task<TResult>> action, TRequest request)
            where TRequest : BaseRequestModel
        {
            try
            {
                var validation = _validationHelper.Validate(request);

                if (validation.IsValid)
                {
                    var response = await action(request);

                    return new OkObjectResult(response);
                }
                else if (validation.ErrorCodes.Contains(ErrorMessageCodesFactory.UNEXPECTED_ERROR))
                {
                    return new InternalServerErrorObjectResult(validation.ErrorCodes);
                }
                else
                {
                    return new BadRequestObjectResult(validation.ErrorCodes);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error! - Message: {ex.Message}", ex);

                return new InternalServerErrorObjectResult(new List<string> { $"Unexpected error! - Message: {ex.Message}" });
            }
        }

        #endregion
    }
}
