using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserAccountService.Data;
using UserAccountService.Models.Configuration;
using UserAccountService.Services;
using UserAccountService.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configure JWT settings from the application configuration.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// Configure the database context to use PostgreSQL with the connection string from the configuration.
builder.Services.AddDbContext<UserAccountDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// Register application services for dependency injection.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IFriendshipService, FriendshipService>();

// Add SignalR for real-time communication.
builder.Services.AddSignalR();

// Add controllers for handling HTTP requests.
builder.Services.AddControllers();

// Configure Swagger for API documentation generation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Include XML comments for better documentation in Swagger.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Configure authentication using JWT bearer tokens.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Set token validation parameters for JWT authentication.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Secret)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    // Handle token retrieval for SignalR hubs.
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/chathub") || path.StartsWithSegments("/friendshiphub")))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Add authorization services.
builder.Services.AddAuthorization();

// Configure CORS to allow requests from the frontend application.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policyBuilder =>
        {
            policyBuilder.WithOrigins(
                    "http://localhost:3000",
                    "https://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
        });
});

var app = builder.Build();

// Enable Swagger UI in development mode.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enforce HTTPS redirection for secure communication.
app.UseHttpsRedirection();

// Apply the CORS policy.
app.UseCors("AllowFrontend");

// Enable authentication and authorization middleware.
app.UseAuthentication();
app.UseAuthorization();

// Map controller routes and SignalR hubs.
app.MapControllers();
app.MapHub<FriendshipHub>("/friendshiphub");

// Apply database migrations at application startup.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserAccountDbContext>();
    dbContext.Database.Migrate();
}

// Run the application.
app.Run();