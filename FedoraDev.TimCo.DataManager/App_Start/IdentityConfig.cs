using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using FedoraDev.TimCo.DataManager.Models;

namespace FedoraDev.TimCo.DataManager
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
			ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            ApplyUserValidationLogic(ref manager);
            ApplyPasswordValidationLogic(ref manager);
            ApplyDataProtectionProvider(ref manager, options.DataProtectionProvider);
			
            return manager;
        }

        static void ApplyUserValidationLogic(ref ApplicationUserManager manager)
		{
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
        }

        static void ApplyPasswordValidationLogic(ref ApplicationUserManager manager)
		{
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
        }

        static void ApplyDataProtectionProvider(ref ApplicationUserManager manager, IDataProtectionProvider dataProtectionProvider)
		{
            if (dataProtectionProvider != null)
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        }
    }
}
