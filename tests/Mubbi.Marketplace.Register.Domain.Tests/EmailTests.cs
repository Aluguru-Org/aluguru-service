using System;
using Xunit;

namespace Mubbi.Marketplace.Register.Domain.Tests
{
    public class EmailTests
    {
        [Theory]
        [InlineData("almeidafelipewanted@gmail.com")]
        [InlineData("felipe.allmeida.dev@outlook.com.br")]
        [InlineData("thais420@terra.uol")]
        [InlineData("aaa@metrocast.com.net")]
        [InlineData("Felipe_Almeida@dell.com")]
        public void CreateEmail_WhenValidEmail_ShouldNotThrowDomainException(string emailAddress)
        {
            new Email(emailAddress);
        }

        [Theory]
        [InlineData("almeidafelipewantedgmail.com")]
        [InlineData("felipe.!all^^meida.dev@outlook.com.br")]
        [InlineData("th^#@#ais420@terra.uol")]
        [InlineData("aaa@metr/cast.com.net")]
        [InlineData("Felipe_Almeida@dell@com")]
        public void CreateEmail_WhenInvalidEmail_ShouldThrowDomainException(string emailAddress)
        {
            Assert.Throws<Exception>(() => new Email(emailAddress));
        }
    }
}
