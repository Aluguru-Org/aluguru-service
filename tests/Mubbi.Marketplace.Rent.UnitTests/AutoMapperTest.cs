using AutoMapper;
using Mubbi.Marketplace.Rent.AutoMapper;
using Xunit;

namespace Mubbi.Marketplace.Rent.UnitTests
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
