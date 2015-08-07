using DynamicRoleBasedAccess.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DynamicRoleBasedAccess.Controllers
{
    [Description("Access")]
    //[CustomAuthorize]
    public class AccessController : Controller
    {
        private ApplicationDbContext _dbContext;

        // GET: Access
        [Description("Access List")]
        public async Task<ActionResult> Index()
        {
            _dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var users = await (from u in _dbContext.Users
                               select new
                               {
                                   UserId = u.Id,
                                   u.UserName,
                                   Roles = _dbContext.Roles.Where(r => u.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.Name)
                               }).Select(ra => new UserRoleViewModel
                               {
                                   UserId = ra.UserId,
                                   UserName = ra.UserName,
                                   Roles = ra.Roles
                               }).ToListAsync();

            return View(users);
        }

        // GET: Access/Edit
        [Description("Edit Access")]
        public async Task<ActionResult> Edit(string id)
        {
            _dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var user = await (from u in _dbContext.Users
                              select new
                              {
                                  UserId = u.Id,
                                  u.UserName,
                                  Roles = _dbContext.Roles.Where(r => u.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.Name)
                              }).SingleOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                return HttpNotFound();

            var roles = await _dbContext.Roles.ToListAsync();

            var viewModel = new EditUserRoleViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                SelectedRoles = user.Roles,
                Roles = roles
            };

            return View(viewModel);
        }

        // POST: Access/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserRoleViewModel viewModel)
        {
            _dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            if (!ModelState.IsValid)
            {
                viewModel.Roles = await _dbContext.Roles.ToListAsync();
                return View(viewModel);
            }

            var user = _dbContext.Users.Find(viewModel.UserId);
            user.Roles.Clear();
            foreach (var roleId in viewModel.SelectedRoles)
            {
                user.Roles.Add(new IdentityUserRole { RoleId = roleId });
            }
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}