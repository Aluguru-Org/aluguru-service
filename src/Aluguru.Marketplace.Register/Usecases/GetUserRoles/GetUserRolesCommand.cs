using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.Domain;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Register.Usecases.GetUserRoles
{
    public class GetUserRolesCommand : Command<GetUserRolesCommandResponse>
    {
        
    }

    public class GetUserRolesCommandResponse
    {
        public List<UserRole> UserRoles { get; set; }
    }
}