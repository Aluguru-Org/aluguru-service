using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System.Collections.Generic;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Mubbi.Marketplace.Register.Domain
{
    public class UserRole : AggregateRoot
    {
        private readonly List<User> _users;
        private readonly List<UserClaim> _userClaims;
        private UserRole() : base(NewId()) 
        {
            _users = new List<User>();
            _userClaims = new List<UserClaim>();
        }

        public UserRole(string roleName)
            : base(NewId())
        {
            Name = roleName;
            _users = new List<User>();
            _userClaims = new List<UserClaim>();
        }

        public string Name { get; private set; }
        public IReadOnlyCollection<User> Users { get; set; }
        public IReadOnlyCollection<UserClaim> UserClaims { get { return _userClaims; } }        

        protected override void ValidateCreation()
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Name), "The role name cannot be null");
            Ensure.That<DomainException>(Name.Length >= 3 && Name.Length <= 10, "The role name must have between 3 and 10 characters");
        }
    }
}