using server.Interfaces;
using server.Middleware;
using server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// cors setup
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

// everyting is singleton 
builder.Services.AddSingleton<IPuzzleService,PuzzleService>();
builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddSingleton<IWordService , WordService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("Client");

app.UseAuthorization();

app.MapControllers();

app.Run();
