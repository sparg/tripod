﻿using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tripod.Domain.Security;

namespace Tripod.Web.Controllers
{
    public partial class SignOnController : Controller
    {
        private readonly IProcessQueries _queries;
        private readonly IProcessCommands _commands;

        [UsedImplicitly]
        public SignOnController(IProcessQueries queries, IProcessCommands commands)
        {
            _queries = queries;
            _commands = commands;
        }

        [ValidateAntiForgeryToken]
        [HttpPost, Route("sign-on")]
        public virtual ActionResult Index(string provider, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(provider)) return View(MVC.Errors.Views.BadRequest);

            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action(MVC.SignOn.Index(returnUrl)));
        }

        [HttpGet, Route("sign-on")]
        public virtual async Task<ActionResult> Index(string returnUrl)
        {
            var loginInfo = await _queries.Execute(new PrincipalRemoteMembershipTicket(User));
            if (loginInfo == null)
                return RedirectToAction(MVC.SignIn.Index());

            // sign in the user with this external login provider if the user already has this login
            // todo: also look up the user by email claim & link login if it is found
            var user = await _queries.Execute(new UserBy(loginInfo.Login));
            if (user != null)
            {
                await _commands.Execute(new SignOn
                {
                    LoginProvider = loginInfo.Login.LoginProvider,
                    ProviderKey = loginInfo.Login.ProviderKey,
                });
                return this.RedirectToLocal(returnUrl);
            }

            // if user doesn't have an email claim, we need them to confirm an email address
            var emailClaim = await _queries.Execute(new ExternalCookieClaim(ClaimTypes.Email));
            if (emailClaim == null)
            {
                return RedirectToAction(await MVC.SignOnEmail.Index(returnUrl));
            }

            // if user does have an email claim, create a confirmation against it
            var createEmailConfirmation = new CreateEmailConfirmation
            {
                Purpose = EmailConfirmationPurpose.CreateRemoteUser,
                EmailAddress = emailClaim.Value,
            };
            await _commands.Execute(createEmailConfirmation);
            return RedirectToAction(await MVC.SignOnUser.Index(createEmailConfirmation.CreatedEntity.Token, returnUrl));

            // if the user does not have an account, then prompt the user to create an account
            //ViewBag.ReturnUrl = returnUrl;
            //ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            //var model = new CreateRemoteMembership
            //{
            //    UserName = loginInfo.UserName
            //};
            //return View(MVC.Account.Views.ExternalLoginConfirmation, model);
        }
	}
}