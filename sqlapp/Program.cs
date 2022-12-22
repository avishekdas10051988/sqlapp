using sqlapp.Services;

var connectionString = "Endpoint=https://featureapp1.azconfig.io;Id=+adz-l9-s0:Is5GW4O+s55KJ2pftH4r;Secret=7O98CYLxilXCMxqTgaBNUlxvqKh3Zxl2WN6k1Q3BpXk=";

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration(builder =>
{
    builder.AddAzureAppConfiguration(connectionString);
});

builder.Services.AddTransient<IProductService, ProductService>();

// Add services to the container.
builder.Services.AddRazorPages();

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
