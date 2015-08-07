using DynamicRoleBasedAccess.Helpers;
using DynamicRoleBasedAccess.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DynamicRoleBasedAccess.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private static Lazy<IEnumerable<ControllerDescription>> _controllerList = new Lazy<IEnumerable<ControllerDescription>>();

        public RoleController()
        {
        }

        public RoleController(ApplicationRoleManager roleManager)
        {
            RoleManager = roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            private set { _roleManager = value; }
        }

        // GET: Role
        [Description("Role List")]
        public ActionResult Index()
        {
            var roles = RoleManager.Roles;
            return View(roles);
        }

        // GET: Role/Create
        [Description("Create Role")]
        public ActionResult Create()
        {
            var createRoleViewModel = new CreateRoleViewModel
            {
                Controllers = GetControllers()
            };
            return View(createRoleViewModel);
        }

        // POST: ROle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateRoleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Controllers = GetControllers();
                return View(viewModel);
            }

            var role = new ApplicationRole
            {
                Name = viewModel.Name,
                RoleAccesses = new List<RoleAccess>()
            };

            //
            foreach (var controller in viewModel.SelectedControllers)
            {
                foreach (var action in controller.Actions)
                {
                    role.RoleAccesses.Add(new RoleAccess { Controller = controller.Name, Action = action.Name });
                }
            }

            await RoleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }

        // GET: /Role/Edit
        [Description("Edit Role")]
        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
                return HttpNotFound();

            var viewModel = new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                RoleAccesses = new List<RoleAccess>(role.RoleAccesses),
                Controllers = GetControllers()
            };

            return View(viewModel);
        }

        // POST: /Role/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditRoleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Controllers = GetControllers();
                return View(viewModel);
            }

            var role = await RoleManager.FindByIdAsync(viewModel.Id);
            role.Name = viewModel.Name;
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            dbContext.RoleAccesses.RemoveRange(role.RoleAccesses);

            foreach (var controller in viewModel.SelectedControllers)
            {
                foreach (var action in controller.Actions)
                {
                    role.RoleAccesses.Add(new RoleAccess
                    {
                        Action = action.Name,
                        Controller = controller.Name,
                        RoleId = role.Id,
                        Role = role,
                    });
                }
            }
            await RoleManager.UpdateAsync(role);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: /Role/Delete
        [Description("Delete Role")]
        public async Task<JsonResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            await RoleManager.DeleteAsync(role);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private static IEnumerable<ControllerDescription> GetControllers()
        {
            if (_controllerList.IsValueCreated)
                return _controllerList.Value;

            _controllerList = new Lazy<IEnumerable<ControllerDescription>>(() => new ControllerHelper().GetControllersNameAnDescription());
            return _controllerList.Value;
        }
    }
}