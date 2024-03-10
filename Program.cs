using AuthApp.Core.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Add DB

builder.Services.
    AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("local");
    // You can use the connectionString here to configure your DbContext options
    options.UseSqlServer(connectionString);
    // Other configuration as needed for your database provider
});

//Add Identity

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>{})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Config Identity

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireNonAlphanumeric = false;

});

//Add Authentication and Jwtbearer
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        string? validIssuer = builder.Configuration["JWT:ValidIssuer"];
        string? validAudience = builder.Configuration["JWT:ValidAudience"];
        string? jwtSecret = builder.Configuration["JWT:Secret"];

        if (!string.IsNullOrEmpty(validIssuer) &&
            !string.IsNullOrEmpty(validAudience) &&
            !string.IsNullOrEmpty(jwtSecret))
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            };
        }
        else
        {
            // Handle the case where JWT configuration values are missing or invalid
            // This might involve logging an error, using defaults, or returning an error response
            // For example:
            throw new InvalidOperationException("JWT configuration values are missing or invalid.");
        }
    });


//Pipeline

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
