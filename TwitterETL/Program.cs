using Microsoft.EntityFrameworkCore;
using TwitterETL.Context;
using TwitterETL.Repositories;
using TwitterETL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<TweetContext>(opt =>
//    opt.UseInMemoryDatabase("Tweet"));

builder.Services.AddSingleton<ITweetGeneration, TweetGeneration>();
builder.Services.AddSingleton<ITweetRepository, TweetRepository>();
builder.Services.AddSingleton<ITweetService, TweetService>();

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
