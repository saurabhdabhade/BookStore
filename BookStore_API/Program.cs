using BookStore_API;
using BookStore_API.Data;
using BookStore_API.Model;
using BookStore_API.Model.Repository;
using BookStore_API.Repository;
using Microsoft.EntityFrameworkCore;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option => { option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection")); });
builder.Services.AddScoped<BookStore_API.Model.Repository.IRepository<User>,UserRepository>();
builder.Services.AddScoped<BookStore_API.Model.Repository.IRepository<Publisher>,PublisherRepository>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingConfig));

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
