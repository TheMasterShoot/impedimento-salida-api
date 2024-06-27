using impedimento_salidaAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<ImpedimentoSalidaContext>(
        options => options.UseSqlServer(connectionString)
);

builder.Services.AddCors(options =>
{
    
                       options.AddPolicy("CorsPolicy", builder =>
                       {
                           builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader();
                       });
});

// Configura los servicios para usar Newtonsoft.Json
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    // Configura opciones de serialización si es necesario
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    //options.SerializerSettings.Converters.Add(new CartaConverter());
    // Puedes agregar más configuraciones según tus necesidades
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
