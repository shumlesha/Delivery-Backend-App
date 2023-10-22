using System.Reflection.Metadata;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using webNET_Hits_backend_aspnet_project_1.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using webNET_Hits_backend_aspnet_project_1;
using webNET_Hits_backend_aspnet_project_1.Middleware;
using webNET_Hits_backend_aspnet_project_1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Authsettings
builder.Services.Configure<JwtParams>(builder.Configuration.GetSection("JWT"));
var jwtConfig = builder.Configuration.GetSection("JWT");
var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidAudience = "BackOfDeliveryService",
                ValidIssuer = jwtConfig["Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        }
    );





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    
    
    options.OperationFilter<OperationFilter>();
});
builder.Services.AddScoped<IUserAccountService, UserAccountService>();



//DB:
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));




var app = builder.Build();

//DB init and update:
using var serviceScope = app.Services.CreateScope();
var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
dbContext?.Database.Migrate(); //Migration


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<AuthorizeMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

