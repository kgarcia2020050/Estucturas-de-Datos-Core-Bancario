using Microsoft.OpenApi.Models;
using ProyectiFinal.Services;
using System.Reflection;
using Utils.EstructuraDeDatos.Arboles.ABB;
using Utils.EstructuraDeDatos.Arboles.AVL;
using Utils.EstructuraDeDatos.DispercionesHash;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Core Bancario",
        Version = "v1",
        Description = "API RESTful modular para simular operaciones esenciales de " +
        "un sistema bancario especializado en tarjetas de crédito. Implementa estructuras " +
        "de datos avanzadas en C# y sigue patrones de diseño para garantizar escalabilidad y mantenibilidad."
    });

});

builder.Services.AddControllers();
builder.Services.AddSingleton<ClientService>();
builder.Services.AddSingleton<CardService>();

builder.Services.AddSingleton<ABB>();
builder.Services.AddSingleton<AVL>();
builder.Services.AddSingleton<Hash>();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
