using Bulimia.MessengerServer.BLL.Services;
using Bulimia.MessengerServer.DAL.Models;
using Bulimia.MessengerServer.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MessengerContext>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<MessageRepository>();
builder.Services.AddSingleton<ChatService>();
builder.Services.AddSingleton<UserManagerService>();
builder.Services.AddSingleton<UserRepository>();

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
