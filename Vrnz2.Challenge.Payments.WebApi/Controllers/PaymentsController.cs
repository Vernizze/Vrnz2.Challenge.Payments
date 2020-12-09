using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Threading.Tasks;
using Vrnz2.Challenge.Payments.Shared.Validations;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;

namespace Vrnz2.Challenge.Payments.WebApi.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController 
        : ControllerBase
    {
        #region Variables

        private readonly ILogger _logger;
        private readonly ControllerHelper _controllerHelper;
        private readonly ValidationHelper _validationHelper;
        private readonly IMediator _mediator;

        #endregion

        #region Constructors

        public PaymentsController(ILogger logger, ControllerHelper controllerHelper, ValidationHelper validationHelper, IMediator mediator)
        {
            _logger = logger;
            _controllerHelper = controllerHelper;
            _validationHelper = validationHelper;
            _mediator = mediator;
        }

        #endregion

        #region Methods

        /// <summary>
        /// [Post] Creation Payment end point
        /// </summary>
        /// <param name="request">Cpf, DueDate (Valid Date/Time) and Value (Money value in format '0,000.00')</param>
        /// <returns>Http Status Code 'OK' with content => Success (True/False), Message and Tid (Transaction Id) of Operation</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreatePaymentModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post(CreatePaymentModel.Request request)
            => await _controllerHelper.ReturnAsync((request) => _mediator.Send(request), request);

        /// <summary>
        /// [GET] Get Payments data end point
        /// </summary>
        /// <param name="cpf">Valid Customer Cpf AND/OR</param>
        /// <param name="monthRefDueDate">Valid Month + Year Due Date parameter (expected format => "yyyy-MM")</param>
        /// <returns>Http Status Code 'OK' with content => List of Payemnts containning Tid (Transaction Id) of Operation, Cpf, Due Date and Value</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetPaymentModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] string cpf, string monthRefDueDate)
        {
            var request = new GetPaymentModel.Request(cpf, monthRefDueDate);

            return await _controllerHelper.ReturnAsync((request) => _mediator.Send(request), request);
        }

        /// <summary>
        /// [GET] Get Payments resume of a Customer data end point
        /// </summary>
        /// <param name="cpf">Valid Customer Cpf AND/OR</param>
        /// <param name="monthRefPaymentDate">Valid Month + Year Due Date parameter (expected format => "yyyy-MM")</param>
        /// <returns>Http Status Code 'OK' with content => List of Payemnts containning Cpf, Payments Period (format yyyy-MM) and Total Value</returns>
        [HttpGet("customer")]
        [ProducesResponseType(typeof(GetCustomerPaymentsModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCustomerPaymentos([FromQuery] string cpf, string monthRefPaymentDate)
        {
            var request = new GetCustomerPaymentsModel.Request(cpf, monthRefPaymentDate);

            return await _controllerHelper.ReturnAsync((request) => _mediator.Send(request), request);
        }

        #endregion
    }
}
