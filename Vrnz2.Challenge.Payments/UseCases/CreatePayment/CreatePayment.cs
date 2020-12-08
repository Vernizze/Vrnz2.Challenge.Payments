using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vrnz2.Challenge.Payments.Shared.Entities;
using Vrnz2.Challenge.Payments.Shared.Queues;
using Vrnz2.Challenge.Payments.Shared.Settings;
using Vrnz2.Challenge.ServiceContracts.Notifications;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;
using Vrnz2.Infra.CrossCutting.Extensions;
using Vrnz2.Infra.CrossCutting.Types;

namespace Vrnz2.Challenge.Payments.UseCases.CreatePayment
{
    public class CreatePayment
        : IRequestHandler<CreatePaymentModel.Request, CreatePaymentModel.Response>
    {
        #region Variables

        private const string MONGODB_COLLECTION = "Payment";
        private const string MONGODB_DATABASE = "Challenge";

        #endregion

        #region Variables

        private readonly ConnectionStringsSettings _connectionStringsSettings;
        private readonly QueuesSettings _queuesSettings;
        private readonly GetPayment.GetPayment _getPayment;
        private readonly IMapper _mapper;
        private readonly QueueHandler _queueHandler;

        #endregion

        #region Constructor

        public CreatePayment(IOptions<ConnectionStringsSettings> connectionStringsOptions, IOptions<QueuesSettings> queuesOptionsSettings, GetPayment.GetPayment getPayment, IMapper mapper, QueueHandler queueHandler)
        {
            _connectionStringsSettings = connectionStringsOptions.Value;
            _queuesSettings = queuesOptionsSettings.Value;
            _getPayment = getPayment;
            _mapper = mapper;
            _queueHandler = queueHandler;
        }

        #endregion

        #region Methods

        public async Task<CreatePaymentModel.Response> Handle(CreatePaymentModel.Request request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Payment>(request);

            using (var mongo = new Data.MongoDB.MongoDB(_connectionStringsSettings.MongoDbChallenge, MONGODB_COLLECTION, MONGODB_DATABASE))
                await mongo.Add(customer);

            await _queueHandler.Send(_mapper.Map<PaymentNotification.Created>(request), _queuesSettings.CustomerCreatedQueueName);

            return new CreatePaymentModel.Response
            {
                Success = true,
                Message = "Success",
                Tid = customer.Tid
            };
        }

        #endregion
    }
}
