using CookBook.Abstractions;
using CookBook.Models;
using CookBook.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
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

builder.Services.AddSingleton<ITimeConverter, TimeConverter>();
builder.Services.AddSingleton<IIngredientRepository, IngredientRepository>();
builder.Services.AddSingleton<IIngredientInRecipeRepository, IngredientInRecipeRepository>();
builder.Services.AddSingleton<IRecipeRepository, RecipeRepository>();

var app = builder.Build();

app.UseExceptionHandler("/error");

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
