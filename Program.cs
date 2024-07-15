using Microsoft.AspNetCore.Authentication.Cookies;
using NpsApi._3___Domain.CommandHandlers;
using NpsApi.Application.Services;
using NpsApi.Data;
using NpsApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DataBaseConnection(connectionString));

builder.Services.AddScoped<FormsGroupsService>();
builder.Services.AddScoped<FormsGroupsRepository>();
builder.Services.AddScoped<FormsGroupsCommandHandler>();

builder.Services.AddScoped<FormsService>();
builder.Services.AddScoped<FormsRepository>();

builder.Services.AddScoped<QuestionsService>();
builder.Services.AddScoped<QuestionsRepository>();

builder.Services.AddScoped<AnswersService>();
builder.Services.AddScoped<AnswersRepository>();

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<UsersRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
      options.Cookie.Name = "NpsProject.AuthCookie";
      options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("AdmininistradorPolicy", policy => policy.RequireRole("Administrador"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
