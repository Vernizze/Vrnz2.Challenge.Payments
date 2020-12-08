using AutoMapper;
using Vrnz2.Challenge.ServiceContracts.Notifications;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;
using Vrnz2.Infra.CrossCutting.Types;

namespace Vrnz2.Challenge.Payments.Infra.Mappings
{
    public class ModelRequestToNotification
        : Profile
    {
        public ModelRequestToNotification()
        {
            CreateMap<CreatePaymentModel.Request, PaymentNotification.Created>()
                .ForMember(d => d.Value, orig => orig.MapFrom(src => new Money(src.Value).Value));
        }
    }
}
