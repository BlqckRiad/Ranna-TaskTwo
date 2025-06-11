using FileImageService.API.Middleware;
using FileImageService.BusinessLayer.Abstract;
using FileImageService.BusinessLayer.Concrete;
using FileImageService.BusinessLayer.Mappings;
using FileImageService.DataAccessLayer.Abstract;
using FileImageService.DataAccessLayer.Concrete;
using FileImageService.DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "File Image Service API",
		Version = "v1",
		Description = "Ürün yönetimi ve dosya işlemleri için REST API",
		Contact = new Microsoft.OpenApi.Models.OpenApiContact
		{
			Name = "API Support",
			Email = "support@example.com"
		}
	});

	// JWT Authentication için Swagger ayarları
	c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
		Name = "Authorization",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

// Database Context
builder.Services.AddDbContext<Context>();

// Dependency Injection

// Generic Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(EfGenericRepository<>));

// Product Services
builder.Services.AddScoped<IProductDal, FileImageService.DataAccessLayer.Concrete.EfProductDal>();
builder.Services.AddScoped<IProductService, ProductManager>();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSettings["Issuer"],
			ValidAudience = jwtSettings["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(key)
		};
	});

// Authorization policies
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
	options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// CORS policy
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

// JWT Middleware
//app.UseMiddleware<JwtMiddleware>();

// Use Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
