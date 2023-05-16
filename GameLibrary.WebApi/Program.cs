using System;
using GameLibrary.WebApi.Data;
using GameLibrary.WebApi.Helpers;
using GameLibrary.WebApi.Interfaces;
using GameLibrary.WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(c => c.AddProfile<GameMappingProfile>());
builder.Services.AddSqlServer<AppDbContext>(builder.Configuration.GetConnectionString("SqlServerConnection"));
builder.Services.AddScoped<IGameRepository, GameRepository>();

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
