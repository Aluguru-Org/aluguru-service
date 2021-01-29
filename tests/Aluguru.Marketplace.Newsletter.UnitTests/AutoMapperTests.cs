using Aluguru.Marketplace.Newsletter.AutoMapper;
using AutoMapper;
using Xunit;

namespace Aluguru.Marketplace.Newsletter.UnitTests
{
    public class AutoMapperTests
    {
        private readonly MapperConfiguration _mapperConfiguration;

        public AutoMapperTests()
        {
            _mapperConfiguration = new MapperConfiguration(x =>
            {
                x.AddProfile(new NewsletterContextMappingConfiguration());
            });
        }

        [Fact]
        public void Map_Configuration()
        {
            _mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
