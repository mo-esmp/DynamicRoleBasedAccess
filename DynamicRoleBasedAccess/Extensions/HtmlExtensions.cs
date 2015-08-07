using DynamicRoleBasedAccess.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace DynamicRoleBasedAccess.Extensions
{
    /// <summary>
    /// Represents extension methods for HtmlHelper class.
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// Returns an anchor element (a element) for the specified link text and action.
        /// </summary>
        ///
        /// <returns>
        /// An anchor element (a element).
        /// </returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param><param name="linkText">The inner text of the anchor element.</param><param name="actionName">The name of the action.</param><exception cref="T:System.ArgumentException">The <paramref name="linkText"/> parameter is null or empty.</exception>
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, null, new RouteValueDictionary(), new RouteValueDictionary());
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified link text, action, and route values.
        /// </summary>
        ///
        /// <returns>
        /// An anchor element (a element).
        /// </returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param><param name="linkText">The inner text of the anchor element.</param><param name="actionName">The name of the action.</param><param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param><exception cref="T:System.ArgumentException">The <paramref name="linkText"/> parameter is null or empty.</exception>
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, null, routeValues, (IDictionary<string, object>)new RouteValueDictionary());
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified link text, action, route values, and HTML attributes.
        /// </summary>
        ///
        /// <returns>
        /// An anchor element (a element).
        /// </returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param><param name="linkText">The inner text of the anchor element.</param><param name="actionName">The name of the action.</param><param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param><param name="htmlAttributes">An object that contains the HTML attributes for the element. The attributes are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param><exception cref="T:System.ArgumentException">The <paramref name="linkText"/> parameter is null or empty.</exception>
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, null, routeValues, htmlAttributes);
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified link text, action, and route values as a route value dictionary.
        /// </summary>
        ///
        /// <returns>
        /// An anchor element (a element).
        /// </returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param><param name="linkText">The inner text of the anchor element.</param><param name="actionName">The name of the action.</param><param name="routeValues">An object that contains the parameters for a route.</param><exception cref="T:System.ArgumentException">The <paramref name="linkText"/> parameter is null or empty.</exception>
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, null, routeValues, new RouteValueDictionary());
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified link text, action, route values as a route value dictionary, and HTML attributes as a dictionary.
        /// </summary>
        ///
        /// <returns>
        /// An anchor element (a element).
        /// </returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param><param name="linkText">The inner text of the anchor element.</param><param name="actionName">The name of the action.</param><param name="routeValues">An object that contains the parameters for a route.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><exception cref="T:System.ArgumentException">The <paramref name="linkText"/> parameter is null or empty.</exception>
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, null, routeValues, htmlAttributes);
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified link text, action, and controller.
        /// </summary>
        ///
        /// <returns>
        /// An anchor element (a element).
        /// </returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param><param name="linkText">The inner text of the anchor element.</param><param name="actionName">The name of the action.</param><param name="controllerName">The name of the controller.</param><exception cref="T:System.ArgumentException">The <paramref name="linkText"/> parameter is null or empty.</exception>
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified link text, action, controller, route values, and HTML attributes.
        /// </summary>
        ///
        /// <returns>
        /// An anchor element (a element).
        /// </returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param><param name="linkText">The inner text of the anchor element.</param><param name="actionName">The name of the action.</param><param name="controllerName">The name of the controller.</param><param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><exception cref="T:System.ArgumentException">The <paramref name="linkText"/> parameter is null or empty.</exception>
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            //var url = htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);

            return CanAccess(htmlHelper, actionName, controllerName)
                ? htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes)
                : new MvcHtmlString(string.Empty);
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified link text, action, controller, route values as a route value dictionary, and HTML attributes as a dictionary.
        /// </summary>
        ///
        /// <returns>
        /// An anchor element (a element).
        /// </returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param><param name="linkText">The inner text of the anchor element.</param><param name="actionName">The name of the action.</param><param name="controllerName">The name of the controller.</param><param name="routeValues">An object that contains the parameters for a route.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><exception cref="T:System.ArgumentException">The <paramref name="linkText"/> parameter is null or empty.</exception>
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return CanAccess(htmlHelper, actionName, controllerName)
                ? htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes)
                : new MvcHtmlString(string.Empty);
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified link text, action, controller, protocol, host name, URL fragment, route values, and HTML attributes.
        /// </summary>
        ///
        /// <returns>
        /// An anchor element (a element).
        /// </returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param><param name="linkText">The inner text of the anchor element.</param><param name="actionName">The name of the action.</param><param name="controllerName">The name of the controller.</param><param name="protocol">The protocol for the URL, such as "http" or "https".</param><param name="hostName">The host name for the URL.</param><param name="fragment">The URL fragment name (the anchor name).</param><param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><exception cref="T:System.ArgumentException">The <paramref name="linkText"/> parameter is null or empty.</exception>
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return CanAccess(htmlHelper, actionName, controllerName)
                ? htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes)
                : new MvcHtmlString(string.Empty);
        }

        /// <summary>
        /// Determines whether this instance can access the requested Controller and Action.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns><c>true</c> if this instance can access the specified HTML helper; otherwise, <c>false</c>.</returns>
        private static bool CanAccess(HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext;
            var dbContext = httpContext.GetOwinContext().Get<ApplicationDbContext>();
            var user = httpContext.User;
            var roleAccess = from ra in dbContext.RoleAccesses
                             let userId = dbContext.Users.FirstOrDefault(u => u.UserName == user.Identity.Name).Id
                             let roleIds = dbContext.Roles.Where(r => r.Users.Any(u => u.UserId == userId)).Select(r => r.Id)
                             where roleIds.Contains(ra.RoleId)
                             select ra;

            if (string.IsNullOrWhiteSpace(controllerName))
                controllerName = htmlHelper.ViewContext.Controller.ToString().Split('.').Last().Replace("Controller", "");

            if (roleAccess.Any(ra =>
                ra.Controller.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase) &&
                ra.Action.Equals(actionName, StringComparison.InvariantCultureIgnoreCase)))
                return true;

            return false;
        }
    }
}