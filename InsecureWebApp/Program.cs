using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using InsecureWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// SECURITY VULNERABILITY: No HTTPS redirect configured
// SECURITY VULNERABILITY: Hardcoded connection string with credentials
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=localhost;Database=InsecureApp;User Id=sa;Password=P@ssw0rd123;TrustServerCertificate=true;"));

// SECURITY VULNERABILITY: Weak JWT configuration without proper validation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // VULNERABILITY: Not validating issuer
            ValidateAudience = false, // VULNERABILITY: Not validating audience
            ValidateLifetime = false, // VULNERABILITY: Not validating token expiration
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("weak-secret-key")) // VULNERABILITY: Weak secret
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SECURITY VULNERABILITY: No CORS policy configured properly
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // VULNERABILITY: Allows any origin
              .AllowAnyMethod() // VULNERABILITY: Allows any HTTP method
              .AllowAnyHeader(); // VULNERABILITY: Allows any header
    });
});

var app = builder.Build();

// SECURITY VULNERABILITY: Detailed error information exposed in production
if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) // Always true!
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); // VULNERABILITY: Exposes stack traces in production
}

// SECURITY VULNERABILITY: No HTTPS redirect
// app.UseHttpsRedirection(); // Commented out - VULNERABILITY

app.UseCors();

// SECURITY VULNERABILITY: No rate limiting
// SECURITY VULNERABILITY: No request size limits

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();