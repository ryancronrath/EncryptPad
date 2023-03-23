using EncryptPad.Actions;
using EncryptPad.Repository;
using EncryptPad.Shared;
using EncryptPad.Shared.Models;
using SQLite;
using TextEncryption;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(new SQLiteAsyncConnection(DataSource.databasePath));
builder.Services.AddSingleton<OneTimePad>();
builder.Services.AddTransient<SqliteOneTimePadService>();
builder.Services.AddTransient<SqliteCleanupAction>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await DBInit();

async Task DBInit()
{
    if (!File.Exists(DataSource.databasePath))
    {
        var db = new SQLiteAsyncConnection(DataSource.databasePath);
        await db.CreateTableAsync<OTPKey>();
    }
}

app.Run();