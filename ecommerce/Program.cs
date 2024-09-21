using ecommerce.Authservices;
using ecommerce.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var mycon = builder.Configuration.GetConnectionString("mycon");
builder.Services.AddDbContext<Appdbcontext>(x => x.UseSqlServer(mycon));

builder.Services.AddIdentity<Appuser,IdentityRole>().AddEntityFrameworkStores<Appdbcontext>();
builder.Services.AddScoped<IAuthModel, AuthModel>();
builder.Services.AddScoped<IitemsCRUD,ItemCRUD>();
builder.Services.AddScoped<ICategCRUD, CategCRUD>();
builder.Services.AddScoped<Iorder,OrderServices>();


builder.Services.AddCors(op =>
{
    op.AddPolicy("Mypolicy", op =>
    {
        op.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme =JwtBearerDefaults.AuthenticationScheme;
    op.DefaultScheme =JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
{
    op.SaveToken = true;
    op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"])),

       ValidIssuer = builder.Configuration["jwt:issuer"],
       ValidAudience = builder.Configuration["jwt:audience"],
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.UseCors("Mypolicy");
app.MapControllers();

app.Run();
