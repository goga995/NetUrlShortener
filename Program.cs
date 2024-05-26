using Microsoft.EntityFrameworkCore;

using NetUrlShortener.Data;
using NetUrlShortener.Extensions;
using NetUrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
//EfCore je takodje scoped service
builder.Services.AddScoped<UrlShorteningService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddEndpoints();

app.UseHttpsRedirection();


app.Run();

