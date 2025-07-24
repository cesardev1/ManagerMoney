using Dapper;
using ManagerMoney.Models;
using ManagerMoney.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);


// Add secrets configuration
var secrets = builder.Configuration.GetSection("Secrets").Get<SecretsOptions>() ?? new SecretsOptions();
secrets.ConnectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING_DEFAULT") ?? secrets.ConnectionString;


// Add services to the container.

var userAuthPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter(userAuthPolicy));
});
builder.Services.AddSingleton(secrets);
builder.Services.AddTransient<IAccountTypeRepository, AccountTypeRepository>();
builder.Services.AddTransient<IUserServices, UserServices>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IReportService, ReportService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IUserStore<User>, UserStore>();
builder.Services.AddIdentityCore<User>().AddErrorDescriber<ErrorMessageIdentity>();
builder.Services.AddTransient<SignInManager<User>>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/user/login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transaction}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
