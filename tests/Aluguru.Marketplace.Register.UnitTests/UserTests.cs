using Bogus;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Security;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.Options;

namespace Aluguru.Marketplace.Register.UnitTests
{
    public class UserTests
    {
        private readonly Cryptography _cryptography = new Cryptography(Options.Create(new SecuritySettings()
        {
            SecretKey = "TD8K9CG3GBMBX6U7"
        }));

        [Fact]
        public void CreateUser_ShouldPass()
        {
            CreateUser();
        }

        [Fact]
        public void CreateUser_WhenInvalidEmail_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User(new Randomizer().String(), _cryptography.Encrypt("really"), "Aluguru Admin", Guid.NewGuid(), _cryptography.CreateRandomHash()));
        }        

        [Fact]
        public void CreateUser_WhenInvalidPassword_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User("admin@aluguru.com", "really", "Felipe de Almeida", Guid.NewGuid(), _cryptography.CreateRandomHash()));
        }

        [Fact]
        public void CreateUser_WhenInvalidFullName_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User("admin@aluguru.com", _cryptography.Encrypt("really"), "", Guid.NewGuid(), _cryptography.CreateRandomHash()));
        }

        [Fact]
        public void CreateUser_WhenInvalidRoleIdGuid_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User("admin@aluguru.com", _cryptography.Encrypt("really"), "", Guid.Empty, _cryptography.CreateRandomHash()));
        }

        [Fact]
        public void CreateUser_WhenInvalidActivationHash_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User("admin@aluguru.com", _cryptography.Encrypt("really"), "", Guid.NewGuid(), ""));
        }

        [Theory]
        [InlineData("")]
        [InlineData("wd2rtW88mWx3EOc1JFX66v634WEQE")]
        public void UpdatePassword_WhenInvalidPassword_ShouldThrowDomainException(string password)
        {
            var user = CreateUser();
            Assert.Throws<DomainException>(() => user.UpdatePassword(password));
        }

        [Fact]
        public void UpdatePassword_WhenValidPassword_ShouldNotThrowDomainException()
        {
            var user = CreateUser();
            user.UpdatePassword("24/74c0ZcIuP++wd2rtW88mWx3EOc1JFX66v634WEQE=");
        }

        [Fact]
        public void Activate_WhenUserIsInactive_ShouldActivateUser()
        {
            var user = CreateUser();

            user.Activate(user.ActivationHash);

            Assert.True(user.IsActive);
        }

        private User CreateUser()
        {
            var faker = new Faker("pt_BR");

            return new User(
                email: faker.Internet.Email(),
                password: _cryptography.Encrypt(faker.Internet.Password()),
                fullName: faker.Name.FullName(),
                role: Guid.NewGuid(),
                _cryptography.CreateRandomHash());
        }
    }
}
