using Microsoft.FeatureManagement;
using sqlapp.Services;
using StackExchange.Redis;

var connectionString = "Endpoint=https://featureapp1.azconfig.io;Id=+adz-l9-s0:Is5GW4O+s55KJ2pftH4r;Secret=7O98CYLxilXCMxqTgaBNUlxvqKh3Zxl2WN6k1Q3BpXk=";

string redisString = "appcache204.redis.cache.windows.net:6380,password=7x8b62MrqFS623pGgILs9ihRKZeOlP2tKAzCaJOFW34=,ssl=True,abortConnect=False";
var multiplexer = ConnectionMultiplexer.Connect(redisString);
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);



builder.Host.ConfigureAppConfiguration(builder =>
{
    builder.AddAzureAppConfiguration(options=> options.Connect(connectionString).UseFeatureFlags());
});

builder.Services.AddTransient<IProductService, ProductService>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddFeatureManagement();
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
