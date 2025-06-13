// ChatApp.ChatService/Program.cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using ChatService.Hubs;
using ChatService.Services;
using ChatService.Models.Configuration; // For JwtSettings and MongoDbSettings

var builder = WebApplication.CreateBuilder(args);

// Configure Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("ConnectionStrings")); // Assuming MongoConnection and MongoDatabaseName are under ConnectionStrings
var mongoDbSettings = builder.Configuration.GetSection("ConnectionStrings").Get<MongoDbSettings>();


// Add services to the container.

// MongoDB Client Registration
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoDbSettings.MongoConnection));
builder.Services.AddSingleton(sp => // Register IMongoDatabase
    sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbSettings.MongoDatabaseName));

builder.Services.AddScoped<IConversationService, ConversationService>(); // Manages MongoDB operations

// SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    // options.AddMessagePackProtocol(); // Optionally add MessagePack
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication for ChatService
// This service validates tokens issued by UserAccountService
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)), // Shared secret
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer, // UserAccountService's Issuer ("https://localhost:5001")
        ValidateAudience = true,
        // ChatService should validate that the token was intended for UserAccountService (as primary audience)
        // or for ChatService itself if UserAccountService issues tokens with multiple audiences.
        // Given the docker-compose, UserAccountService issues tokens with aud "https://localhost:5001".
        // ChatService will trust tokens issued by "https://localhost:5001" for audience "https://localhost:5001".
        ValidAudience = jwtSettings.Issuer, // Audience of the token issuer (UserAccountService)
        // If UserAccountService could issue tokens with aud: "https://localhost:5002" for ChatService,
        // then ChatService's own audience (jwtSettings.Audience from its config) would be used here.
        // For simplicity with the current docker-compose, we assume ChatService trusts tokens issued by UserAccountService for UserAccountService's audience.
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    // For SignalR, token can be sent in query string
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

builder.Services.AddAuthorization();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:3000") // Frontend URL
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // Required for SignalR with credentials
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub"); // Map SignalR Hub

app.Run();