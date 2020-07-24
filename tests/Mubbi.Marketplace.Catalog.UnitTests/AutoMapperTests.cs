using AutoMapper;
using Mubbi.Marketplace.Catalog.AutoMapper;
using Xunit;

namespace Mubbi.Marketplace.Catalog.UnitTests
{
    public class AutoMapperTest
    {
        private readonly MapperConfiguration _mapperConfiguration;

        public AutoMapperTest()
        {
            _mapperConfiguration = new MapperConfiguration(x =>
            {
                x.AddProfile(new CatalogContextMappingConfiguration());
            });
        }

        [Fact]
        public void Map_Configuration()
        {
            _mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
