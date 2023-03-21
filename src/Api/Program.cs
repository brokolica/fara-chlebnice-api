using Api.Authorization;
using Api.ConfigLoader;
using Application.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Load configurations.
builder.Configuration.LoadConfigurations(builder.Environment.EnvironmentName);

// Add services to the container.
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<AnnouncementsService>();

builder.Services.AddControllers();

// Add DbContext.
var connStr = builder.Configuration.GetConnectionString("FaraChlebniceDb")!;
builder.Services.AddDbContext<FaraChlebniceDbContext>(options => options.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        JwtBearerDefaults.AuthenticationScheme,
        options =>
        {
            options.Authority = builder.Configuration["Auth0:Domain"];
            options.Audience = builder.Configuration["Auth0:Audience"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,

                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Auth0:Domain"],
            };
        });

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy(
            "create:announcements",
            policy => policy.Requirements.Add(
                new HasScopeRequirement("create:announcements", builder.Configuration["Auth0:Domain"])
            ));
        options.AddPolicy(
            "update:announcements",
            policy => policy.Requirements.Add(
                new HasScopeRequirement("update:create", builder.Configuration["Auth0:Domain"])
            ));
        options.AddPolicy(
            "delete:announcements",
            policy => policy.Requirements.Add(
                new HasScopeRequirement("delete:create", builder.Configuration["Auth0:Domain"])
            ));
    });

// TODO: overit ci treba, ale .NET si to asi registruje defaultne sam.
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(x => x
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());

    app.UseSwagger();
    app.UseSwaggerUI();
    ApplyMigrations(app);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void ApplyMigrations(IHost app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FaraChlebniceDbContext>();
    db.Database.Migrate();
}