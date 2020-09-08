using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Register.Usecases.UpdateUserRole;
using PampaDevs.Utils;
using System.Collections.Generic;
using static PampaDevs.Utils.Helpers.IdHelper;
using static PampaDevs.Utils.Helpers.DateTimeHelper;
using Mubbi.Marketplace.Register.Events;
using System.Linq;

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

        public UserRole UpdateUserRole(UpdateUserRoleCommand command)
        {
            Name = command.UserRole.Name;

            ValidateEntity();

            DateUpdated = NewDateTime();

            AddEvent(new UserRoleUpdatedEvent(Id, this));

            return this;
        }

        public void AddClaims(IEnumerable<UserClaim> userClaims)
        {
            foreach(var userClaim in userClaims)
            {
                AddClaim(userClaim);
            }
        }

        public void AddClaim(UserClaim userClaim)
        {
            if (_userClaims.Any(x => x.Type == userClaim.Type && x.Value == userClaim.Value)) return;

            userClaim.AssociateUserRole(Id);

            _userClaims.Add(userClaim);
        }

        protected override void ValidateEntity()
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Name), "The role name cannot be null");
            Ensure.That<DomainException>(Name.Length >= 3 && Name.Length <= 10, "The role name must have between 3 and 10 characters");
        }
    }
}