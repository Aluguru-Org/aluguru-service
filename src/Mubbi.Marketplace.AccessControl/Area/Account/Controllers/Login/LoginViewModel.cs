using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.AccessControl.Area.Account.Controllers.Login
{
    public class LoginViewModel
    {
        public string ReturnUrl { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderScheme { get; set; }
        public string Error { get; set; }
    }
}
