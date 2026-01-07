using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using CookBook.Models;
using CookBook.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    { 
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
    });
builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler("/error");

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();