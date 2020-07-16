using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.AccessControl.Area.Account.Controllers.Login
{
    public class LoginController : Controller
    {
        private readonly IUsersStore usersStore;
        private readonly ICollaboratorsStore collaboratorsService;
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly IEventService eventsService;

        public LoginController(IUsersStore usersStore, ICollaboratorsStore collaboratorsService,
            IIdentityServerInteractionService interactionService, IAuthenticationSchemeProvider schemeProvider, IEventService eventsService)
        {
            this.userAccountService = userAccountService;
            this.usersStore = usersStore;
            this.collaboratorsService = collaboratorsService;
            this.interactionService = interactionService;
            this.schemeProvider = schemeProvider;
            this.eventsService = eventsService;
        }

        public async Task<IActionResult> IndexAsync(string returnUrl)
        {
            var vm = await BuildLoginViewModelAsync(returnUrl);

            return View("Index", vm);
        }

        public IActionResult ChallengeAsync(string provider, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            if (Url.IsLocalUrl(returnUrl) == false && interactionService.IsValidReturnUrl(returnUrl) == false)
            {
                throw new ArgumentException("Invalid return URL");
            }

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", provider },
                    }
            };

            return Challenge(props, provider);
        }

        public async Task<IActionResult> Callback()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            var localUserInfo = await GetOrCreateUserAsync(result);

            await HttpContext.SignInAsync(localUserInfo.UserId.ToString(), localUserInfo.Name, new Claim[] { });
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";
            var context = await interactionService.GetAuthorizationContextAsync(returnUrl);

            await eventsService.RaiseAsync(new UserLoginSuccessEvent(localUserInfo.Provider, localUserInfo.SubjectId, localUserInfo.SubjectId, localUserInfo.Name, true, context?.ClientId));

            return Redirect(returnUrl);
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, string defaultAuthScheme = "microsoft")
        {
            var schemes = await schemeProvider.GetAllSchemesAsync();

            var microsoftAuthScheme = schemes.FirstOrDefault(s => s.Name == defaultAuthScheme);

            return new LoginViewModel
            {
                ProviderDisplayName = microsoftAuthScheme?.DisplayName,
                ProviderScheme = microsoftAuthScheme?.Name,
                ReturnUrl = returnUrl
            };
        }

        private async Task<UserInfo> GetOrCreateUserAsync(AuthenticateResult result)
        {
            var subjectId = result.Principal.FindFirstValue(JwtClaimTypes.Subject) ?? result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (subjectId == null)
            {
                throw new InvalidOperationException("A sub claim was expected and wasn't returned from external provider.");
            }

            var existingUser = await usersStore.GetUserByExternalProviderIdAsync(subjectId);

            var localUserInfo = new UserInfo
            {
                UserId = existingUser?.Id,
                SubjectId = subjectId,
                Provider = result.Properties.Items["scheme"],
                Name = existingUser?.Name ?? result.Principal.FindFirstValue(ClaimTypes.Name),
                Email = existingUser?.Email ?? result.Principal.FindFirstValue(ClaimTypes.Email)
            };

            if (existingUser == null)
            {
                localUserInfo.UserId = await usersStore.CreateUserAsync(localUserInfo.Email, localUserInfo.Name, localUserInfo.Provider, localUserInfo.SubjectId);
            }

            return localUserInfo;
        }
    }

}
