using Api.ConfigLoader;
using Application.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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

app.UseAuthorization();

app.MapControllers();

app.Run();


static void ApplyMigrations(IHost app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FaraChlebniceDbContext>();
    db.Database.Migrate();
}