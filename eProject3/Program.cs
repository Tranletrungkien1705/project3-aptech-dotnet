using eProject3.Data;
using eProject3.Models;
using eProject3.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentity<CustomUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();
        builder.Services.AddControllersWithViews();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultChallengeScheme = "CustomRedirect";
        })
        .AddCookie("CustomRedirect", options =>
        {
            options.LoginPath = new PathString("/Identity/Account/Login"); // Replace with your custom login path
            options.AccessDeniedPath = new PathString("/Identity/Account/Login"); // Replace with your custom access denied path
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Optional: set the expiration time for the cookie
        });
        
                builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
        builder.Services.AddScoped<IStaffRepository, StaffRepository>();
        builder.Services.AddScoped<IStudentRepository, StudentRespository>();
        builder.Services.AddScoped<ICompetitionRepository, CompetitionRepository>();
        builder.Services.AddScoped<IAwardRepository, AwardRepository>();
        builder.Services.AddScoped<IExhibitionRepository, ExhibitionRepository>();
        builder.Services.AddScoped<IMarkOfPaintInCompRepository,MarkOfPaintInCompRepository>();
        builder.Services.AddScoped<IPaintingInCompRepository, PaintingInCompRepository>();
        builder.Services.AddScoped<IPaintingInExRepository, PaintingInExRepository>();
        builder.Services.AddScoped<IPaintingRepository, PaintingRepository>();
        builder.Services.AddScoped<ISubInClassRepository, SubInClassRepository>();
        builder.Services.AddScoped<IClassRepository, ClassRepository>();
        builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
        builder.Services.AddScoped<IStudentInSubRepository, StudentInSubRepository>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });
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

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        //app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Art Institute");
        });
        app.Run();
    }
}