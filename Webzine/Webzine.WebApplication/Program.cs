// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Ajoute les services n�cessaires pour permettfre l'utilisation des
// controllers avec des vues.
builder.Services.AddControllersWithViews()
    // Ajoute la compilation des vues lors de l'ex�cution de l'application.
    // Cela nous �vite de recompiler l'application � chaque modification de vue.
    // N�cessite le package Nuget Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation.
    .AddRazorRuntimeCompilation();

var app = builder.Build();

// Ajoute les fichiers statiques (Bootstrap, CSS, JS)
app.UseStaticFiles();

app.UseRouting();

// Configure le routage pour les zones (areas)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Configure le routage par défaut
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");

app.Run();
