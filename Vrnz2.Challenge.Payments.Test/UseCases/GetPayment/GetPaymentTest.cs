using AutoMapper;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vrnz2.Challenge.Payments.Shared.Entities;
using Vrnz2.Challenge.Payments.Shared.Settings;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;
using Vrnz2.Infra.CrossCutting.Types;
using Xunit;
using Get = Vrnz2.Challenge.Payments.UseCases.GetPayment;

namespace Vrnz2.Challenge.Payments.Test.UseCases.GetPayment
{
    public class GetPaymentTest
    {
        private IMapper _mapper;
        private IOptions<ConnectionStringsSettings> _connectionStringsOptions;

        public GetPaymentTest()
        {
            _connectionStringsOptions = Options.Create(new ConnectionStringsSettings
            {
                MongoDbChallenge = string.Empty
            });

            _mapper = Substitute.For<IMapper>();
        }

        private GetPaymentMock GetInstance()
            => new GetPaymentMock(_connectionStringsOptions, _mapper);

        [Fact]
        public async Task GetPayment_Hamdler_Test()
        {
            //Arrange           
            var tid = new Guid("ef01bedb-2d4c-418e-ac52-1e8a10b9b2a8");
            Cpf cpf = "434.443.474-99";
            var paymentDate = new DateTime(2020, 12, 01);
            var service = GetInstance();

            var request = new GetPaymentModel.Request
            {
                Cpf = cpf.Value,
                DueDate = paymentDate.ToString("yyyy-MM-ddTHH:mm:ss")                
            };

            var response = new GetPaymentModel.Response 
            {
                Payments = new List<GetPaymentModel.ResponsePayments> 
                {
                    new GetPaymentModel.ResponsePayments { Tid = tid, Cpf = cpf.Value, Value = 10 },
                    new GetPaymentModel.ResponsePayments { Tid = tid, Cpf = cpf.Value, Value = 30 }
                }
            };

            _mapper.Map<GetPaymentModel.Response>(Arg.Any<List<Payment>>()).Returns(response);

            //Act
            var result = await service.Handle(request, new System.Threading.CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Payments.Count());
            Assert.Equal(40M, result.Payments.Sum(s => s.Value));
        }
    }

    public class GetPaymentMock
        : Get.GetPayment
    {
        public GetPaymentMock(IOptions<ConnectionStringsSettings> connectionStringsOptions, IMapper mapper)
            : base(connectionStringsOptions, mapper)
        {
        }

        public override Task<List<Payment>> GetPayments(GetPaymentModel.Request request)
        {
            var tid = new Guid("ef01bedb-2d4c-418e-ac52-1e8a10b9b2a8");
            Cpf cpf = "434.443.474-99";
            var date = new DateTime(2020, 12, 01);

            var result = new List<Payment>
            {
                new Payment { Tid = tid, Cpf = cpf.Value, DueDate = date, Value = 10 },
                new Payment { Tid = tid, Cpf = cpf.Value, DueDate = date, Value = 30 }
            };

            return Task.FromResult(result);
        }
    }
}