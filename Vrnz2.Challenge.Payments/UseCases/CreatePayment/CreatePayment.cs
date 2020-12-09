using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Vrnz2.Challenge.Payments.Shared.Entities;
using Vrnz2.Challenge.Payments.Shared.Queues;
using Vrnz2.Challenge.Payments.Shared.Settings;
using Vrnz2.Challenge.ServiceContracts.Notifications;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;

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

            await SendToMongo(customer);

            await SendToQueue(_mapper.Map<PaymentNotification.Created>(request));

            return new CreatePaymentModel.Response
            {
                Success = true,
                Message = "Success",
                Tid = customer.Tid
            };
        }

        public virtual async Task SendToMongo(Payment payment) 
        {
            using (var mongo = new Data.MongoDB.MongoDB(_connectionStringsSettings.MongoDbChallenge, MONGODB_COLLECTION, MONGODB_DATABASE))
                await mongo.Add(payment);
        }

        public virtual async Task SendToQueue(PaymentNotification.Created notification)
            => await _queueHandler.Send(_mapper.Map<PaymentNotification.Created>(notification), _queuesSettings.PaymentCreatedQueueName);

        #endregion
    }
}
