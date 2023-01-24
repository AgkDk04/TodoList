using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;
using TodoList;
using TodoList.Models;
using TodoList.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));

builder.Services.AddSingleton<IDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

builder.Services.AddSingleton<IJwtSettings>(sp =>
    sp.GetRequiredService<IOptions<JwtSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(sp =>
    {
        var connStr = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value?.ConnectionString;
        return new MongoClient(connStr);
    });

// Add model validator for TodoItem
builder.Services.AddScoped<IValidator<TodoItem>, TodoItemValidator>();
builder.Services.AddScoped<ITodoListService, TodoListService>();
builder.Services.AddControllersWithViews();

// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Bearer Authentication with JWT Token",
            Type = SecuritySchemeType.Http
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                }
            },
            new List < string > ()
        }
        });
    });

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        var conf = builder.Configuration.GetSection(nameof(JwtSettings));
        
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = conf["Audience"],
            ValidIssuer = conf["Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Key"]))
        };
    });

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod()
                                    .AllowAnyHeader();
        });
    });
}

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
   name: "default",
   pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.Run();
