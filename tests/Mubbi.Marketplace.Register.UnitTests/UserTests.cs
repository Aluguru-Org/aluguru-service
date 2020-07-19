using Bogus;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Security;
using System;
using System.Collections.Generic;
using Xunit;

namespace Mubbi.Marketplace.Register.UnitTests
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
            Assert.Throws<DomainException>(() => new User(new Randomizer().String(), Cryptography.Encrypt("really"), "Mubbi Admin", Guid.NewGuid()));
        }

        [Fact]
        public void CreateUser_WhenInvalidPassword_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User("admin@mubbi.com", "really", "Felipe de Almeida", Guid.NewGuid()));
        }

        [Fact]
        public void CreateUser_WhenInvalidFullName_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User("admin@mubbi.com", Cryptography.Encrypt("really"), "", Guid.NewGuid()));
        }

        [Fact]
        public void CreateUser_WhenInvalidRoleIdGuid_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new User("admin@mubbi.com", Cryptography.Encrypt("really"), "", Guid.Empty));
        }

        private static User CreateUser()
        {
            return new Faker<User>()
                .CustomInstantiator(x => new User(
                    x.Internet.Email(),
                    Cryptography.Encrypt(x.Internet.Password()),
                    x.Name.FullName(x.PickRandom<Bogus.DataSets.Name.Gender>()),
                    x.Random.Guid()));
        }
    }
}
