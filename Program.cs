using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository;
using _71BootlegStore.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    //options.Stores.ProtectPersonalData = false;
})
.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ApplicationUser, Role>>()
.AddRoles<Role>()
.AddEntityFrameworkStores<ApplicationDbContext>()
//.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication();

// Policy example
builder.Services.AddAuthorization(options =>
{
    foreach (var policyValue in Enum.GetNames<PolicyValueEnum>())
    {
        foreach (var policyType in (PolicyTypeEnum[])Enum.GetValues(typeof(PolicyTypeEnum)))
        {
            options.AddPolicy($"{policyType.PolicyTypeToString()} {policyValue}",
            policy => policy.RequireClaim(policyType.PolicyTypeToString(), policyValue));
        }
    }


});

// session service
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication();

/*builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        //option.Cookie.HttpOnly = true;
        option.ExpireTimeSpan = TimeSpan.FromDays(30);
        //option.SlidingExpiration = true;
    });*/

// custom service
builder.Services.AddTransient<IQueryBuilder, QueryBuilder>();
builder.Services.AddTransient<IFileUpload, FileUpload>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.LoginPath = "/";
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

/*app.UseEndpoints(endpoints => 
{
    endpoints.MapHub<SignalServer>("/notify");
});*/

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

