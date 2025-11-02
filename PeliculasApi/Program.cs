
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasApi;
using PeliculasApi.Data;
using PeliculasApi.Servicios;
using PeliculasApi.Utilidades;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(proveedor => new MapperConfiguration(configuracion =>{
    var geometryFactory = proveedor.GetRequiredService<GeometryFactory>();
    configuracion.AddProfile(new AutoMapperProfiles(geometryFactory));
}).CreateMapper());

builder.Services.AddDbContext<ApplicationDbContext>(opciones=>
    opciones.UseSqlServer("name=DefaultConnection", sqlServer =>
    sqlServer.UseNetTopologySuite()));

builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
});

var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!.Split(",");
//var origenesPermitidos = new[] { "http://localhost:4200" };

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(opcionesCORS =>
    {
        opcionesCORS.WithOrigins(origenesPermitidos).AllowAnyMethod().AllowAnyHeader()
        .WithExposedHeaders("cantidadTotalRegistros");
    });
});

builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseOutputCache();

app.Run();
