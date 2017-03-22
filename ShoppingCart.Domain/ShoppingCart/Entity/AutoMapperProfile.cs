using AutoMapper;
using ShoppingCart.Domain.Data.Entity;

namespace ShoppingCart.Domain.ShoppingCart.Entity
{
    public class AutoMapperProfile:Profile
    {
        protected override void Configure()
        {
            CreateMap<Item, SoldItem>()
                .ForMember(dest => dest.PriceDuringCheckout, opt => opt.MapFrom(src => src.Products.Price));
        }
    }
}
