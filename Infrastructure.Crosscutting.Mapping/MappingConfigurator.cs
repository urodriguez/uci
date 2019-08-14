using AutoMapper;
using Infrastructure.Crosscutting.Mapping.Profiles;

namespace Infrastructure.Crosscutting.Mapping
{
    public class MapperFactory
    {
        public static MapperConfiguration GetConfiguredMApper()
        {
            return new MapperConfiguration(cfg => {
                cfg.AddProfile<ProductProfile>();
            });
        }
    }
}
