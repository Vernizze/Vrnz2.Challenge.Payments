using AutoMapper;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vrnz2.Challenge.Payments.Shared.Entities;
using Vrnz2.Challenge.Payments.Shared.Settings;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;
using Vrnz2.Infra.CrossCutting.Types;
using Xunit;
using Get = Vrnz2.Challenge.Payments.UseCases.GetCustomerPayments;

namespace Vrnz2.Challenge.Payments.Test.UseCases.GetCustomerPayments
{
    public class GetCustomerPaymentsTest
    {
        private IMapper _mapper;
        private IOptions<ConnectionStringsSettings> _connectionStringsOptions;

        public GetCustomerPaymentsTest()
        {
            _connectionStringsOptions = Options.Create(new ConnectionStringsSettings
            {
                MongoDbChallenge = string.Empty
            });

            _mapper = Substitute.For<IMapper>();
        }

        private GetCustomerPaymentsMock GetInstance()
        {
            return new GetCustomerPaymentsMock(_connectionStringsOptions, _mapper);
        }

        [Fact]
        public async Task GetCustomerPayments_Handler_Test()
        {
            //Arrange           
            Cpf cpf = "434.443.474-99";
            var paymentDate = new DateTime(2020, 12, 01);
            var service = GetInstance();

            var request = new GetCustomerPaymentsModel.Request
            {
                Cpf = cpf.Value,
                PaymentDate = paymentDate.ToString("yyyy-MM-ddTHH:mm:ss")                
            };

            //Act
            var result = await service.Handle(request, new System.Threading.CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal("2020-12", result.PaymentPeriod);
            Assert.Equal(40M, result.TotalValue);
        }
    }

    public class GetCustomerPaymentsMock
        : Get.GetCustomerPayments
    {
        public GetCustomerPaymentsMock(IOptions<ConnectionStringsSettings> connectionStringsOptions, IMapper mapper) 
            : base(connectionStringsOptions, mapper)
        {
        }

        public override Task<List<CustomerConsumption>> GetPayments(GetCustomerPaymentsModel.Request request)
        {
            Cpf cpf = "434.443.474-99";
            var date = new DateTime(2020, 12, 01);

            List<CustomerConsumption> result = new List<CustomerConsumption> 
            {
                new CustomerConsumption { Cpf = cpf.Value, MonthReference = date.Month, YearReference = date.Year, Value = 10 },
                new CustomerConsumption { Cpf = cpf.Value, MonthReference = date.Month, YearReference = date.Year, Value = 30 }
            };

            return Task.FromResult(result);
        }
    }
}
