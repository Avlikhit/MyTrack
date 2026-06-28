using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MyTrack.Api.Middleware;
using MyTrack.Application.Interfaces;
using MyTrack.Application.Services;
using MyTrack.Application.Validators;
using MyTrack.Infrastructure.Data;
using MyTrack.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

#region Service Registration

// Add framework services.
builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProjectRequestValidator>();

builder.Services.AddFluentValidationAutoValidation();

// Register application services.
builder.Services.AddScoped<IWorkLogService, WorkLogService>();
builder.Services.AddScoped<IWorkLogRepository, WorkLogRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();

// Register Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

builder.Services.AddDbContext<MyTrackDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyTrackDatabase")));

// Health checks
builder.Services.AddHealthChecks().AddDbContextCheck<MyTrackDbContext>();

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

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

#endregion

app.Run();