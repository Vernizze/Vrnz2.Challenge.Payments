using MassTransit;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Vrnz2.Challenge.Payments.Shared.Settings;

namespace Vrnz2.Challenge.Payments.Shared.Queues
{
    public class QueueHandler
    {
        #region Variables

        private readonly AwsSettings _awsSettings;


        #endregion

        #region Constructors 

        public QueueHandler(IOptions<AwsSettings> awsOptionsSettings)
            => _awsSettings = awsOptionsSettings.Value;

        #endregion

        #region Methods

        public async Task Send<T>(T message, string queue)
        {
            var bus = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.Host(_awsSettings.Region, h =>
                {
                    h.AccessKey(_awsSettings.AccessKey);
                    h.SecretKey(_awsSettings.SecretKey);
                });
            });
            bus.Start();

            Task<ISendEndpoint> sendEndpointTask = bus.GetSendEndpoint(new Uri(queue));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            await sendEndpoint.Send(message);
        }

        #endregion
    }
}
