using Infinit.Assessment.Api.Midllewares;
using Infinit.Assessment.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(builder.Configuration);

// NOTE: Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddApplicationServices();

WebApplication app = builder.Build();

// NOTES: add middlewares
app.UseMiddleware<ErrorHandlerMiddleware>();

// NOTE: Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
