using MyTrack.Application.Interfaces;
using MyTrack.Application.Services;

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