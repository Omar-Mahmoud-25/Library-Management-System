using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Services.Interfaces;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
    });
builder.Services.AddAuthorization();

// Dependency Injection for Repositories and Services
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

// Seed admin user
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LMSContext>();
    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
    
    // Ensure database is created
    context.Database.EnsureCreated();
    
    // Check if admin exists
    var adminEmail = "admin@library.com";
    var existingAdmin = context.Users.FirstOrDefault(u => u.Email == adminEmail);
    
    if (existingAdmin == null)
    {
        var adminUser = new User
        {
            FirstName = "System",
            LastName = "Administrator",
            Email = adminEmail,
            PhoneNumber = "1234567890",
            IsAdmin = true,
            IsActive = true,
            JoiningDate = DateTime.Now
        };
        
        // Use your service to hash password properly
        await accountService.RegisterAsync(adminUser, "Admin123!");
        Console.WriteLine("Admin user created successfully!");
    }
}

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
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
