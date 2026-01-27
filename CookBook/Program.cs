using CookBook;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddSwagger()
    .AddApplicationServices();

var app = builder.Build();

app.UseExceptionHandler("/error");

app.UseSwagger();
app.UseSwaggerUI();

// ƒл€ чего нужна эта проверка? » если € буду это использовать, то наебнетс€ контейнер приложени€ в докере?
// ѕомню ты что-то где-то дописывал во врем€ лекции про докер, чтобы контейнер запустилс€.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/

app.MapControllers();

app.Run();
