using EncryptPad.Blazor.Repository;
using EncryptPad.Shared;
using EncryptPad.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SQLite;
using TextEncryption;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton(new SQLiteAsyncConnection(DataSource.databasePath));
builder.Services.AddSingleton<OneTimePad>();
builder.Services.AddTransient<SqliteOneTimePadService>();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


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
