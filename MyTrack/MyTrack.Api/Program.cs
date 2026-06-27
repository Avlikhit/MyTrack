using Microsoft.EntityFrameworkCore;
using MyTrack.Application.Interfaces;
using MyTrack.Application.Services;
using MyTrack.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

#region Service Registration

// Add framework services.
builder.Services.AddControllers();

// Register application services.
builder.Services.AddScoped<IWorkLogService, WorkLogService>();

// Register Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

builder.Services.AddDbContext<MyTrackDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyTrackDatabase")));
var app = builder.Build();

#region Middleware Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();