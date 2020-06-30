using AutoMapper;
using Mubbi.Marketplace.Register.Application.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mubbi.Marketplace.Register.Application.UnitTests
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
