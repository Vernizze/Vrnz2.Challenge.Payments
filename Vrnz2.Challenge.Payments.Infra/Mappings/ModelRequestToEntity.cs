using AutoMapper;
using System;
using Vrnz2.Challenge.Payments.Shared.Entities;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;
using Vrnz2.Infra.CrossCutting.Types;

namespace Vrnz2.Challenge.Payments.Infra.Mappings
{
    public class ModelRequestToEntity
        : Profile
    {
        public ModelRequestToEntity()
        {
            CreateMap<CreatePaymentModel.Request, Payment>()
                .ForMember(d => d.ReceitpDatetTime, orig => orig.MapFrom(src => DateTime.UtcNow))
                .ForMember(d => d.Tid, orig => orig.MapFrom(src => Guid.NewGuid()))
                .ForMember(d => d.DueDate, orig => orig.MapFrom(src => DateTime.Parse(src.DueDate)))
                .ForMember(d => d.Value, orig => orig.MapFrom(src => new Money(src.Value).Value));
        }
    }
}
