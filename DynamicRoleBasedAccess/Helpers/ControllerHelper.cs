using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace DynamicRoleBasedAccess.Helpers
{
    /// <summary>
    /// A Utility class for MVC controllers.
    /// </summary>
    public class ControllerHelper
    {
        /// <summary>
        /// Gets the controllers name an description with their actions.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>Lazy&lt;IEnumerable&lt;ControllerDescription&gt;&gt;.</returns>
        public IEnumerable<ControllerDescription> GetControllersNameAnDescription(string filter = null)
        {
            var assembly = Assembly.GetCallingAssembly();
            var controllers = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Controller)))
                .ToList();

            if (!string.IsNullOrWhiteSpace(filter))
                controllers = controllers.Where(t => !t.Name.Contains(filter)).ToList();

            var controllerList = (
                    from controller in controllers
                    let attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(controller, typeof(DescriptionAttribute))
                    let ctrlDescription = attribute == null ? "" : attribute.Description
                    select new ControllerDescription
                    {
                        Name = controller.Name.Replace("Controller", ""),
                        Description = ctrlDescription,
                        Actions = GetActionList(controller)
                    }
                ).ToList();

            return controllerList;
        }

        /// <summary>
        /// Gets the action list.
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns>IEnumerable&lt;ActionDescription&gt;.</returns>
        private static IEnumerable<ActionDescription> GetActionList(Type controllerType)
        {
            var actions = new ReflectedControllerDescriptor(controllerType).GetCanonicalActions().ToList();

            var actionList = (from actionDescriptor in actions
                              let attribute = actionDescriptor.GetCustomAttributes(typeof(DescriptionAttribute), false).LastOrDefault() as DescriptionAttribute
                              let acnDescription = attribute == null ? "" : attribute.Description
                              select new ActionDescription
                              {
                                  Name = actionDescriptor.ActionName,
                                  Description = acnDescription
                              }).ToList();
            actionList = actionList.GroupBy(a => a.Name).Select(g => g.First()).ToList();
            return actionList;
        }
    }

    /// <summary>
    ///  Controller description class.
    /// </summary>
    public class ControllerDescription
    {
        /// <summary>
        /// Gets or sets the name of controller.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of controller.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        /// <value>The actions.</value>
        public IEnumerable<ActionDescription> Actions { get; set; }
    }

    /// <summary>
    /// Action description class.
    /// </summary>
    public class ActionDescription
    {
        /// <summary>
        /// Gets or sets the name of action.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of action.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
    }
}