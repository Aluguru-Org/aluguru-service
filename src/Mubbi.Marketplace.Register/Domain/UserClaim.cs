using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Mubbi.Marketplace.Register.Domain
{
    public class UserClaim : Entity
    {
        private UserClaim() : base(NewId()) { }
        public UserClaim(string type, string value) : base(NewId())
        {
            Type = type;
            Value = value;
        }

        public Guid UserRoleId { get; private set; }
        public string Type { get; private set; }
        public string Value { get; private set; }

        // EF Relational
        public UserRole UserRole { get; set; }
        internal void AssociateUserRole(Guid userRoleId)
        {
            UserRoleId = userRoleId;
        }

        protected override void ValidateEntity()
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Type), "The claim type cannot be null or empty");
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Value), "The claim value cannot be null or empty");
        }
    }
}
