using Admin.Np.WebApi.Helper;
using Admin.Np.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Np.Admin.Service;
using Np.Admin.Service.ActivityLogs;
using Np.Admin.Service.AdminUsers;
using Np.Admin.Service.AppSettings;
using Np.Admin.Service.Articles;
using Np.Admin.Service.Categories;
using Np.Admin.Service.Emails;
using Np.Admin.Service.LoginHistory;
using Np.Admin.Service.Organisations;
using Np.Admin.Service.Tags;
using Np.Admin.Service.Token;
using Np.Admin.Service.UrlRecords;
using Np.DAL;
using Np.DAL.Context;
using Np.DAL.Repository;
using Np.ViewModel;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDataProtection();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    })
   .AddNewtonsoftJson()
   .AddNewtonsoftJson(options =>
   {
       // Ignore circular references
       options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
       // Use camel case property names
       options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
   });

// Set the JSON serializer options
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
});
var configuration = builder.Configuration;

var appConfig = configuration.Get<AppConfig>();
builder.Services.Configure<AppConfig>(conf => conf.DomainConfig = appConfig.DomainConfig);
builder.Services.Configure<AppConfig>(conf => conf.MicrosoftAuthenticationApp = appConfig.MicrosoftAuthenticationApp);
builder.Services.Configure<AppConfig>(conf => conf.AdminUrl = appConfig.AdminUrl);
builder.Services.Configure<AppConfig>(conf => conf.Password = appConfig.Password);
builder.Services.Configure<AppConfig>(conf => conf.Jwt = appConfig.Jwt);

builder.Services.Configure<AppConfig>(conf => conf.EmailSettings = appConfig.EmailSettings);
builder.Services.Configure<AppConfig>(conf => conf.SMTPSettings = appConfig.SMTPSettings);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
#if !DEBUG
     options.RequireHttpsMetadata = false; // Set to true in production
#endif
           options.SaveToken = true;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.Jwt.Key)),
               ValidateIssuer = false,
               ValidateAudience = false,
               ValidateLifetime = true,
           };
       });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "E-PV API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        BearerFormat = "Bearer {token}",
        Scheme = "bearer",
        Description = "Enter your JWT token in the text input below.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
});


builder.Services.AddDbContext<NewsPortalContext>(o =>
{
    o.UseSqlServer(appConfig.ConnectionStrings.NewsSystemDatabaseValue);
    //o.UseSqlServer(appConfig.ConnectionStrings.NewsSystemDatabaseValue,
    //     providerOptions => providerOptions.EnableRetryOnFailure(
    //         maxRetryCount: 2,
    //         maxRetryDelay: TimeSpan.FromSeconds(10),
    //         errorNumbersToAdd: null
    //         ));

});


builder.Services.TryAddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

builder.Services.TryAddTransient<IAdminUserService, AdminUserService>();
builder.Services.TryAddTransient<IActivityLogService, ActivityLogService>();
builder.Services.TryAddTransient<IArticleService, ArticleService>();
builder.Services.TryAddTransient<IAppSettingService, AppSettingService>();

builder.Services.TryAddTransient<IUrlRecordService, UrlRecordService>();

builder.Services.TryAddTransient<ICategoryService, CategoryService>();

builder.Services.TryAddTransient<IEmailService, EmailService>();

builder.Services.TryAddTransient<ILoginHistoryService, LoginHistoryService>();

builder.Services.TryAddTransient<IOrganisationService, OrganisationService>();

builder.Services.TryAddTransient<ISqlHelper, SqlHelper>();

builder.Services.TryAddTransient<ITagService, TagService>();
builder.Services.TryAddTransient<ITokenService, TokenService>();

builder.Services.TryAddTransient<IUrlRecordService, UrlRecordService>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(x => x
             .SetIsOriginAllowed(origin => true)
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials());

app.UseMiddleware<ApiResponseMiddleware>();
// Hook in the global error-handling middleware
app.UseMiddleware(typeof(ErrorHandlingMiddleware));


//app.UseMiddleware<ClaimsMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();