using Mubbi.Marketplace.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Application.Usecases.LogInUser
{
    public class LogInUserCommandResponse : CommandResponse
    {
        public string Token { get; set; }
    }
}
