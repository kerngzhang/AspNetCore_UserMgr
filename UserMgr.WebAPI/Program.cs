using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserMgr.Domain;
using UserMgr.Infrastracture;
using UserMgr.Infrastracture.Repository;
using UserMgr.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AddDbContext
builder.Services.AddDbContext<UserDbContext>(opt =>
{
    opt.UseSqlServer("Server.;Database=ddd1;Trusted_Connection=True;");
});

//Add Filter
builder.Services.Configure<MvcOptions>(o =>
{
    o.Filters.Add<UnitOfWorkFilter>();
});

builder.Services.AddDistributedMemoryCache();
//Add MediaR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddScoped<UserDomainService>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<ISmsCodeSender, MockSmsCodeSender>();

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
