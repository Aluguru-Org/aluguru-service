using Mubbi.Marketplace.Register.Domain;
using System;
using Xunit;

namespace Mubbi.Marketplace.Register.UnitTests
{ 
    public class DocumentTests
    {
        [Theory]
        [InlineData("886.280.870-44")]
        [InlineData("923.159.870-83")]
        [InlineData("54003064038")]
        [InlineData("47279336086")]
        public void CreateDocument_WhenCPF_AndValidNumber_ShouldNotThrowDomainException(string number)
        {
            new Document(number, EDocumentType.CPF);
        }

        [Theory]
        [InlineData("886.280.870-442")]
        [InlineData("923.159.87083")]
        [InlineData("540030640385")]
        [InlineData("4727933608")]
        public void CreateDocument_WhenCPF_AndInvalidNumber_ShouldNotThrowDomainException(string number)
        {
            Assert.Throws<Exception>(() => new Document(number, EDocumentType.CPF));
        }

        [Theory]
        [InlineData("74.765.845/0001-04")]
        [InlineData("25.222.744/0001-81")]
        [InlineData("92621944000102")]
        [InlineData("41846246000166")]
        public void CreateDocument_WhenCNPJ_AndValidNumber_ShouldNotThrowDomainException(string number)
        {
            new Document(number, EDocumentType.CNPJ);
        }

        [Theory]
        [InlineData("74.765.8450001-04")]
        [InlineData("25222.744/0001-81")]
        [InlineData("926219440001022")]
        [InlineData("4184624600016")]
        public void CreateDocument_WhenCNPJ_AndInvalidNumber_ShouldNotThrowDomainException(string number)
        {
            Assert.Throws<Exception>(() => new Document(number, EDocumentType.CNPJ));

        }
    }
}
