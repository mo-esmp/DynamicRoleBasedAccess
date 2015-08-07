using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DynamicRoleBasedAccess.Models;
using Microsoft.AspNet.Identity.Owin;

namespace DynamicRoleBasedAccess.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private string _requestControllerName;
        private string _requestedActionName;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            _requestControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            _requestedActionName = filterContext.ActionDescriptor.ActionName;

            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            var user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
                return false;

            var dbContext = httpContext.GetOwinContext().Get<ApplicationDbContext>();
            var roleAccess = from ra in dbContext.RoleAccesses
                             let userId = dbContext.Users.FirstOrDefault(u => u.UserName == user.Identity.Name).Id
                             let roleIds = dbContext.Roles.Where(r => r.Users.Any(u => u.UserId == userId)).Select(r => r.Id)
                             where roleIds.Contains(ra.RoleId)
                             select ra;

            if (roleAccess.Any(ra =>
                ra.Controller.Equals(_requestControllerName, StringComparison.InvariantCultureIgnoreCase) &&
                ra.Action.Equals(_requestedActionName, StringComparison.InvariantCultureIgnoreCase)))
                return true;

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult(403);
                return;
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}