using Mubbi.Marketplace.Register.Domain;
using System;
using Xunit;
using Bogus.Extensions.Brazil;
using Bogus;
using System.Collections.Generic;
using Bogus.DataSets;

namespace Mubbi.Marketplace.Register.UnitTests
{

    public class DocumentTests
    {
        public static IEnumerable<object[]> formattedCpfs => Populator.Populate(10, () => new Person().Cpf());
        public static IEnumerable<object[]> notFormattedCpfs => Populator.Populate(10, () => new Person().Cpf(false));
        public static IEnumerable<object[]> formattedCnpj => Populator.Populate(10, () => new Company().Cnpj());
        public static IEnumerable<object[]> notFormattedCnpj => Populator.Populate(10, () => new Company().Cnpj(false));

        [Theory]
        [MemberData(nameof(formattedCpfs))]
        public void CreateDocument_WhenCPF_ShouldPass(string cpf)
        {
            new Document(cpf, EDocumentType.CPF);            
        }

        [Theory]
        [MemberData(nameof(notFormattedCpfs))]
        public void CreateDocument_WhenCPF_WhithoutFormatting_ShouldPass(string cpf)
        {
            new Document(cpf, EDocumentType.CPF);
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
        [MemberData(nameof(formattedCnpj))]
        public void CreateDocument_WhenCNPJ_ShouldPass(string cnpj)
        {
            new Document(cnpj, EDocumentType.CNPJ);
        }

        [Theory]
        [MemberData(nameof(notFormattedCnpj))]
        public void CreateDocument_WhenCNPJ_WhithoutFormatting_ShouldPass(string cnpj)
        {
            new Document(cnpj, EDocumentType.CNPJ);
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
