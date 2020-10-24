using Bogus;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Security;
using System;
using System.Collections.Generic;
using Xunit;

namespace Aluguru.Marketplace.Register.UnitTests
{
    public class UserTests
    {
        public static IEnumerable<object[]> users => Populator.Populate(100, () => CreateUser());
        [Theory]
        [MemberData(nameof(users))]
        public void CreateUser_ShouldPass(User user)
        {
            Assert.NotNull(user);
        }

        [Fact]
        public void CreateUser_WhenInvalidEmail_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User(new Randomizer().String(), Cryptography.Encrypt("really"), "Aluguru Admin", Guid.NewGuid(), Cryptography.CreateRandomHash()));
        }        

        [Fact]
        public void CreateUser_WhenInvalidPassword_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User("admin@aluguru.com", "really", "Felipe de Almeida", Guid.NewGuid(), Cryptography.CreateRandomHash()));
        }

        [Fact]
        public void CreateUser_WhenInvalidFullName_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User("admin@aluguru.com", Cryptography.Encrypt("really"), "", Guid.NewGuid(), Cryptography.CreateRandomHash()));
        }

        [Fact]
        public void CreateUser_WhenInvalidRoleIdGuid_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User("admin@aluguru.com", Cryptography.Encrypt("really"), "", Guid.Empty, Cryptography.CreateRandomHash()));
        }

        [Fact]
        public void CreateUser_WhenInvalidActivationHash_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User("admin@aluguru.com", Cryptography.Encrypt("really"), "", Guid.NewGuid(), ""));
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

        private static User CreateUser()
        {
            return new Faker<User>()
                .CustomInstantiator(x => new User(
                    x.Internet.Email(),
                    Cryptography.Encrypt(x.Internet.Password()),
                    x.Name.FullName(x.PickRandom<Bogus.DataSets.Name.Gender>()),
                    x.Random.Guid(),
                    Cryptography.CreateRandomHash()));
        }
    }
}
