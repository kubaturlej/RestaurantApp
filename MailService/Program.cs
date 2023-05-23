var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MailConsumer>();

var app = builder.Build();

app.MapGet("/", () => "Mail Service online !");

app.Run();

var mailConsumer = app.Services.GetService<MailConsumer>();

mailConsumer.Subscribe("test topic");
