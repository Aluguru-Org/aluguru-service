using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.Domain;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Register.Usecases.GetUserRoles
{
    public class GetUserRolesCommand : Command<GetUserRolesCommandResponse>
    {
        
    }

    public class GetUserRolesCommandResponse
    {
        public List<UserRole> UserRoles { get; set; }
    }
}