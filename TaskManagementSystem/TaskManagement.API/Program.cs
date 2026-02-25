using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TaskManagement.Application.Common;
using TaskManagement.Infrastructure.DependencyInjection;
using TaskManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Management API", Version = "v1" });
    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Description = "Basic Authentication header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "basic"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
            },
            Array.Empty<string>()
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));

builder.Services.AddInfrastructure();
builder.Services.AddScoped<TaskManagement.Application.Common.Interfaces.IUserRepository, TaskManagement.Infrastructure.Repositories.UserRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseMiddleware<TaskManagement.API.Middleware.BasicAuthMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();









































































//Scaffold-DbContext "Server=ROOMI-LAPTOP;Database=TaskManagementDb;User Id=SA;Password=123456;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Persistence\Models -Context AppDbContext -UseDatabaseNames -DataAnnotations -Force