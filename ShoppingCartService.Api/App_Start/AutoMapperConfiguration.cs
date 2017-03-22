using AutoMapper;
using ShoppingCart.Domain.ShoppingCart.Entity;

namespace ShoppingCart.Api.App_Start
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<AutoMapperProfile>();
            });
        }
    }
}