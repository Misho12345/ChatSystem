using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using ChatService.Hubs;
using ChatService.Services;
using ChatService.Models.Configuration;

// Create a WebApplication builder
var builder = WebApplication.CreateBuilder(args);

// Configure JwtSettings from the configuration file
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// Configure MongoDbSettings from the configuration file
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("ConnectionStrings"));
var mongoDbSettings = builder.Configuration.GetSection("ConnectionStrings").Get<MongoDbSettings>();

// Register MongoDB client and database as singleton services
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoDbSettings!.MongoConnection));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbSettings!.MongoDatabaseName));

// Register ConversationService as a scoped service
builder.Services.AddScoped<IConversationService, ConversationService>();

// Add SignalR with detailed error configuration for development environment
builder.Services.AddSignalR(options => { options.EnableDetailedErrors = builder.Environment.IsDevelopment(); });

// Add controllers and API documentation services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Include XML comments for Swagger documentation
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Define token validation parameters
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Secret)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Issuer,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        // Handle token retrieval for SignalR connections
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

// Add authorization services
builder.Services.AddAuthorization();

// Configure CORS to allow requests from specific origins
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

// Build the application
var app = builder.Build();

// Ensure MongoDB indexes are created
using (var scope = app.Services.CreateScope())
{
    var svc = scope.ServiceProvider.GetRequiredService<IConversationService>();
    await svc.EnsureIndexesAsync();
}

// Enable Swagger in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure middleware for HTTPS redirection, CORS, authentication, and authorization
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

// Map controllers and SignalR hub
app.MapControllers();
app.MapHub<ChatHub>("/chathub");

// Run the application
app.Run();