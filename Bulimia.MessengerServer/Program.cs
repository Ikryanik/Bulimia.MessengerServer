using Bulimia.MessengerServer.BLL.Services;
using Bulimia.MessengerServer.DAL.Models;
using Bulimia.MessengerServer.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<MessengerContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<MessageRepository>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<UserManagerService>();
builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
