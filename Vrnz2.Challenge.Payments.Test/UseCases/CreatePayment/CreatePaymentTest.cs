using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vrnz2.Challenge.Payments.Shared.Entities;
using Vrnz2.Challenge.Payments.Shared.Queues;
using Vrnz2.Challenge.Payments.Shared.Settings;
using Vrnz2.Challenge.ServiceContracts.Notifications;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;
using Vrnz2.Infra.CrossCutting.Types;
using Xunit;
using Create = Vrnz2.Challenge.Payments.UseCases.CreatePayment;
using Get = Vrnz2.Challenge.Payments.UseCases.GetPayment;

namespace Vrnz2.Challenge.Payments.Test.UseCases.CreatePayment
{
    public class CreatePaymentTest
    {
        private IMapper _mapper;
        private QueueHandler _queueHandler;
        private IOptions<ConnectionStringsSettings> _connectionStringsOptions;
        private IOptions<QueuesSettings> _queuesOptionsSettings;
        private IOptions<AwsSettings> _awsOptionsSettings;

        public CreatePaymentTest() 
        {
            _connectionStringsOptions = Options.Create(new ConnectionStringsSettings
            {
                MongoDbChallenge = string.Empty
            });

            _queuesOptionsSettings = Options.Create(new QueuesSettings
            {
                PaymentCreatedQueueName = "fila-teste"
            });

            _awsOptionsSettings = Options.Create(new AwsSettings
            {
                AccessKey = "XXX",
                Region = "XXX",
                SecretKey = "XXX"
            });

            _mapper = Substitute.For<IMapper>();
            _queueHandler = new QueueHandler(_awsOptionsSettings);
        }

        private CreatePaymentMock GetInstance()
        {
            var getPayment = new Get.GetPayment(_connectionStringsOptions, _mapper);

            return new CreatePaymentMock(_connectionStringsOptions, _queuesOptionsSettings, getPayment, _mapper, _queueHandler);
        }

        [Fact]
        public async Task GetCreatePaymentTest()
        {
            //Arrange           
            Cpf cpf = "434.443.474-99";
            var dueDate = DateTime.UtcNow;
            var tid = Guid.NewGuid();
            var value = 20M;

            var payment = new Payment 
            {
                Cpf = cpf.Value,
                DueDate = dueDate,
                Tid = tid,
                Value = value
            };

            _mapper.Map<Payment>(Arg.Any<CreatePaymentModel.Request>()).Returns(payment);

            var createPayment = GetInstance();

            var request = new CreatePaymentModel.Request
            {
                Cpf = cpf.Value,
                DueDate = dueDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                Value = value.ToString()
            };

            //Act
            var result = await createPayment.Handle(request, new System.Threading.CancellationToken());
            
            //Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(payment.Tid, result.Tid);
        }
    }

    public class CreatePaymentMock
        : Create.CreatePayment
    {
        public CreatePaymentMock(
            IOptions<ConnectionStringsSettings> connectionStringsOptions, 
            IOptions<QueuesSettings> queuesOptionsSettings, 
            Get.GetPayment getPayment, IMapper mapper, QueueHandler queueHandler) 
            : base(connectionStringsOptions, queuesOptionsSettings, getPayment, mapper, queueHandler)
        {

        }

        public override Task SendToMongo(Payment payment)
            =>  Task.CompletedTask;

        public override Task SendToQueue(PaymentNotification.Created notification)
            => Task.CompletedTask;
    }
}
