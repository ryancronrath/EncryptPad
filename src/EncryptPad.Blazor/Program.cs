using EncryptPad.Blazor.Repository;
using EncryptPad.Shared;
using EncryptPad.Shared.Models;
using SQLite;
using TextEncryption;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Create a SqliteAsyncConnection, which will create the database if it doesn't already exist.
SQLiteAsyncConnection sqliteConnection = new(DataSource.databasePath);

builder.Services.AddSingleton(new SqliteOneTimePadService(new OneTimePad(), sqliteConnection, 120));

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


app.Run();