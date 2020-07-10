using AutoMapper;
using Mubbi.Marketplace.Register.AutoMapper;
using Xunit;

namespace Mubbi.Marketplace.Register.UnitTests
{
    public class AutoMapperTest
    {
        private readonly MapperConfiguration _mapperConfiguration;

        public AutoMapperTest()
        {            
            _mapperConfiguration = new MapperConfiguration(x =>
            {
                x.AddProfile(new RegisterContextMappingConfiguration());
            });
        }

        [Fact]
        public void Map_Configuration()
        {
            _mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
