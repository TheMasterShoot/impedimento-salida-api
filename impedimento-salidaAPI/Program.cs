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


var builder = WebApplication.CreateBuilder(args);


// configuracion de conexion a base de datos
var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<ImpedimentoSalidaContext>(
        options => options.UseSqlServer(connectionString)
);

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//           Path.Combine(builder.Environment.ContentRootPath, "Documentos")),
//    RequestPath = "/Documentos"
//});

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
