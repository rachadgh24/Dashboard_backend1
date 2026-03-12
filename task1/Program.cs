using task1.Application.DependencyInjection;
using task1.DataLayer.DependencyInjection;
using task1.DataLayer.Entities;
using task1.DataLayer.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===== Add Services =====
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("NextPolicy",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationLayerServices(builder.Configuration);
// builder.Services.AddDataLayerRepositories(); // keep if needed

// ===== JWT Authentication =====
var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"])),

        RoleClaimType = "role",
        NameClaimType = "name"
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            // Support old tokens that used long claim URIs: ensure "role" and "name" exist for [Authorize] and User.Identity.Name
            var identity = (ClaimsIdentity?)context.Principal?.Identity;
            if (identity == null) return Task.CompletedTask;

            if (identity.FindFirst("role") == null)
            {
                var oldRole = identity.FindFirst(ClaimTypes.Role);
                if (oldRole != null)
                    identity.AddClaim(new Claim("role", oldRole.Value));
            }
            if (identity.FindFirst("name") == null)
            {
                var oldName = identity.FindFirst(ClaimTypes.Name) ?? identity.FindFirst("unique_name");
                if (oldName != null)
                    identity.AddClaim(new Claim("name", oldName.Value));
            }

            return Task.CompletedTask;
        },
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"message\":\"Unauthorized: please sign in again.\"}");
        },
        OnForbidden = async context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"message\":\"Forbidden: you do not have permission to access this resource.\"}");
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NextPolicy");
app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();