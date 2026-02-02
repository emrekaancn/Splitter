using Microsoft.EntityFrameworkCore;
using Split.Data;
using Split.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=/tmp/split.db");
});

// 🔹 Services
builder.Services.AddScoped<SplitSettlementService>();

// 🔹 Controllers
builder.Services.AddControllers();

// 🔹 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 CORS (React için)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// 🔹 Middleware sırası ÖNEMLİ
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend"); // 👈 AUTH'TAN ÖNCE

app.UseAuthorization();

app.MapControllers();

app.Run();