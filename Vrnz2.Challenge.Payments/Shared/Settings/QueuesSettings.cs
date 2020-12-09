using Vrnz2.Challenge.ServiceContracts.Settings;

namespace Vrnz2.Challenge.Payments.Shared.Settings
{
    public class QueuesSettings
        : BaseAppSettings
    {
        public string PaymentCreatedQueueName { get; set; }
    }
}
