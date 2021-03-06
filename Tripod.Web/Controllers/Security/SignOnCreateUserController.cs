﻿using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tripod.Domain.Security;
using Tripod.Web.Models;

namespace Tripod.Web.Controllers
{
    public partial class SignOnCreateUserController : Controller
    {
        private readonly IProcessQueries _queries;
        private readonly IProcessCommands _commands;

        [UsedImplicitly]
        public SignOnCreateUserController(IProcessQueries queries, IProcessCommands commands)
        {
            _queries = queries;
            _commands = commands;
        }

        [HttpGet, Route("sign-on/register")]
        public virtual async Task<ActionResult> Index(string token, string ticket, string returnUrl)
        {
            // make sure we still have a remote login
            var loginInfo = await _queries.Execute(new PrincipalRemoteMembershipTicket(User));
            if (loginInfo == null)
                return RedirectToAction(MVC.SignIn.Index());

            var verification = await _queries.Execute(new EmailVerificationBy(ticket));
            if (verification == null) return HttpNotFound();
            var emailClaim = await _queries.Execute(new ExternalCookieClaim(ClaimTypes.Email));

            // todo: verification cannot be expired, redeemed, or for different purpose

            // if suggested username is already in use, use email address
            var user = await _queries.Execute(new UserBy(loginInfo.UserName));

            ViewBag.EmailAddress = verification.EmailAddress.Value;
            ViewBag.UserName = user == null ? loginInfo.UserName : ViewBag.EmailAddress;
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            ViewBag.HasClaimsEmail = emailClaim != null;
            ViewBag.Token = token;
            ViewBag.Ticket = ticket;
            ViewBag.ReturnUrl = returnUrl;
            return View(MVC.Security.Views.SignOn.CreateUser);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, Route("sign-on/register/validate/{fieldName?}")]
        public virtual ActionResult Validate(CreateRemoteMembership command, string fieldName = null)
        {
            //System.Threading.Thread.Sleep(new Random().Next(5000, 5001));

            if (command == null)
            {
                Response.StatusCode = 400;
                return Json(null);
            }

            var result = new ValidatedFields(ModelState, fieldName);

            //ModelState[command.PropertyName(x => x.UserName)].Errors.Clear();
            //result = new ValidatedFields(ModelState, fieldName);

            return new CamelCaseJsonResult(result);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, Route("sign-on/register")]
        public virtual async Task<ActionResult> Post(CreateRemoteMembership command, string returnUrl, string emailAddress)
        {
            //System.Threading.Thread.Sleep(new Random().Next(5000, 5001));

            // make sure we still have a remote login
            var loginInfo = await _queries.Execute(new PrincipalRemoteMembershipTicket(User));
            if (loginInfo == null)
                return RedirectToAction(MVC.SignIn.Index());

            if (command == null || string.IsNullOrWhiteSpace(emailAddress))
                return View(MVC.Errors.Views.BadRequest);

            if (!ModelState.IsValid)
            {
                var emailClaim = await _queries.Execute(new ExternalCookieClaim(ClaimTypes.Email));
                ViewBag.EmailAddress = emailAddress;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                ViewBag.HasClaimsEmail = emailClaim != null;
                ViewBag.Token = command.Token;
                ViewBag.Ticket = command.Ticket;
                ViewBag.ReturnUrl = returnUrl;
                return View(MVC.Security.Views.SignOn.CreateUser, command);
            }

            await _commands.Execute(command);

            var signOn = new SignOn
            {
                Principal = User,
            };
            await _commands.Execute(signOn);
            Session.VerifyEmailTickets(null);
            Response.ClientCookie(signOn.SignedOn.Id, _queries);
            return this.RedirectToLocal(returnUrl, await MVC.UserName.Index());
        }
    }
}