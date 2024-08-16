using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using NpsApi._2___Application.Services;
using NpsApi._3___Domain.CommandHandlers;
using NpsApi.Application.Services;
using NpsApi.Data;
using NpsApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddSingleton(new DatabaseConnection(connectionString));

builder.Services.AddScoped<FormsGroupsService>();
builder.Services.AddScoped<FormsGroupsCommandHandler>();
builder.Services.AddScoped<FormsGroupsRepository>();

builder.Services.AddScoped<FormsService>();
builder.Services.AddScoped<FormsCommandHandler>();
builder.Services.AddScoped<FormsRepository>();

builder.Services.AddScoped<QuestionsService>();
builder.Services.AddScoped<QuestionsCommandHandler>();
builder.Services.AddScoped<QuestionsRepository>();

builder.Services.AddScoped<AnswersService>();
builder.Services.AddScoped<AnswersCommandHandler>();
builder.Services.AddScoped<AnswersRepository>();

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<UsersCommandHandler>();
builder.Services.AddScoped<UsersRepository>();

builder.Services.AddScoped<NpsService>();
builder.Services.AddScoped<NpsCommandHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.Cookie.Name = "NpsProject.AuthCookie";
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = false;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "NpsProject.AuthCookie";
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = false;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdmininistradorPolicy", policy => policy.RequireRole("Administrador"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("AllowOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
