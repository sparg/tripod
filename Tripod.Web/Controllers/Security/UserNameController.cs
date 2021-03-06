﻿using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Tripod.Domain.Security;
using Tripod.Web.Models;

namespace Tripod.Web.Controllers
{
    [Authorize]
    public partial class UserNameController : Controller
    {
        private readonly IProcessQueries _queries;
        private readonly IProcessCommands _commands;

        [UsedImplicitly]
        public UserNameController(IProcessQueries queries, IProcessCommands commands)
        {
            _queries = queries;
            _commands = commands;
        }

        [HttpGet, Route("settings")]
        public virtual async Task<ActionResult> Index()
        {
            var view = await _queries.Execute(new UserViewBy(User.Identity.GetUserId<int>()));
            if (view == null) return HttpNotFound();

            var model = new ChangeUserNameModel
            {
                UserView = view,
                Command = new ChangeUserName
                {
                    UserId = view.UserId,
                    UserName = view.UserName,
                },
            };

            return View(MVC.Security.Views.User.Settings, model);
        }

        [ValidateAntiForgeryToken]
        [HttpPut, Route("settings/username")]
        public virtual async Task<ActionResult> Put(ChangeUserName command)
        {
            if (command == null) return View(MVC.Errors.Views.BadRequest);

            var view = await _queries.Execute(new UserViewBy(command.UserId));
            if (view == null) return HttpNotFound();

            if (!ModelState.IsValid)
            {
                var model = new ChangeUserNameModel
                {
                    UserView = view,
                    Command = command,
                };

                return View(MVC.Security.Views.User.Settings, model);
            }

            await _commands.Execute(command);
            Response.ClientCookie(command.SignedIn.Id, _queries);
            return RedirectToAction(await MVC.UserName.Index());
        }

        [ValidateAntiForgeryToken]
        [HttpPost, Route("settings/username/validate")]
        public virtual ActionResult ValidatePut(ChangeUserName command)
        {
            //System.Threading.Thread.Sleep(new Random().Next(5000, 5001));
            if (command == null)
            {
                Response.StatusCode = 400;
                return Json(null);
            }

            var result = new ValidatedFields(ModelState);

            //ModelState[command.PropertyName(x => x.UserName)].Errors.Clear();
            //result = new ValidatedFields(ModelState, command.PropertyName(x => x.UserName));

            return new CamelCaseJsonResult(result);
        }
    }
}