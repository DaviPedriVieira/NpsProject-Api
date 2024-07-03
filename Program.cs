using NpsApi.Data;
using NpsApi.Repositories;
using NpsApi.Services;

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
builder.Services.AddScoped<FormsService>();
builder.Services.AddScoped<FormsRepository>();
builder.Services.AddScoped<QuestionsService>();
builder.Services.AddScoped<QuestionsRepository>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<AnswersService>();
builder.Services.AddScoped<AnswersRepository>();

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
