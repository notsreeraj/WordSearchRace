using server.Interfaces;
using server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Client", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// addscoped will make sure that there puzzleservice instance for each request
builder.Services.AddScoped<IPuzzleService,PuzzleService>();
builder.Services.AddSingleton<IGameService, GameService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Client");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
