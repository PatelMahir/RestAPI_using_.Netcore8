using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RestAPI_using_.Netcore8.AutoMapper;
using RestAPI_using_.Netcore8.Data;
using RestAPI_using_.Netcore8.Models;
using RestAPI_using_.Netcore8.Repository.IRepository;
using RestAPI_using_.Netcore8.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore.Metadata.Internal;
=======
>>>>>>> 17e3a8fabd919c98a0d6580bcb3a8967631bec5b
var builder = WebApplication.CreateBuilder(args);
// Db connection config
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Add services to the container.
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(ApplicationMapper));
//Logger setup
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// builder cache
builder.Services.AddMemoryCache();
//.Net Identity Configuration
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
//Setting Authentication Code
var key = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
<<<<<<< HEAD
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
=======
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
>>>>>>> 17e3a8fabd919c98a0d6580bcb3a8967631bec5b
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
//Required for Authorization
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
<<<<<<< HEAD
//builder.Services.AddEndpointsApiExplorer();
=======
builder.Services.AddEndpointsApiExplorer();
>>>>>>> 17e3a8fabd919c98a0d6580bcb3a8967631bec5b
//Swagger Configuration
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Auth Bearer Token \r\n\r\n" +
        "Insert The token with the following format: Bearer thgashqkssuqj",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer"
            },
            new List<string>()
        }
    });
});
//CORS Policy
<<<<<<< HEAD
/*builder.Services.AddCors(p => p.AddPolicy("CorsPolicy", build =>
{
    //modify URL with required domain of the front-end app
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));*/
=======
builder.Services.AddCors(p => p.AddPolicy("CorsPolicy", build =>
{
    //modify URL with required domain of the front-end app
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
>>>>>>> 17e3a8fabd919c98a0d6580bcb3a8967631bec5b
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
<<<<<<< HEAD
//app.UseCors("CorsPolicy");/
=======
app.UseCors("CorsPolicy");
>>>>>>> 17e3a8fabd919c98a0d6580bcb3a8967631bec5b
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();