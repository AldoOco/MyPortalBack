using MyPortalBack.Api.Common;
using MyPortalBack.Api.Middlewares;
using MyPortalBack.Application.DependencyInjection;
using MyPortalBack.Infrastructure.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "MyPortalBack API",
        Version = "v1",
        Description = "Core API para la plataforma MyPortalBack."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(
        AppContext.BaseDirectory,
        xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
