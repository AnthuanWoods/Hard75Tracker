using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string? origins = "origins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: origins,
    policy =>
    {
        policy.WithOrigins("https://localhost:7165").AllowAnyHeader().AllowAnyMethod();
        //policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials();
    });
});
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("")
    });
var app = builder.Build();
app.UseCors(origins);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
