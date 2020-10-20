using Aluguru.Marketplace.Register.Domain;
using System;
using Xunit;

namespace Aluguru.Marketplace.Register.UnitTests
{
    public class AddressTests
    {
        [Theory]
        [InlineData("General Lima e Silva", "480", "Cidade Baixa", "Porto Alegre", "Rio Grande do Sul", "Brasil", "90050100")]
        [InlineData("Belgraf", "400", "aVENIDA BLA", "Eldorado do sul", "Rio Grande do Sul", "Brasil", "6546548947")]
        public void CreateAddress_WhenValidData_ShouldNotThrowDomainException(string street, string number, string neighborhood, string city, string state, string country, string zipcode)
        {
            new Address(Guid.NewGuid(), street, number, neighborhood, city, state, country, zipcode);
        }

        [Theory]
        [InlineData("", "480", "Cidade Baixa", "Porto Alegre", "Rio Grande do Sul", "Brasil", "90050100")]
        [InlineData("General Lima e Silva", "", "Cidade Baixa", "Porto Alegre", "Rio Grande do Sul", "Brasil", "90050100")]
        [InlineData("General Lima e Silva", "480", "", "Porto Alegre", "Rio Grande do Sul", "Brasil", "90050100")]
        [InlineData("General Lima e Silva", "480", "Cidade Baixa", "", "Rio Grande do Sul", "Brasil", "90050100")]
        [InlineData("General Lima e Silva", "480", "Cidade Baixa", "Porto Alegre", "", "Brasil", "90050100")]
        [InlineData("General Lima e Silva", "480", "Cidade Baixa", "Porto Alegre", "Rio Grande do Sul", "", "90050100")]
        [InlineData("General Lima e Silva", "480", "Cidade Baixa", "Porto Alegre", "Rio Grande do Sul", "Brasil", "")]
        public void CreateAddress_WhenInvalidData_ShouldThrowDomainException(string street, string number, string neighborhood, string city, string state, string country, string zipcode)
        {
            Assert.Throws<Exception>(() => new Address(Guid.NewGuid(), street, number, neighborhood, city, state, country, zipcode));

        }
    }
}
