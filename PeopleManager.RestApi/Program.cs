using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PeopleManager.Core;
using PeopleManager.RestApi.Authentication;
using PeopleManager.RestApi.Settings;
using PeopleManager.Services;

const string corsOrigins = "PeopleManagerCorsOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo{Title = "People Manager Api", Version = "v1"});

    var securityDefinition = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    };
    options.AddSecurityDefinition("Bearer", securityDefinition);

    var securityRequirementScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            securityRequirementScheme, new string[] { }
        }
    };
    options.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddDbContext<PeopleManagerDbContext>(options =>
{
    options.UseInMemoryDatabase(nameof(PeopleManagerDbContext));
});

var jwtSettings = new JwtSettings();
builder.Configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

// within this section we are configuring the authentication and setting the default scheme
builder.Services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwt =>
    {
        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
            IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to our Jwt encryption
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true
        };
    });

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<PeopleManagerDbContext>();

builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<VehicleService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsOrigins,
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    var scope = app.Services.CreateScope();
    var database = scope.ServiceProvider.GetRequiredService<PeopleManagerDbContext>();
    database.Seed();

}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(corsOrigins);

app.MapControllers();


app.Run();
