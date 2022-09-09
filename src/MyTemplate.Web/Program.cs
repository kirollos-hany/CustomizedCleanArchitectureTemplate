using System.Text;
using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MyTemplate.Core;
using MyTemplate.Core.Security.Entities;
using MyTemplate.Web.Security.Token.Configuration;
using MyTemplate.Infrastructure;
using MyTemplate.Infrastructure.Data;
using MyTemplate.Web.Extensions;
using MyTemplate.Web.Filters;
using MyTemplate.Web.Middlewares;
using ExtCore.FileStorage;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using MyTemplate.Web.Security.Token.Providers;
using MyTemplate.Web.Security.Token.Interfaces;
using MyTemplate.Core.Security.Interfaces;
using MyTemplate.Web.Security.Providers;
using Microsoft.AspNetCore.HttpOverrides;
using MyTemplate.Web.Security.Authentication;
using MyTemplate.Core.Security.Enums;

var builder = WebApplication.CreateBuilder(args);
#region logging

Log.Logger = new LoggerConfiguration()
  .WriteTo
  .Console()
  .CreateBootstrapLogger();

builder
  .Host
  .UseSerilog((context, loggingConfig) => loggingConfig
    .WriteTo
    .Console()
    .ReadFrom
    .Configuration(context.Configuration));

#endregion

#region Identity Configuration
builder.Services.AddIdentity<User, Role>(options =>
{
  //identity configuration goes here
})
.AddRoles<Role>()
.AddEntityFrameworkStores<ApplicationDbContext>();

#endregion 

#region JWT
builder.Services.Configure<IJwtConfig>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<IJwtConfig>(services => services.GetRequiredService<IOptions<IJwtConfig>>().Value);

builder
.Services
.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters()
  {
    ValidateActor = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],
    ValidAudience = builder.Configuration["JWT:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secrets"]))
  };
  options.TokenValidationParameters.RoleClaimType = nameof(ClaimsTypes.Roles);
});


builder.Services.AddScoped(typeof(IClaimsProvider<>), typeof(ClaimsProvider<>));
builder.Services.AddScoped(typeof(IJwtProvider), typeof(JwtProvider));
builder.Services.AddScoped(typeof(IAuthenticator<>), typeof(Authenticator<>));
#endregion

#region authorization
builder.Services.AddAuthorization(options => options.AddPolicies());
#endregion 

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

#region ef core config
var sqlServerConnection = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext(sqlServerConnection);
#endregion

builder.Services.AddMvc().AddFluentValidation(options =>
{
  options.AutomaticValidationEnabled = false;
  options.ImplicitlyValidateRootCollectionElements = true;
  options.ImplicitlyValidateChildProperties = true;
  options.RegisterValidatorsFromAssemblyContaining<DefaultCoreModule>();
});

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddRazorPages();

builder.Services.AddSwaggerGen(c =>
{
  c.OperationFilter<AuthorizeOperationFilter>();
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyTemplate", Version = "v1" });
  c.EnableAnnotations();
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));
  containerBuilder.RegisterModule(new DefaultCoreModule());
});

builder.Logging.ClearProviders().AddConsole();

builder.Services.Configure<FileStorageOptions>(options =>
{
  var path = Path.Combine(builder.Environment.ContentRootPath, "Files");
  options.RootPath = path;
});

var app = builder.Build();

app.UseCors(
  options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
);

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware();
}
else
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}
app.UseMiddleware<ValidationMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.UseRouting();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyTemplate V1");
  c.DocumentTitle = "MyTemplate";
});
app.UseEndpoints(endpoints =>
{
  endpoints.MapDefaultControllerRoute();
  endpoints.MapRazorPages();
});

#region database seed and migration
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  try
  {
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    context.Database.EnsureCreated();
  }
  catch (Exception ex)
  {
    Log.Error(ex, "An error occurred seeding/migrating the DB. {exceptionMessage}", ex.Message);
  }
}
#endregion 

app.Run();
