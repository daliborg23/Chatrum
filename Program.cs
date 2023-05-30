using Chatrum;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;



var builder = WebApplication.CreateBuilder(args);




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddCors(options => {
//    options.AddDefaultPolicy(builder => {
//        builder.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//    });
//});

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder.WithOrigins("http://daliborg.8u.cz/")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

//builder.Services.AddCors(options => {
//    options.AddDefaultPolicy(builder => {
//        builder.WithOrigins("https://test23.somee.com", "http://daliborg.8u.cz/", "http://daliborg.8u.cz/login.html", "http://daliborg.8u.cz/Chatrum")
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});
//builder.Services.AddCors(options => {
//    options.AddDefaultPolicy(builder => {
//        builder.WithOrigins("http://localhost:7251")
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});

builder.Services.AddIdentity<User, IdentityRole>() // ??
      .AddEntityFrameworkStores<UserDbContext>()
      .AddDefaultTokenProviders();
builder.Services.AddDbContext<UserDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserDbContext"))
    );
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Chatrum-Issuer",
            ValidAudience = "audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey._secretKey))
        };
    });
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();

builder.Services.AddRazorPages();
builder.Services.AddRazorPages(options => {
    options.Conventions.AuthorizePage("/Dashboard");
});



var app = builder.Build();







// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => {
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthentication();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

//app.UseCors(builder =>
//    builder
//        .AllowAnyOrigin()
//        .AllowAnyMethod()
//        .AllowAnyHeader());
//app.UseCors("AllowOrigin");


app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapRazorPages();
});

app.MapControllers();

app.Run();
