using DevTrackR.API.Persistence;
using DevTrackR.API.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration.GetConnectionString("DevTrackRCs");
builder.Services.AddDbContext<DevTrackRContext>(o => o.UseSqlServer(connString));

builder.Services.AddScoped<IPackageRepository, PackageRepository>();

var sendGridApiKey = builder.Configuration.GetSection("SendGridApiKey").Value;
builder.Services.AddSendGrid(o => o.ApiKey = sendGridApiKey);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(op =>
{
    op.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevTrackR.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Gabriel",
            Email = "gabriel-rodriguesbs@hotmail.com",
            Url = new Uri("https://github.com/BrouGabri3l")
        }
    });
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "DevTrackR.API.xml");
    op.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
