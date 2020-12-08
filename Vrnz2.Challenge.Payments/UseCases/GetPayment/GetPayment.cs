using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vrnz2.Challenge.Payments.Shared.Entities;
using Vrnz2.Challenge.Payments.Shared.Settings;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;
using Vrnz2.Infra.CrossCutting.Extensions;

namespace Vrnz2.Challenge.Payments.UseCases.GetPayment
{
    public class GetPayment
        : IRequestHandler<GetPaymentModel.Request, GetPaymentModel.Response>
    {
        #region Variables

        private const string MONGODB_COLLECTION = "Payment";
        private const string MONGODB_DATABASE = "Challenge";

        #endregion

        #region Variables

        private readonly ConnectionStringsSettings _connectionStringsSettings;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public GetPayment(IOptions<ConnectionStringsSettings> connectionStringsOptions, IMapper mapper)
        {
            _connectionStringsSettings = connectionStringsOptions.Value;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<GetPaymentModel.Response> Handle(GetPaymentModel.Request request, CancellationToken cancellationToken)
        {
            var payments = await GetPayments(request);

            return _mapper.Map<GetPaymentModel.Response>(payments);
        }

        public async Task<List<Payment>> GetPayments(GetPaymentModel.Request request)
        {
            List<Payment> result;

            using (var mongo = new Data.MongoDB.MongoDB(_connectionStringsSettings.MongoDbChallenge, MONGODB_COLLECTION, MONGODB_DATABASE))
            {
                var res = await mongo.GetMany<Payment>(GetFilter(request));

                result = (res).ToList();
            }

            return result;
        }

        private string GetFilter(GetPaymentModel.Request request)
        {
            var result = string.Empty;
            var iniDate = !string.IsNullOrEmpty(request.DueDate) ? $"ISODate(\"{request.DueDate.FirstDayOfMonth().Value.ToString("yyyy-MM-ddTHH:mm:ss")}\")" : string.Empty;
            var endDate = !string.IsNullOrEmpty(request.DueDate) ? $"ISODate(\"{request.DueDate.LastDayOfMonth().Value.ToString("yyyy-MM-ddTHH:mm:ss")}\")" : string.Empty;

            if (!string.IsNullOrEmpty(request.Cpf) && !string.IsNullOrEmpty(request.DueDate))
                result = $"{{ \"Cpf\": \"{request.Cpf}\", \"DueDate\": {{$gte: {iniDate}, $lte: {endDate}}}}}";
            else if (!string.IsNullOrEmpty(request.Cpf))
                result = $"{{ \"Cpf\": \"{request.Cpf}\"}}";
            else
                result = $"{{\"DueDate\": {{$gte: {iniDate}, $lte: {endDate}}}}}";

            return result;
        }

        public bool IsNew(CreatePaymentModel.Request request)
        {
            var result = false;

            if (request.IsValid())
            {
                var refDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                var payments = GetPayments(new GetPaymentModel.Request { Cpf = request.Cpf, DueDate = refDate }).Result;

                var payment = _mapper.Map<Payment>(request);

                var found = payments.Where(p => p.Equals(payment));

                result = !payments.HaveAny() || (payments.HaveAny() && !found.HaveAny());
            }

            return result;
        }

        #endregion
    }
}
