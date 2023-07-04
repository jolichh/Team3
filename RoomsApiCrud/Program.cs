using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddIdentity<RoomsApiCrud.Models.RoomsApiCrudUser, RoomsApiCrud.Models.RoomsApiCrudRole>().AddDefaultTokenProviders();

builder.Services.AddTransient<IUserStore<RoomsApiCrud.Models.RoomsApiCrudUser>, RoomsApiCrud.Data.RoomsApiCrudUserStore>();
builder.Services.AddTransient<IRoleStore<RoomsApiCrud.Models.RoomsApiCrudRole>, RoomsApiCrud.Data.RoomsApiCrudRoleStore>();
builder.Services.AddTransient<RoomsApiCrud.Services.IEmailSender, RoomsApiCrud.Services.EmailSender>();

builder.Services.AddControllers();

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
