using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AdminLTE.Common;
using AdminLTE.Models;
using System.Security.Claims;
using AdminLTE.Common.Extensions;
using System;
using AdminLTE.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AdminLTE.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public SidebarViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string filter)
        {
            //you can do the access rights checking here by using session, user, and/or filter parameter
            var sidebars = new List<SidebarMenu>();

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewData["UserMenuType"] = currentUser.Position.ToString().ToUpper();
            sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.Home));

            if (User.IsInRole("SuperAdmins"))
            {
                sidebars.Add(ModuleHelper.AddTree("Administration"));
                sidebars.Last().TreeChild = new List<SidebarMenu>()
                {
                    ModuleHelper.AddModule(ModuleHelper.Module.SuperAdmin),
                    ModuleHelper.AddModule(ModuleHelper.Module.Role),
                    ModuleHelper.AddModule(ModuleHelper.Module.UserLogs),
                    ModuleHelper.AddModule(ModuleHelper.Module.AuditLogs)
                };
             
            }

            return View(sidebars);
        }
    }
}
