using AutoMapper;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            DefaultContractResolver contractResolver= new DefaultContractResolver() 
            { 
                NamingStrategy = new CamelCaseNamingStrategy()
            };
        }
    }
}
