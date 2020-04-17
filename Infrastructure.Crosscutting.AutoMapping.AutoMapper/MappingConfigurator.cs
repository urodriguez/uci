using AutoMapper;
using Infrastructure.Crosscutting.AutoMapping.AutoMapper.Profiles;

namespace Infrastructure.Crosscutting.AutoMapping.AutoMapper
{
    public class MapperFactory
    {
        public static MapperConfiguration GetConfiguredMapper()
        {
            return new MapperConfiguration(cfg => {
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<UserProfile>();
            });
        }
    }
}
