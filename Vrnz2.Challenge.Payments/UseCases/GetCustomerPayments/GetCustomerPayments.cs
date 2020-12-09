using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vrnz2.Challenge.Payments.Shared.Entities;
using Vrnz2.Challenge.Payments.Shared.Settings;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;
using Vrnz2.Infra.CrossCutting.Extensions;
using Vrnz2.Infra.CrossCutting.Types;

namespace Vrnz2.Challenge.Payments.UseCases.GetCustomerPayments
{
    public class GetCustomerPayments
        : IRequestHandler<GetCustomerPaymentsModel.Request, GetCustomerPaymentsModel.Response>
    {
        #region Variables

        private const string MONGODB_COLLECTION = "CustomerPayments";
        private const string MONGODB_DATABASE = "Challenge";

        #endregion

        #region Variables

        private readonly ConnectionStringsSettings _connectionStringsSettings;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public GetCustomerPayments(IOptions<ConnectionStringsSettings> connectionStringsOptions, IMapper mapper)
        {
            _connectionStringsSettings = connectionStringsOptions.Value;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<GetCustomerPaymentsModel.Response> Handle(GetCustomerPaymentsModel.Request request, CancellationToken cancellationToken)
        {
            var cpf = string.Empty;
            var paymentPeriod = string.Empty;
            var totalValue = 0M;

            var payments = await GetPayments(request);

            if (payments.HaveAny()) 
            {
                var paymentRef = payments.FirstOrDefault();

                cpf = new Cpf(paymentRef.Cpf).FormatedValue;
                paymentPeriod = $"{paymentRef.YearReference}-{paymentRef.MonthReference}";
                totalValue = payments.Sum(s => s.Value);
            }            

            return new GetCustomerPaymentsModel.Response
            {
                Cpf = cpf,
                PaymentPeriod = paymentPeriod,
                TotalValue = totalValue
            };
        }

        public virtual async Task<List<CustomerConsumption>> GetPayments(GetCustomerPaymentsModel.Request request)
        {
            List<CustomerConsumption> result;

            using (var mongo = new Data.MongoDB.MongoDB(_connectionStringsSettings.MongoDbChallenge, MONGODB_COLLECTION, MONGODB_DATABASE))
            {
                var res = await mongo.GetMany<CustomerConsumption>(GetFilter(request));

                result = (res).ToList();
            }

            return result;
        }

        private string GetFilter(GetCustomerPaymentsModel.Request request)
        {
            var result = string.Empty;
            var iniDate = !string.IsNullOrEmpty(request.PaymentDate) ? $"ISODate(\"{request.PaymentDate.FirstDayOfMonth().Value.ToString("yyyy-MM-ddTHH:mm:ss")}\")" : string.Empty;
            var endDate = !string.IsNullOrEmpty(request.PaymentDate) ? $"ISODate(\"{request.PaymentDate.LastDayOfMonth().Value.ToString("yyyy-MM-ddTHH:mm:ss")}\")" : string.Empty;
            
            if (!string.IsNullOrEmpty(request.Cpf) && !string.IsNullOrEmpty(request.PaymentDate))
                result = $"{{ \"Cpf\": \"{new Cpf(request.Cpf).Value}\", \"PaymentDate\": {{$gte: {iniDate}, $lte: {endDate}}}}}";
            else if (!string.IsNullOrEmpty(request.Cpf))
                result = $"{{ \"Cpf\": \"{new Cpf(request.Cpf).Value}\"}}";
            else
                result = $"{{\"PaymentDate\": {{$gte: {iniDate}, $lte: {endDate}}}}}";

            return result;
        }

        #endregion
    }
}
