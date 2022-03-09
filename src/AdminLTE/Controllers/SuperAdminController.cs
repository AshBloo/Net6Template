using AdminLTE.Data;
using AdminLTE.Models;
using AdminLTE.Models.SuperAdminViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdminLTE.Controllers
{

    [Authorize(Roles = "SuperAdmins")]
    public class SuperAdminController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IUserValidator<ApplicationUser> userValidator;
        private IPasswordValidator<ApplicationUser> passwordValidator;
        private IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly IWebHostEnvironment hostingEnvironment;
private readonly ApplicationDbContext _context;

        private ApplicationUser testUser = new ApplicationUser
        {
            UserName = "TestTestForPassword",
            Email = "testForPassword@test.test"
        };

        public SuperAdminController(UserManager<ApplicationUser> userMgr,
            IUserValidator<ApplicationUser> userValid, IPasswordValidator<ApplicationUser> passValid,
            IPasswordHasher<ApplicationUser> passHasher, IWebHostEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            userManager = userMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passHasher;
            this.hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        // GET: /<controller>/
        public ViewResult Index()
        {
            return View(userManager.Users);
        }

        // GET: /<controller>/
        public ViewResult AuditList()
        {

            var audits = _context.Audit.ToList();
            return View(audits);
        }
        // GET: QuestionAnswers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audit = await _context.Audit
                .FirstOrDefaultAsync(m => m.Id == id);
            if (audit == null)
            {
                return NotFound();
            }

            return View(audit);
        }

        public JsonResult Audit(int id)
        {
            Audit SD = new Audit();
            var AuditTrail = GetAudit(id);
            return Json(AuditTrail);
        }

        public List<Audit> GetAudit(int ID)
        {
            List<Audit> rslt = new List<Audit>();
           
            var AuditTrail = _context.Audit.Where(s => s.Id == ID).OrderByDescending(s => s.AuditDateTimeUtc);
            var serializer = new XmlSerializer(typeof(Audit));
            foreach (var record in AuditTrail)
            {
                //AuditChange Change = new AuditChange();
                //Change.DateTimeStamp = record.DateTimeStamp.ToString();
                //Change.AuditActionType = (AuditActionType)record.AuditActionTypeENUM;
                //Change.AuditActionTypeName = Enum.GetName(typeof(AuditActionType), record.AuditActionTypeENUM);
                //List<AuditDelta> delta = new List<AuditDelta>();
                //delta = JsonConvert.DeserializeObject<List<AuditDelta>>(record.Changes);
                //Change.Changes.AddRange(delta);
                rslt.Add(record);
            }
            return rslt;
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVm createVm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = createVm.Email,
                    Email = createVm.Email,
                    //extended properties
                    FirstName = createVm.FirstName,
                    LastName = createVm.LastName,
                    AvatarURL = "/images/user.png",
                    DateRegistered = DateTime.UtcNow.ToString(),
                    Position = "Top",
                    NickName = "",
                };

                IdentityResult result = await userManager.CreateAsync(user, createVm.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(createVm);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.TryAddModelError("", error.Description);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View("Index", userManager.Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // the names of its parameters must be the same as the property of the User class if we use asp-for in the view
        // otherwise form values won't be passed properly
        public async Task<IActionResult> Edit(string id, string userName, string email, string firstname, string lastname, string nickname, string position, IFormFile file)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Validate UserName and Email 
                user.UserName = email; // UserName won't be changed in the database until UpdateAsync is executed successfully
                user.Email = email;
                user.FirstName = firstname;
                user.LastName = lastname;
                user.NickName = nickname;
                user.Position = position;

               

                IdentityResult validUseResult = await userValidator.ValidateAsync(userManager, user);
                if (!validUseResult.Succeeded)
                {
                    AddErrors(validUseResult);
                }

                // Update user info
                if (validUseResult.Succeeded)
                {
                
                   string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name


                    if (file != null && file.Length > 0)
                    {
                       string uniqueFileName = user.Id.ToString() + "_" + Path.GetExtension(file.FileName); //model.DocumentFile.FileName;
                       string filePath = Path.Combine(uploadsFolder, uniqueFileName);//Path.Combine(@"\\fs01\Archive\", uniqueFileName);


                        file.CopyTo(new FileStream(filePath, FileMode.Create));
                        user.AvatarURL = filePath;
                    }
                    else
                    {
                        if (user.AvatarURL != null)
                        {
                            user.AvatarURL = user.AvatarURL;
                        }
                        else
                        {
                            throw new FileNotFoundException("This file was not found.");
                        }

                    }


                    //var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim();
                    //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", file.FileName);
                    //var path = Path.Combine(Server.MapPath("~/images"), file);

                    //using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                    //{
                    //    await file.CopyToAsync(stream);
                    //}
                  

                    // UpdateAsync validates user info such as UserName and Email except password since it's been hashed 
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "SuperAdmin");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            ;

            return View(user);
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, string password)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Validate password
                // Step 1: using built in validations
                IdentityResult passwordResult = await userManager.CreateAsync(testUser, password);
                if (passwordResult.Succeeded)
                {
                    await userManager.DeleteAsync(testUser);
                }
                else
                {
                    AddErrors(passwordResult);
                }
                /* Step 2: Because of DI, IPasswordValidator<User> is injected into the custom password validator. 
                   So the built in password validation stop working here */
                IdentityResult validPasswordResult = await passwordValidator.ValidateAsync(userManager, user, password);
                if (validPasswordResult.Succeeded)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                }
                else
                {
                    AddErrors(validPasswordResult);
                }

                // Update user info
                if (passwordResult.Succeeded && validPasswordResult.Succeeded)
                {
                    // UpdateAsync validates user info such as UserName and Email except password since it's been hashed 
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "SuperAdmin");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View(user);
        }
    }
}
