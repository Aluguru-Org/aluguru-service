using Aluguru.Marketplace.Register.Domain;
using System;
using Xunit;

namespace Aluguru.Marketplace.Register.UnitTests
{
    public class AddressTests
    {
        [Theory]
        [InlineData("General Lima e Silva", "480", "Cidade Baixa", "Porto Alegre", "Rio Grande do Sul", "Brasil", "90050100", "02")]
        [InlineData("Belgraf", "400", "aVENIDA BLA", "Eldorado do sul", "Rio Grande do Sul", "Brasil", "6546548947", "02")]
        public void CreateAddress_WhenValidData_ShouldNotThrowDomainException(string street, string number, string neighborhood, string city, string state, string country, string zipcode, string complement)
        {
            new Address(street, number, neighborhood, city, state, country, zipcode, complement);
        }

        [Theory]
        [InlineData("", "480", "Cidade Baixa", "Porto Alegre", "Rio Grande do Sul", "Brasil", "90050100", "02")]
        [InlineData("General Lima e Silva", "", "Cidade Baixa", "Porto Alegre", "Rio Grande do Sul", "Brasil", "90050100", "02")]
        [InlineData("General Lima e Silva", "480", "", "Porto Alegre", "Rio Grande do Sul", "Brasil", "90050100", "02")]
        [InlineData("General Lima e Silva", "480", "Cidade Baixa", "", "Rio Grande do Sul", "Brasil", "90050100", "02")]
        [InlineData("General Lima e Silva", "480", "Cidade Baixa", "Porto Alegre", "", "Brasil", "90050100", "02")]
        [InlineData("General Lima e Silva", "480", "Cidade Baixa", "Porto Alegre", "Rio Grande do Sul", "", "90050100", "02")]
        [InlineData("General Lima e Silva", "480", "Cidade Baixa", "Porto Alegre", "Rio Grande do Sul", "Brasil", "", "02")]
        public void CreateAddress_WhenInvalidData_ShouldThrowDomainException(string street, string number, string neighborhood, string city, string state, string country, string zipcode, string complement)
        {
            Assert.Throws<Exception>(() => new Address(street, number, neighborhood, city, state, country, zipcode, complement));
        }
    }
}
