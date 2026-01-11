using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using Nedo.AspNet.Request.Validation.Extensions;
using TodoApi.Filters;

var builder = WebApplication.CreateBuilder(args);

//add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRequestValidation();

// Register filter sebagai service
builder.Services.AddScoped<RequestValidationFilter>();

// Add controllers dengan filter global
builder.Services.AddControllers(options =>
{
    options.Filters.Add<RequestValidationFilter>();
});

//add DbContext
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthorization();
app.Run();