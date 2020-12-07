using AutoMapper;

namespace Vrnz2.Challenge.Payments.Infra.Mappings
{
    public class ModelRequestToEntity
        : Profile
    {
        public ModelRequestToEntity()
        {
            //CreateMap<CreateCustomerModel.Request, Customer>()
            //    .ForMember(d => d.UniqueId, orig => orig.MapFrom(src => Guid.NewGuid()));
        }
    }
}
