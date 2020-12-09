using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Vrnz2.Challenge.Payments.Shared.Entities;
using Vrnz2.Challenge.ServiceContracts.UseCases.Models;

namespace Vrnz2.Challenge.Payments.Infra.Mappings
{
    public class EntityToModelResponse
        : Profile
    {
        public EntityToModelResponse()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Payment, GetPaymentModel.ResponsePayments>();
            });

            var mapper = config.CreateMapper();

            CreateMap<Payment, GetPaymentModel.ResponsePayments>();
            CreateMap<List<Payment>, List<GetPaymentModel.ResponsePayments>>()
                .ConvertUsing(ss => ss.Select(bs => mapper.Map<Payment, GetPaymentModel.ResponsePayments>(bs)).ToList());
            CreateMap<List<Payment>, GetPaymentModel.Response>()
                .ForMember(d => d.Payments, orig => orig.MapFrom(src => src));
        }
    }
}