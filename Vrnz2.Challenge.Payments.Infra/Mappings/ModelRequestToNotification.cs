using AutoMapper;
using System;
using Vrnz2.Challenge.ServiceContracts.Notifications;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;
using Vrnz2.Infra.CrossCutting.Extensions;
using Vrnz2.Infra.CrossCutting.Types;

namespace Vrnz2.Challenge.Payments.Infra.Mappings
{
    public class ModelRequestToNotification
        : Profile
    {
        public ModelRequestToNotification()
        {
            CreateMap<CreatePaymentModel.Request, PaymentNotification.Created>()
                .ForMember(d => d.CreationDate, orig => orig.MapFrom(src => DateTime.UtcNow))
                .ForMember(d => d.Value, orig => orig.MapFrom(src => GetValue(src.Cpf).Value));
        }

        private Money GetValue(string cpf) 
            => new Money(string.Concat(cpf.TakeString(2), cpf.TakeString(cpf.Length - 2, 2), ".00"));
    }
}
