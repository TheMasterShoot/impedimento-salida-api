using impedimento_salidaAPI.Context;
using impedimento_salidaAPI.Custom;
using impedimento_salidaAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.FileProviders;
using Microsoft.Data.SqlClient;
using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.OpenApi;


var builder = WebApplication.CreateBuilder(args);

// configuracion de politicas de cors
builder.Services.AddCors(options =>
{
    
                       options.AddPolicy("CorsPolicy", builder =>
                       {
                           builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader();
                       });
});

// Agregar soporte para JWT
builder.Services.AddSingleton<Utilities>();

builder.Services.AddAuthentication(config => {
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
    };
});

// Configuracion de los servicios para usar Newtonsoft.Json
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    // Configura opciones de serialización si es necesario
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    //options.SerializerSettings.Converters.Add(new CartaConverter());
    // Puedes agregar más configuraciones según tus necesidades
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// para envio de emails
builder.Services.AddScoped<IEmailService, EmailService>();


// Agregar AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//azure conexion
var connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");//String.Empty;
//if (builder.Environment.IsDevelopment())
//{
//    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
//    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
//}
//else
//{
//    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
//}

builder.Services.AddDbContext<ImpedimentoSalidaContext>(options =>
    options.UseSqlServer(connection));


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//app.MapGet("/Person", (ImpedimentoSalidaContext context) =>
//{
//    return context.Person.ToList();
//})
//.WithName("GetPersons")
//.WithOpenApi();

//app.MapPost("/Person", (Person person, ImpedimentoSalidaContext context) =>
//{
//    context.Add(person);
//    context.SaveChanges();
//})
//.WithName("CreatePerson")
//.WithOpenApi();

app.Run();

//public class Person
//{
//    public int Id { get; set; }
//    public string FirstName { get; set; }
//    public string LastName { get; set; }
//}

//public class PersonDbContext : DbContext
//{
//    public PersonDbContext(DbContextOptions<PersonDbContext> options)
//        : base(options)
//    {
//    }

//    public DbSet<Person> Person { get; set; }
//}