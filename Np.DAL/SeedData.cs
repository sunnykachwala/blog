namespace Np.DAL
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Np.Common;
    using Np.DAL.Context;
    using Np.DAL.Domain;

    public static class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            using (var ctx = new NewsPortalContext(services.GetRequiredService<DbContextOptions<NewsPortalContext>>()))
            {

                #region Application Setting
                var applicationId = Guid.Parse("687EB657-26F7-4876-ADF0-EF81E03BD3EF");
                var appSetting = new AppSetting()
                {
                    Id = applicationId,
                    ApplicationName = "GJTales",
                    Logo = "logo.png",
                    AppIcon = "favicon.png",
                    DefaultCustomer = 1,
                    SaleAccount = 1,
                    PurchaseAccount = 1,
                    PayrollAccount = 1,
                    Copyright = "GJTales",
                    CompanyName = "KampherTech",
                    CompanyEmail = "kemphertech@email.com",
                    CompanyPhone = "+0123456789",
                    CompanyAddress = "-",
                    CompanyCity = "-",
                    CompanyState = "-",
                    CompanyPostalCode = "-",
                    CompanyCountry = "-",
                    CompanyTaxNumber = "TAX00000000001",
                    DefaultTimezone = "America/New_York",
                    DefaultLanguage = "en-US",
                    DefaultCurrency = "USD",
                    MailProtocol = "SMTP",
                    MailEncryption = "SSL",
                    MailHost = "host.com",
                    MailPort = 587, // YourMailPort
                    MailUserName = "email@host.com",
                    MailPassword = "Pass@1234",
                    SendInvoice = true,
                    InvoiceTemplate = "sale_thermal",
                    ThemeLayout = "sidebar-layout",
                    ThemeColor = "theme-8",
                    ThemeAppBar = "light",
                    ThemeSideBar = "light",
                    AwardedPointsPerSpent = 5,
                    RewardPointsWorth = 0.5m
                };

                if (!ctx.AppSetting.Any())
                {
                    ctx.AppSetting.Add(appSetting);
                    ctx.SaveChanges();
                }
                #endregion

                #region Role
                var roleGuid = Guid.Parse("687EB657-26F7-4876-ADF0-EF81E03BD3EF");

                var adminUserRole = new AdminUserRole()
                {
                    UserRoleGuid = roleGuid,
                    Name = "Admin",
                    IsDefaultRole = true,
                    DefaultHome = "Dashboard"
                };
                ctx.AdminUserRole.Add(adminUserRole);

                adminUserRole = new AdminUserRole()
                {
                    UserRoleGuid = Guid.NewGuid(),
                    Name = "Author",
                    IsDefaultRole = true,
                    DefaultHome = "AuthorDashboard"
                };
                ctx.AdminUserRole.Add(adminUserRole);
                if (!ctx.AdminUserRole.Any())
                {
                    ctx.SaveChanges();
                }
                #endregion

                #region Permission
                //Login, Home, Dashboard, MyProfile

                var login = new UserPermission()
                {
                    IsActive = true,
                    Permission = "Login",
                    UserPermissionId = Guid.NewGuid(),
                    CreatedBy = applicationId
                };
                var home = new UserPermission()
                {
                    IsActive = true,
                    Permission = "Home",
                    UserPermissionId = Guid.NewGuid(),
                    CreatedBy = applicationId
                };
                var dashboard = new UserPermission()
                {
                    IsActive = true,
                    Permission = "Dashboard",
                    UserPermissionId = Guid.NewGuid(),
                    CreatedBy = applicationId
                };
                var myprofile = new UserPermission()
                {
                    IsActive = true,
                    Permission = "MyProfile",
                    UserPermissionId = Guid.NewGuid(),
                    CreatedBy = applicationId,
                };

                var appSettingRole = new UserPermission()
                {
                    IsActive = true,
                    Permission = "Settings.AppSetting",
                    UserPermissionId = Guid.NewGuid(),
                    CreatedBy = applicationId,
                };

                var roles = new UserPermission()
                {
                    IsActive = true,
                    Permission = "Settings.Roles",
                    UserPermissionId = Guid.NewGuid(),
                    CreatedBy = applicationId,
                };

                var templateRole = new UserPermission()
                {
                    IsActive = true,
                    Permission = "Settings.Templates",
                    UserPermissionId = Guid.NewGuid(),
                    CreatedBy = applicationId,
                };

                var blogListRole = new UserPermission()
                {
                    IsActive = true,
                    Permission = "  ",
                    UserPermissionId = Guid.NewGuid(),
                    CreatedBy = applicationId,
                };

                if (!ctx.UserPermission.Any())
                {
                    ctx.UserPermission.Add(login);
                    ctx.UserPermission.Add(home);
                    ctx.UserPermission.Add(dashboard);
                    ctx.UserPermission.Add(myprofile);

                    ctx.UserPermission.Add(appSettingRole);
                    ctx.UserPermission.Add(roles);
                    ctx.UserPermission.Add(templateRole);
                    ctx.UserPermission.Add(blogListRole);
                    ctx.SaveChanges();
                }
                #endregion

                #region Permission Mapping
                var loginAdminMapping = new RolePermissionMapping()
                {
                    RolePermissionMappingId = Guid.NewGuid(),
                    UserPermissionId = login.UserPermissionId,
                    UserRoleId = roleGuid,
                };

                var homeAdminMapping = new RolePermissionMapping()
                {
                    RolePermissionMappingId = Guid.NewGuid(),
                    UserPermissionId = home.UserPermissionId,
                    UserRoleId = roleGuid,
                };

                var dashboadAdminMapping = new RolePermissionMapping()
                {
                    RolePermissionMappingId = Guid.NewGuid(),
                    UserPermissionId = dashboard.UserPermissionId,
                    UserRoleId = roleGuid,
                };

                var myprofileAdminMapping = new RolePermissionMapping()
                {
                    RolePermissionMappingId = Guid.NewGuid(),
                    UserPermissionId = myprofile.UserPermissionId,
                    UserRoleId = roleGuid,
                };

                var appSettingRoleMapping = new RolePermissionMapping()
                {
                    RolePermissionMappingId = Guid.NewGuid(),
                    UserPermissionId = appSettingRole.UserPermissionId,
                    UserRoleId = roleGuid,
                };

                var userRoleMapping = new RolePermissionMapping()
                {
                    RolePermissionMappingId = Guid.NewGuid(),
                    UserPermissionId = roles.UserPermissionId,
                    UserRoleId = roleGuid,
                };

                var templateRoleMapping = new RolePermissionMapping()
                {
                    RolePermissionMappingId = Guid.NewGuid(),
                    UserPermissionId = templateRole.UserPermissionId,
                    UserRoleId = roleGuid,
                };

                var blogListRoleMapping = new RolePermissionMapping()
                {
                    RolePermissionMappingId = Guid.NewGuid(),
                    UserPermissionId = blogListRole.UserPermissionId,
                    UserRoleId = roleGuid,
                };

                if (!ctx.RolePermissionMapping.Any())
                {
                    ctx.RolePermissionMapping.Add(loginAdminMapping);
                    ctx.RolePermissionMapping.Add(homeAdminMapping);
                    ctx.RolePermissionMapping.Add(dashboadAdminMapping);
                    ctx.RolePermissionMapping.Add(myprofileAdminMapping);

                    ctx.RolePermissionMapping.Add(appSettingRoleMapping);
                    ctx.RolePermissionMapping.Add(userRoleMapping);
                    ctx.RolePermissionMapping.Add(templateRoleMapping);
                    ctx.RolePermissionMapping.Add(blogListRoleMapping);
                    ctx.SaveChanges();
                }
                #endregion

                var organisationId = Guid.Parse("D05B9639-D60D-4A62-92FA-ACC9B6A59E5D");
                var organisation = new Organisation()
                {
                    OrganisationGuid = organisationId,
                    OrganisationName = "KampherTech",
                    IsActive = true
                };

                if (!ctx.Organisation.Any())
                {
                    ctx.Organisation.Add(organisation);
                    ctx.SaveChanges();
                }
                var userId = Guid.Parse("2838CDCF-BD76-44A2-9F65-26966CCBB18C");
                if (!ctx.AdminUser.Any())
                {
                    byte[] salt = UtilityHelper.GenerateSalt();
                    string randomPassword = "Test@123";
                    string hashedPassword = UtilityHelper.GenerateHashedPassword(salt, randomPassword);
                    var roleExists = ctx.AdminUserRole.Any(r => r.UserRoleGuid == roleGuid);
                    if (!roleExists)
                    {
                        throw new Exception("Specified role does not exist.");
                    }
                    else
                    {
                        var adminUser = new AdminUser()
                        {
                            UserGuid = userId,
                            UserEmail = "sunny.vikingweb@gmail.com",
                            FirstName = "Sunny",
                            LastName = "Kachwala",
                            UserRoleGuid = roleGuid,
                            IsActive = true,
                            LoginAttempts = 0,
                            IsConfirmedRegistration = true,
                            Salt = salt,
                            HashedConformationCode = null,
                            UserPasswordHash = hashedPassword,
                            TwofactorEnabled = false,
                            OrganisationGuid = organisationId,
                            AboutUser = "Sunny Kachwala",
                            AvatarUrl = "sunny.png"
                        };
                        ctx.AdminUser.Add(adminUser);
                        ctx.SaveChanges();
                    }
                
                }
            }
        }
    }
}