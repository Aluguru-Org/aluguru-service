using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Aluguru.Marketplace.Security.User
{
    public interface IAspNetUser
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserEmail();
        bool IsAutenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetUserClaims();
        HttpContext GetHttpContext();
    }

    public class AspNetUser : IAspNetUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public Guid GetUserId()
        {
            return IsAutenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string GetUserName()
        {
            return IsAutenticated() ? _accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value : "";
        }

        public string GetUserEmail()
        {
            return IsAutenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public bool IsAutenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsInRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        public IEnumerable<Claim> GetUserClaims()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public HttpContext GetHttpContext()
        {
            return _accessor.HttpContext;
        }
    }

}
