// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Tripod.Web.Controllers
{
    public partial class EmailAddressesController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected EmailAddressesController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SignUpValidate()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SignUpValidate);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Confirm()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Confirm);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public EmailAddressesController Actions { get { return MVC.EmailAddresses; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "EmailAddresses";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "EmailAddresses";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string SignUp = "SignUp";
            public readonly string SignUpValidate = "SignUpValidate";
            public readonly string Confirm = "Confirm";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string SignUp = "SignUp";
            public const string SignUpValidate = "SignUpValidate";
            public const string Confirm = "Confirm";
        }


        static readonly ActionParamsClass_SignUp s_params_SignUp = new ActionParamsClass_SignUp();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SignUp SignUpParams { get { return s_params_SignUp; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SignUp
        {
            public readonly string command = "command";
        }
        static readonly ActionParamsClass_SignUpValidate s_params_SignUpValidate = new ActionParamsClass_SignUpValidate();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SignUpValidate SignUpValidateParams { get { return s_params_SignUpValidate; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SignUpValidate
        {
            public readonly string command = "command";
            public readonly string fieldName = "fieldName";
        }
        static readonly ActionParamsClass_Confirm s_params_Confirm = new ActionParamsClass_Confirm();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Confirm ConfirmParams { get { return s_params_Confirm; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Confirm
        {
            public readonly string ticket = "ticket";
            public readonly string token = "token";
            public readonly string command = "command";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_EmailAddressesController : Tripod.Web.Controllers.EmailAddressesController
    {
        public T4MVC_EmailAddressesController() : base(Dummy.Instance) { }

        partial void SignUpOverride(T4MVC_System_Web_Mvc_ViewResult callInfo);

        public override System.Web.Mvc.ViewResult SignUp()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ViewResult(Area, Name, ActionNames.SignUp);
            SignUpOverride(callInfo);
            return callInfo;
        }

        partial void SignUpOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Tripod.Domain.Security.SendConfirmationEmail command);

        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> SignUp(Tripod.Domain.Security.SendConfirmationEmail command)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SignUp);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "command", command);
            SignUpOverride(callInfo, command);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }

        partial void SignUpValidateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Tripod.Domain.Security.SendConfirmationEmail command, string fieldName);

        public override System.Web.Mvc.ActionResult SignUpValidate(Tripod.Domain.Security.SendConfirmationEmail command, string fieldName)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SignUpValidate);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "command", command);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "fieldName", fieldName);
            SignUpValidateOverride(callInfo, command, fieldName);
            return callInfo;
        }

        partial void ConfirmOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ticket, string token);

        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Confirm(string ticket, string token)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Confirm);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ticket", ticket);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "token", token);
            ConfirmOverride(callInfo, ticket, token);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }

        partial void ConfirmOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ticket, Tripod.Domain.Security.VerifyConfirmEmailSecret command);

        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Confirm(string ticket, Tripod.Domain.Security.VerifyConfirmEmailSecret command)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Confirm);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ticket", ticket);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "command", command);
            ConfirmOverride(callInfo, ticket, command);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
