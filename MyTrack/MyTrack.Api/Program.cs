using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MyTrack.Api.Middleware;
using MyTrack.Api.Services;
using MyTrack.Application.Interfaces;
using MyTrack.Application.Services;
using MyTrack.Application.Settings;
using MyTrack.Application.Validators;
using MyTrack.Infrastructure.Data;
using MyTrack.Infrastructure.Repositories;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

#region Service Registration

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProjectRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IWorkLogService, WorkLogService>();
builder.Services.AddScoped<IWorkLogRepository, WorkLogRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddDbContext<MyTrackDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyTrackDatabase")));

builder.Services.AddHealthChecks()
    .AddDbContextCheck<MyTrackDbContext>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyTrack API",
        Version = "v1",
        Description = "Personal productivity and work tracking API."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
{
    {
        new OpenApiSecuritySchemeReference("Bearer", document),
        new List<string>()
    }
});
});

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MyTrackDbContext>();
    await MyTrackDbSeeder.SeedAsync(context);
}

#region Middleware Pipeline

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

#endregion

app.Run();