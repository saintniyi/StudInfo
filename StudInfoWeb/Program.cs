using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudInfoDataAccess.Data;
using StudInfoDataAccess.IRepository;
using StudInfoDataAccess.Repository;
using StudInfoModel;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

//builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseExceptionHandler("/ErrorHandler");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/ErrorHandler");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseStaticFiles(new StaticFileOptions()
//{
//    OnPrepareResponse = context =>
//    {
//        context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
//        context.Context.Response.Headers.Add("Expires", "-1");
//    }
//});

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
