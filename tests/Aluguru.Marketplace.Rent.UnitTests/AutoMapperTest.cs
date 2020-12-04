using AutoMapper;
using Aluguru.Marketplace.Rent.AutoMapper;
using Xunit;

namespace Aluguru.Marketplace.Rent.Tests
{
    public class AutoMapperTest
    {
        private readonly MapperConfiguration _mapperConfiguration;

        public AutoMapperTest()
        {
            _mapperConfiguration = new MapperConfiguration(x =>
            {
                x.AddProfile(new RentContextMappingConfiguration());
            });
        }

        [Fact]
        public void Map_Configuration()
        {
            _mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
